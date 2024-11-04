using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SdWrapCore.Utils;
using SdWrapCore.SdWrap.Crypto;
using SdWrapCore.SdWrap.Hash;

namespace SdWrapCore.SdWrap
{
    /// <summary>
    /// SdWrap版本等级
    /// </summary>
    public enum SdWrapLevel : uint
    {
        V1,
        V2,
        V3,
        V4,
        V5,
        V6,
        V7,
    }

    /// <summary>
    /// SdWrapStub返回值
    /// </summary>
    public enum SdWrapStubResult : uint
    {
        /// <summary>
        /// 成功
        /// </summary>
        Successed = 0u,

        /// <summary>
        /// 数据无效
        /// </summary>
        StubInvalid = 0xC0000000u,
        /// <summary>
        /// 未知版本
        /// </summary>
        StubUnknowVersion = 0xC0000001u,
        /// <summary>
        /// 数据校验失败
        /// </summary>
        StubHashInvalid = 0xC0000002u,
    }

    /// <summary>
    /// SdWrapStub枚举扩展
    /// </summary>
    public static class SdWrapStubEnumExtend
    {
        /// <summary>
        /// 检测SdWrapStubResult值是否成功
        /// </summary>
        public static bool Successed(this SdWrapStubResult value)
        {
            return value == SdWrapStubResult.Successed;
        }

        /// <summary>
        /// 获取SdWrapStubResult错误信息
        /// </summary>
        public static string ErrorMessage(this SdWrapStubResult value)
        {
            return value switch
            {
                SdWrapStubResult.StubInvalid => "Stub数据无效",
                SdWrapStubResult.StubUnknowVersion => "Stub版本未知",
                SdWrapStubResult.StubHashInvalid => "Stub数据校验失败",
                _ => string.Empty,
            };
        }
    }

    /// <summary>
    /// SdWrap信息
    /// </summary>
    public abstract class SdWrapStub
    {
        /// <summary>
        /// 版本等级
        /// </summary>
        public abstract SdWrapLevel Level { get; }
        /// <summary>
        /// 信息大小
        /// </summary>
        public abstract uint Size { get; }
        /// <summary>
        /// 对齐大小
        /// </summary>
        public abstract uint AlignSize { get; }

        /// <summary>
        /// 解析补丁参数
        /// </summary>
        /// <param name="args">参数数据</param>
        /// <returns>True解析成功 False解析失败</returns>
        protected abstract SdWrapStubResult ParsePatchArguments(in ReadOnlySpan<byte> args);

        protected SdWrapHeader mHeader;                         //信息头
        protected SdWrapConfig mConfig;                         //配置信息
        protected List<SdWrapPatch> mPatchList = new(256);      //补丁列表
        protected string mExecutableFileName = string.Empty;    //可执行文件名

        /// <summary>
        /// 获取SdWrap配置信息
        /// </summary>
        public SdWrapConfig Config => this.mConfig;
        /// <summary>
        /// 获取补丁信息
        /// </summary>
        public ReadOnlyCollection<SdWrapPatch> Patches => this.mPatchList.AsReadOnly();
        /// <summary>
        /// 主程序名称
        /// </summary>
        public string ExecutableFileName
        {
            get
            {
                SdWrapFlags mode = this.mConfig.Mode;
                if ((mode.HasFlag(SdWrapFlags.UseExecutableFileNameArgument) || mode.HasFlag(SdWrapFlags.ExecutableFileNotPack)) && !string.IsNullOrEmpty(this.mExecutableFileName))
                {
                    return this.mExecutableFileName;
                }
                else
                {
                    return "main.bin";
                }
            }
        }

