using System;
using System.Runtime.InteropServices;
using SdWrapCore.Utils;

namespace SdWrapCore.SdWrap
{
    /// <summary>
    /// SdWrap枚举值
    /// </summary>
    [Flags]
    public enum SdWrapFlags : uint
    {
        /// <summary>
        /// 命令行使用短路径
        /// </summary>
        UseCommandLineShortPath = 0x00000001u,
        /// <summary>
        /// 使用默认显示设置
        /// </summary>
        UseDefaultDisplaySetting = 0x00000002u,
        /// <summary>
        /// 自动验证运行
        /// </summary>
        AutoVerify = 0x00000004u,
        /// <summary>
        /// 使用主程序Exe文件名参数
        /// </summary>
        UseExecutableFileNameArgument = 0x00000008u,
        /// <summary>
        /// 允许多开
        /// </summary>
        AllowMultiProcess = 0x00000010u,
        /// <summary>
        /// 软电池充电对话框
        /// </summary>
        UCScidChargeDialog = 0x00000020u,
        /// <summary>
        /// 没有原Exe进行加壳
        /// <para>加壳: SdWrap与原Exe进行打包</para>
        /// <para>不加壳: SdWrap与原Exe分开两个Exe</para>
        /// </summary>
        ExecutableFileNotPack = 0x00000040u,
        /// <summary>
        /// 显示软电池对话框
        /// </summary>
        ShowSoftDCDialog = 0x00000080u,
        /// <summary>
        /// 使用临时路径
        /// <para>释放Exe到临时路径</para>
        /// </summary>
        UseTempPath = 0x00000100u,
        /// <summary>
        /// 使用锁定文件模式
        /// <para>防止复制修改</para>
        /// </summary>
        UseLockFile = 0x00000200u,
        /// <summary>
        /// 软电池版本3
        /// </summary>
        UCDialogVersion3 = 0x00000400u,
    }

    /// <summary>
    /// SdWrap信息头
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0x10)]
    public struct SdWrapHeader
    {
        private uint mVersion;
        private uint mHash;
        private uint mKey;
        private uint mReserve1;

        /// <summary>
        /// 版本
        /// </summary>
        public readonly uint Version => this.mVersion;
        /// <summary>
        /// 校验Hash
        /// </summary>
        public readonly uint Hash => this.mHash;
        /// <summary>
        /// 解密Key
        /// </summary>
        public readonly uint Key => this.mKey;
    }

    /// <summary>
    /// SdWrap配置
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0x40)]
    public unsafe struct SdWrapConfig
    {
        private fixed byte mReserve[0x3C];
        private SdWrapFlags mMode;

        /// <summary>
        /// 获取模式
        /// </summary>
        public readonly SdWrapFlags Mode => this.mMode;
    }

    /// <summary>
    /// SdWrap补丁类型
    /// </summary>
    public enum SdWrapPatchFlags : uint
    {
        /// <summary>
        /// 不加密
        /// </summary>
        None,
        /// <summary>
        /// 只加密主程序Exe
        /// </summary>
        ExecutableOnly,
        /// <summary>
        /// 文件加密
        /// </summary>
        File,
        /// <summary>
        /// 内存加密
        /// </summary>
        Memory,
    }

    /// <summary>
    /// SdWrap补丁
    /// </summary>
    public class SdWrapPatch
    {
        /// <summary>
        /// 获取补丁文件名
        /// </summary>
        public string FileName { get; init; } = string.Empty;
        /// <summary>
        /// 获取补丁位置
        /// <para>文件模式: 该值为FOA</para>
        /// <para>内存模式: 该值为RVA</para>
        /// </summary>
        public uint Position { get; init; }
        /// <summary>
        /// 获取补丁长度
        /// </summary>
        public uint Length { get; init; }

        /// <summary>
        /// 获取数据标记1
        /// </summary>
        public uint Signature1 { get; init; }
        /// <summary>
        /// 获取数据标记2
        /// </summary>
        public uint Signature2 { get; init; }

        public uint Reserve1 { get; init; }


        /// <summary>
        /// 加密模式
        /// </summary>
        public SdWrapPatchFlags Mode { get; internal set; }
    }

    /// <summary>
    /// SdWrap补丁V1
    /// <para>SdWrapStubV1使用</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0x10)]
    internal struct SdWrapPatchV1
    {
        private uint mPosition;
        private uint mSignature1;
        private uint mSignature2;
        private uint mReserve1;

        public readonly uint Position => this.mPosition;
        public readonly uint Signature1 => this.mSignature1;
        public readonly uint Signature2 => this.mSignature2;
    }

    /// <summary>
    /// SdWrap补丁V2
    /// <para>SdWrapStubV2-3使用</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0x114)]
    internal unsafe struct SdWrapPatchV2
    {
        private fixed byte mFileName[0x100];
        private uint mPosition;
        private uint mSignature1;
        private uint mLength;
        private uint mSignature2;
        private uint mReserve1;

        public readonly string FileName
        {
            get
            {
                fixed(byte* ptr = this.mFileName)
                {
                    ReadOnlySpan<byte> bytes = new(ptr, 0x100);
                    return bytes.ReadASCIIString(932, 0x80);
                }
            }
        }
        public readonly uint Position => this.mPosition;
        public readonly uint Signature1 => this.mSignature1;
        public readonly uint Length => this.mLength;
        public readonly uint Signature2 => this.mSignature2;
        public readonly uint Reserve1 => this.mReserve1;
    }

    /// <summary>
    /// SdWrap补丁V3
    /// <para>SdWrapStubV4-7使用</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0x94)]
    internal unsafe struct SdWrapPatchV3
    {
        private fixed byte mFileName[0x80];
        private uint mPosition;
        private uint mSignature1;
        private uint mLength;
        private uint mSignature2;
        private uint mReserve1;

        public readonly string FileName
        {
            get
            {
                fixed (byte* ptr = this.mFileName)
                {
                    ReadOnlySpan<byte> bytes = new(ptr, 0x80);
                    return bytes.ReadASCIIString(932, -1);
                }
            }
        }
        public readonly uint Position => this.mPosition;
        public readonly uint Signature1 => this.mSignature1;
        public readonly uint Length => this.mLength;
        public readonly uint Signature2 => this.mSignature2;
        public readonly uint Reserve1 => this.mReserve1;
    }
}
