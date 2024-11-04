using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SdWrapCore.PE
{
    /// <summary>
    /// PE文件
    /// </summary>
    public class PEFile
    {
        private ImageDosHeader mImageDosHeader;
        private ImageFileHeader mImageFileHeader;
        private readonly List<ImageSectionHeader> mImageSectionHeaders = new();

        public ImageDosHeader ImageDosHeader => this.mImageDosHeader;
        public ImageFileHeader ImageFileHeader => this.mImageFileHeader;
        public ReadOnlyCollection<ImageSectionHeader> ImageSectionHeaders => this.mImageSectionHeaders.AsReadOnly();

        /// <summary>
        /// 附加数据文件偏移
        /// </summary>
        public uint OverlayDataFileOffset
        {
            get
            {
                List<ImageSectionHeader> sections = this.mImageSectionHeaders;
                if (!sections.Any())
                {
                    return 0u;
                }

                ImageSectionHeader lastSec = sections.Last();

                return lastSec.PointerOfRawData + lastSec.SizeOfRawData;
            }
        }

        /// <summary>
        /// 加载PE
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>True加载成功 False非法PE</returns>
        public virtual unsafe bool Load(in ReadOnlySpan<byte> data)
        {
            this.Clear();

            int len = data.Length;

            int dosHeaderSize = Unsafe.SizeOf<ImageDosHeader>();
            int fileHeaderSize = Unsafe.SizeOf<ImageFileHeader>();
            if (len > dosHeaderSize)
            {
                //读取DosHeader
                ImageDosHeader dosHeader = MemoryMarshal.Read<ImageDosHeader>(data);

                int peOffset = (int)dosHeader.e_lfanew;
                if (dosHeader.IsVaild && len > peOffset + fileHeaderSize + 4)
                {
                    //读取PE标记
                    uint peSign = MemoryMarshal.Read<uint>(data[peOffset..]);
                    if(peSign == 0x00004550u)
                    {
                        //读取FileHeader
                        ImageFileHeader fileHeader = MemoryMarshal.Read<ImageFileHeader>(data[(peOffset + 4)..]);

                        this.mImageDosHeader = dosHeader;
                        this.mImageFileHeader = fileHeader;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 清除
        /// </summary>
        protected virtual void Clear()
        {
            this.mImageDosHeader = default;
            this.mImageFileHeader = default;
            this.mImageSectionHeaders.Clear();
        }

        /// <summary>
        /// RVA转FOA
        /// </summary>
        public uint RVAToFOA(uint rva)
        {
            List<ImageSectionHeader> sections = this.mImageSectionHeaders;
            if (!sections.Any())
            {
                return 0u;
            }

            //PE头部分
            ImageSectionHeader first = sections.First();
            if (rva < first.VirtualAddress && rva < first.PointerOfRawData)
            {
                return rva;
            }

            for(int i = 0; i < sections.Count; ++i)
            {
                ImageSectionHeader sec = sections[i];
                if(rva >= sec.VirtualAddress && rva < sec.VirtualAddress + sec.SizeOfRawData)
                {
                    return rva - sec.VirtualAddress + sec.PointerOfRawData;
                }
            }

            return 0u;
        }

        /// <summary>
        /// 尝试加载PE可选头
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据</param>
        /// <param name="result">PE可选头返回值</param>
        /// <returns></returns>
        protected unsafe bool TryLoadOptionHeader<T>(in ReadOnlySpan<byte> data, ref T result) where T : struct
        {
            ImageDosHeader dosHeader = this.mImageDosHeader;
            ImageFileHeader fileHeader = this.mImageFileHeader;

            //可选PE头偏移 大小
            int optionOffset = (int)dosHeader.e_lfanew + Unsafe.SizeOf<ImageFileHeader>() + 4;
            int optionSize = Math.Min(Unsafe.SizeOf<T>(), fileHeader.SizeOfOptionalHeader);

            //可选PE头指针
            ReadOnlySpan<byte> optionPtr = data.Slice(optionOffset, optionSize);

            Type type = typeof(T);
            if ((type == typeof(ImageOptionalHeader32) && fileHeader.Is32Bit) || (type == typeof(ImageOptionalHeader64) && fileHeader.Is64Bit))
            {
                optionPtr.CopyTo(MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref result, 1)));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 加载节表
        /// </summary>
        /// <param name="data">数据</param>
        protected unsafe void LoadSection(in ReadOnlySpan<byte> data)
        {
            ImageDosHeader dosHeader = this.mImageDosHeader;
            ImageFileHeader fileHeader = this.mImageFileHeader;

            //单个节大小 节偏移
            int sectionSize = Unsafe.SizeOf<ImageSectionHeader>();
            int sectionOffset = (int)dosHeader.e_lfanew + Unsafe.SizeOf<ImageFileHeader>() + 4 + fileHeader.SizeOfOptionalHeader;

            for(int i = 0; i < fileHeader.NumberOfSections; ++i)
            {
                ImageSectionHeader sectionHeader = MemoryMarshal.Read<ImageSectionHeader>(data[(sectionOffset + sectionSize * i)..]);
                this.mImageSectionHeaders.Add(sectionHeader);
            }
        }

        public override string ToString()
        {
            return "PE";
        }
    }

    /// <summary>
    /// PE文件 32位
    /// </summary>
    public class PEFile32 : PEFile
    {
        public ImageOptionalHeader32 mImageOptionalHeader;

        public override bool Load(in ReadOnlySpan<byte> data)
        {
            if(base.Load(data))
            {
                if (this.TryLoadOptionHeader(data, ref this.mImageOptionalHeader))
                {
                    this.LoadSection(data);
                    return true;
                }
            }
            return false;
        }

        protected override void Clear()
        {
            base.Clear();
            this.mImageOptionalHeader = default;
        }

        public override string ToString()
        {
            return "PE32";
        }
    }

    /// <summary>
    /// PE文件 64位
    /// </summary>
    public class PEFile64 : PEFile
    {
        public ImageOptionalHeader64 mImageOptionalHeader;

        public override bool Load(in ReadOnlySpan<byte> data)
        {
            if (base.Load(data))
            {
                if (this.TryLoadOptionHeader(data, ref this.mImageOptionalHeader))
                {
                    this.LoadSection(data);
                    return true;
                }
            }
            return false;
        }

        protected override void Clear()
        {
            base.Clear();
            this.mImageOptionalHeader = default;
        }

        public override string ToString()
        {
            return "PE64";
        }
    }
}