        /// <summary>
        /// 解密资源
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="key">key</param>
        /// <param name="signature1">标记1</param>
        /// <param name="signature2">标记2</param>
        /// <param name="packedMode">True加密模式 False解密模式</param>
        /// <returns>True处理成功 False标记检查错误</returns>
        public virtual bool DecryptResource(in Span<byte> data, uint key, uint signature1, uint signature2, bool packedMode)
        {
            uint sig = MemoryMarshal.Read<uint>(data);
            if (packedMode)
            {
                //封包模式(加密模式)
                if (sig == signature1)
                {
                    MemoryMarshal.Write(data, ref signature2);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //解包模式(解密模式)
                if (sig == signature2)
                {
                    MemoryMarshal.Write(data, ref signature1);
                }
                else
                {
                    return false;
                }
            }

            if (data.Length > 4)
            {
                SdWrapDecryptorV1.Decrypt(data[4..], key);
            }
            return true;
        }

        /// <summary>
        /// 设置上下文参数
        /// </summary>
        /// <param name="header">数据头</param>
        /// <param name="args">原数据</param>
        public virtual unsafe SdWrapStubResult SetArguments(in SdWrapHeader header, byte[] args)
        {
            this.mHeader = header;

            this.Decrypt(args);
            if (!this.CheckHash(args))
            {
                return SdWrapStubResult.StubHashInvalid;
            }

            //获取配置
            int cfgSize = Unsafe.SizeOf<SdWrapConfig>();
            this.mConfig = MemoryMarshal.Read<SdWrapConfig>(args);


            SdWrapStubResult res = this.ParsePatchArguments(args.AsSpan()[cfgSize..]);

            //刷新补丁模式
            if (res.Successed())
            {
                this.RefreshPatchMode();
            }

            return res;
        }

        /// <summary>
        /// 刷新补丁模式
        /// </summary>
        protected virtual void RefreshPatchMode()
        {
            List<SdWrapPatch> patches = this.mPatchList;

            if (this.mConfig.Mode.HasFlag(SdWrapFlags.UseTempPath))
            {
                //使用临时路径  只加密Exe
                foreach(SdWrapPatch swp in patches)
                {
                    if(swp.Position != 0u || !string.IsNullOrEmpty(swp.FileName))
                    {
                        swp.Mode = SdWrapPatchFlags.ExecutableOnly;
                    }
                    else
                    {
                        swp.Mode = SdWrapPatchFlags.None;
                    }
                }
            }
            else
            {
                //不使用临时路径  加密文件/加密主程序内存
                foreach(SdWrapPatch swp in patches)
                {
                    if (swp.Position != 0u || !string.IsNullOrEmpty(swp.FileName))
                    {
                        if (!string.IsNullOrEmpty(swp.FileName))
                        {
                            swp.Mode = SdWrapPatchFlags.File;
                        }
                        else
                        {
                            swp.Mode = SdWrapPatchFlags.Memory;
                        }
                    }
                    else
                    {
                        swp.Mode = SdWrapPatchFlags.None;
                    }
                }
            }
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="bytes">数据</param>
        protected virtual void Decrypt(in Span<byte> bytes)
        {
            SdWrapDecryptorV1.Decrypt(bytes, this.mHeader.Key);
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="bytes">数据</param>
        /// <returns>True校验成功 False校验失败</returns>
        protected virtual bool CheckHash(in ReadOnlySpan<byte> bytes)
        {
            return CRC32.Hash(bytes) == this.mHeader.Hash;
        }


        /// <summary>
        /// 创建SdWrap对象
        /// </summary>
        /// <param name="s">原始流</param>
        /// <param name="obj">SdWrap对象</param>
        public unsafe static SdWrapStubResult CreateFactory(Stream s, out SdWrapStub? obj)
        {
            obj = null;

            //读取头部
            SdWrapHeader header;
            {
                int size = Unsafe.SizeOf<SdWrapHeader>();
                if (s.Read(new(&header, size)) != size)
                {
                    return SdWrapStubResult.StubInvalid;
                }
            }

            //创建SdWarp对象
            SdWrapStub stub;
            switch (header.Version)
            {
                case 0x00576453u:
                {
                    stub = new SdWrapStubV1();
                    break;
                }
                case 0x32576453u:
                {
                    stub = new SdWrapStubV2();
                    break;
                }
                case 0x33576453u:
                {
                    stub = new SdWrapStubV3();
                    break;
                }
                case 0x34576453u:
                {
                    stub = new SdWrapStubV4();
                    break;
                }
                case 0x35576453u:
                {
                    stub = new SdWrapStubV5();
                    break;
                }
                case 0x36576453u:
                {
                    stub = new SdWrapStubV6();
                    break;
                }
                case 0x37576453u:
                {
                    stub = new SdWrapStubV7();
                    break;
                }
                default:
                {
                    return SdWrapStubResult.StubUnknowVersion;
                }
            }

            //读取SdWrap参数
            byte[] args = new byte[stub.Size];
            if (s.Read(args) != args.Length)
            {
                return SdWrapStubResult.StubInvalid;
            }

            //初始化参数
            SdWrapStubResult result = stub.SetArguments(header, args);
            if (result == SdWrapStubResult.Successed)
            {
                obj = stub;
            }
            return result;
        }
    }

    internal class SdWrapStubV1 : SdWrapStub
    {
        public override SdWrapLevel Level => SdWrapLevel.V1;

        public override uint Size => 0x140u;

        public override uint AlignSize => 0x1000u;

        protected override SdWrapStubResult ParsePatchArguments(in ReadOnlySpan<byte> args)
        {
            //V1版本无加密功能 16组补丁 仅仅替换4字节Key值
            List<SdWrapPatch> patches = this.mPatchList;

            int argStructSize = Unsafe.SizeOf<SdWrapPatchV1>();
            for (int i = 0; i < 16; ++i)
            {
                SdWrapPatchV1 curArg = MemoryMarshal.Read<SdWrapPatchV1>(args.Slice(i * argStructSize, argStructSize));

                patches.Add(new()
                {
                    FileName = string.Empty,
                    Position = curArg.Position,
                    Length = 0u,
                    Signature1 = curArg.Signature1,
                    Signature2 = curArg.Signature2,
                    Reserve1 = 0u,
                });
            }
            return SdWrapStubResult.Successed;
        }
    }

    internal class SdWrapStubV2 : SdWrapStubV1
    {
        public override SdWrapLevel Level => SdWrapLevel.V2;

        public override uint Size => 0x1180u;

        public override uint AlignSize => 0x2000u;

        protected override SdWrapStubResult ParsePatchArguments(in ReadOnlySpan<byte> args)
        {
            //V2版本含加密功能 16组补丁 且有4字节Key
            List<SdWrapPatch> patches = this.mPatchList;

            int argStructSize = Unsafe.SizeOf<SdWrapPatchV2>();
            for (int i = 0; i < 16; ++i)
            {
                SdWrapPatchV2 curArg = MemoryMarshal.Read<SdWrapPatchV2>(args.Slice(i * argStructSize, argStructSize));

                patches.Add(new()
                {
                    FileName = curArg.FileName,
                    Position = curArg.Position,
                    Length = curArg.Length,
                    Signature1 = curArg.Signature1,
                    Signature2 = curArg.Signature2,
                    Reserve1 = curArg.Reserve1,
                });
            }
            return SdWrapStubResult.Successed;
        }
    }

    internal class SdWrapStubV3 : SdWrapStubV2
    {
        public override SdWrapLevel Level => SdWrapLevel.V3;

        public override uint Size => 0x1280u;

        protected override SdWrapStubResult ParsePatchArguments(in ReadOnlySpan<byte> args)
        {
            //V3版本比V2多了个0x100字节的Exe名称参数
            this.mExecutableFileName = args[^0x100..].ReadASCIIString(932, -1);

            return base.ParsePatchArguments(args[..^0x100]);
        }
    }

    internal class SdWrapStubV4 : SdWrapStubV3
    {
        public override SdWrapLevel Level => SdWrapLevel.V4;

        public override uint Size => 0x9540u;

        public override uint AlignSize => 0x10000u;

        protected override SdWrapStubResult ParsePatchArguments(in ReadOnlySpan<byte> args)
        {
            //V4版本 256组解密补丁 含0x100字节Exe名称参数
            List<SdWrapPatch> patches = this.mPatchList;

            int argStructSize = Unsafe.SizeOf<SdWrapPatchV3>();
            for (int i = 0; i < 256; ++i)
            {
                SdWrapPatchV3 curArg = MemoryMarshal.Read<SdWrapPatchV3>(args.Slice(i * argStructSize, argStructSize));

                patches.Add(new()
                {
                    FileName = curArg.FileName,
                    Position = curArg.Position,
                    Length = curArg.Length,
                    Signature1 = curArg.Signature1,
                    Signature2 = curArg.Signature2,
                    Reserve1 = curArg.Reserve1,
                });
            }

            //加密参数 最后0x100字节  Exe名称参数
            this.mExecutableFileName = args[^0x100..].ReadASCIIString(932, -1);

            return SdWrapStubResult.Successed;
        }
    }

    internal class SdWrapStubV5 : SdWrapStubV4
    {
        public override SdWrapLevel Level => SdWrapLevel.V5;
    }

    internal class SdWrapStubV6 : SdWrapStubV5
    {
        public override SdWrapLevel Level => SdWrapLevel.V6;
    }

    internal class SdWrapStubV7 : SdWrapStubV6
    {
        public override SdWrapLevel Level => SdWrapLevel.V7;
    }
}
