using System;
using System.Collections.ObjectModel;
using System.IO;
using SdWrapCore.PE;

namespace SdWrapCore.SdWrap
{
    /// <summary>
    /// 软电池程序
    /// </summary>
    public class SdWrapProgram
    {
        private SdWrapStub? mStub = null;
        private byte[] mExecutableFileBytes = Array.Empty<byte>();
        private PEFile? mExecutablePE = null;
        private string mCurrentDirectory = string.Empty;
        private string mFileName = string.Empty;
        private uint mSize;

        private string mLastError = string.Empty;
        private bool mIsValid = false;

        /// <summary>
        /// 获取软电池配置信息
        /// </summary>
        public SdWrapStub? Stub => this.mStub;

        /// <summary>
        /// 获取主程序类型
        /// </summary>
        public string ExecutableVersion
        {
            get
            {
                if(this.mExecutablePE is PEFile pe)
                {
                    return pe.ToString();
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取主程序大小
        /// </summary>
        public int ExecutableSize => this.mExecutableFileBytes.Length;

        /// <summary>
        /// 获取软电池程序大小
        /// </summary>
        public uint Size => this.mSize;
        /// <summary>
        /// 获取最后错误信息
        /// </summary>
        public string LastError => this.mLastError;
        /// <summary>
        /// 获取是否可用
        /// </summary>
        public bool IsValid => this.mIsValid;

        /// <summary>
        /// 加载SdWrap文件
        /// </summary>
        /// <param name="executablePath">主程序文件路径</param>
        public bool Load(string executablePath)
        {
            this.Clear();

            string errMsg = string.Empty;
            bool result = false;

            if (File.Exists(executablePath))
            {
                string curDirectory = Path.GetDirectoryName(executablePath)!;
                this.mCurrentDirectory = curDirectory;
                this.mFileName = Path.GetFileName(executablePath);

                //读取主程序
                byte[] sdwrapBytes = File.ReadAllBytes(executablePath);
                using MemoryStream sdwrapStream = new(sdwrapBytes, false);

                //软电池壳体目前只有32位
                PEFile32 sdwrapPE = new();
                if (sdwrapPE.Load(sdwrapBytes))
                {
                    //软电池配置在附加数据段
                    uint programSize = sdwrapPE.OverlayDataFileOffset;
                    this.mSize = programSize;

                    sdwrapStream.Position = programSize;

                    //加载软电池配置
                    SdWrapStubResult stubResult = SdWrapStub.CreateFactory(sdwrapStream, out SdWrapStub? obj);
                    if (stubResult.Successed())
                    {
                        this.mStub = obj;
                        bool exeCanFind = false;

                        if (obj!.Config.Mode.HasFlag(SdWrapFlags.ExecutableFileNotPack))
                        {
                            //不加壳模式  读取真实Exe
                            string p = Path.Combine(curDirectory, obj.ExecutableFileName);
                            if (File.Exists(p))
                            {
                                this.mExecutableFileBytes = File.ReadAllBytes(p);
                                exeCanFind = true;
                            }
                            else
                            {
                                errMsg = "原始Exe文件不存在";
                            }
                        }
                        else
                        {
                            //加壳模式  位于壳体参数段后方
                            sdwrapStream.Position = programSize + obj.AlignSize;

                            byte[] exeData = new byte[sdwrapStream.Length - sdwrapStream.Position];
                            sdwrapStream.Read(exeData);
                            this.mExecutableFileBytes = exeData;

                            exeCanFind = true;
                        }

                        if (exeCanFind)
                        {
                            //解析主程序
                            byte[] data = this.mExecutableFileBytes;

                            PEFile? exePE = null;
                            PEFile32 pe32 = new();
                            PEFile64 pe64 = new();
                            if (pe32.Load(data))
                            {
                                exePE = pe32;
                            }
                            else if (pe64.Load(data))
                            {
                                exePE = pe64;
                            }
                            
                            if (exePE is not null)
                            {
                                this.mExecutablePE = exePE;
                                result = true;
                            }
                            else
                            {
                                errMsg = "主程序不是PE文件";
                            }
                        }
                    }
                    else
                    {
                        errMsg = stubResult.ErrorMessage();
                    }
                }
                else
                {
                    errMsg = "SdWrap仅支持32位";
                }
            }
            else
            {
                errMsg = "文件不存在";
            }

            this.mLastError = errMsg;
            this.mIsValid = result;
            return result;
        }

        /// <summary>
        /// 提取文件
        /// </summary>
        public bool Extract()
        {
            string errMsg = string.Empty;
            bool result = false;

            if (this.mIsValid)
            {
                PEFile exePE = this.mExecutablePE!;
                SdWrapStub stub = this.mStub!;
                ReadOnlyCollection<SdWrapPatch> patches = stub.Patches;

                string gameDir = this.mCurrentDirectory;
                string outDir = Path.Combine(gameDir, "SdWrap_Extract");

                //深拷贝一份Exe副本
                byte[] exeBytes = new byte[this.mExecutableFileBytes.Length];
                this.mExecutableFileBytes.CopyTo(exeBytes, 0L);
                Span<byte> exeBytesPtr = exeBytes;

                //错误的补丁
                bool[] errPatches = new bool[patches.Count];

                //解密
                for(int i = 0; i < patches.Count; ++i)
                {
                    SdWrapPatch swp = patches[i];
                    switch (swp.Mode)
                    {
                        case SdWrapPatchFlags.ExecutableOnly:
                        {
                            //解密Exe区块
                            Span<byte> ptr = exeBytesPtr.Slice((int)swp.Position, (int)swp.Length);
                            if (!stub.DecryptResource(ptr, swp.Position, swp.Signature1, swp.Signature2, false))
                            {
                                errPatches[i] = true;
                            }
                            break;
                        }
                        case SdWrapPatchFlags.File:
                        {
                            //Exe单独处理
                            if (swp.FileName == stub.ExecutableFileName)
                            {
                                //解密Exe区块
                                Span<byte> ptr = exeBytesPtr.Slice((int)swp.Position, (int)swp.Length);
                                if (!stub.DecryptResource(ptr, swp.Position, swp.Signature1, swp.Signature2, false))
                                {
                                    errPatches[i] = true;
                                }
                            }
                            else
                            {
                                string srcPath = Path.Combine(gameDir, swp.FileName);
                                string destPath = Path.Combine(outDir, swp.FileName);
                                if (File.Exists(srcPath))
                                {
                                    {
                                        if (Path.GetDirectoryName(destPath) is string dir && !Directory.Exists(dir))
                                        {
                                            Directory.CreateDirectory(dir);
                                        }
                                    }

                                    using FileStream inFs = File.OpenRead(srcPath);
                                    using FileStream outFs = File.Create(destPath);

                                    //读取数据块
                                    byte[] block = new byte[swp.Length];
                                    inFs.Position = swp.Position;
                                    inFs.Read(block);

                                    //解密数据块
                                    if (!stub.DecryptResource(block, swp.Position, swp.Signature1, swp.Signature2, false))
                                    {
                                        errPatches[i] = true;
                                    }

                                    //复制文件
                                    inFs.Position = 0L;
                                    inFs.CopyTo(outFs);

                                    //数据块回写
                                    outFs.Position = swp.Position;
                                    outFs.Write(block);
                                    outFs.Flush();
                                }
                                else
                                {
                                    errPatches[i] = true;
                                }
                            }
                            break;
                        }
                        case SdWrapPatchFlags.Memory:
                        {
                            uint foa = exePE.RVAToFOA(swp.Position);
                            Span<byte> ptr = exeBytesPtr.Slice((int)foa, (int)swp.Length);

                            //解密Exe区块
                            if (!stub.DecryptResource(ptr, swp.Position, swp.Signature1, swp.Signature2, false))
                            {
                                errPatches[i] = true;
                            }
                            break;
                        }
                    }
                }

                //回写Exe
                {
                    string exeDestPath = Path.Combine(outDir, this.mFileName);
                    if(Path.GetDirectoryName(exeDestPath) is string dir && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    using FileStream exeFs = File.Create(exeDestPath);
                    exeFs.Write(exeBytesPtr);
                    exeFs.Flush();
                }

                //检查错误的补丁
                result = true;
                for(int i = 0; i < errPatches.Length; ++i)
                {
                    if (i != 0 && i % 16 == 0)
                    {
                        errMsg += "\r\n";
                    }
                    if (errPatches[i])
                    {
                        errMsg += $"{i}, ";
                        result &= false;
                    }
                }
            }
            else
            {
                errMsg = "SdWrap未初始化";
            }

            this.mLastError = errMsg;
            return result;
        }

        /// <summary>
        /// 清除
        /// </summary>
        private void Clear()
        {
            this.mStub = null;
            this.mExecutableFileBytes = Array.Empty<byte>();
            this.mExecutablePE = null;
            this.mCurrentDirectory = string.Empty;
            this.mFileName = string.Empty;
            this.mSize = 0u;
            this.mLastError = string.Empty;
            this.mIsValid = false;
        }
    }
}
