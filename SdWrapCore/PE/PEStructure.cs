using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SdWrapCore.PE
{
    /// <summary>
    /// Dos头结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 0x40)]
    public unsafe struct ImageDosHeader
    {
        /// <summary>
        /// MZ头标记
        /// </summary>
        public ushort e_magic;

        public ushort e_cblp;
        public ushort e_cp;
        public ushort e_crlc;
        public ushort e_cparhdr;
        public ushort e_minalloc;
        public ushort e_maxalloc;
        public ushort e_ss;
        public ushort e_sp;
        public ushort e_csum;
        public ushort e_ip;
        public ushort e_cs;
        public ushort e_lfarlc;
        public ushort e_ovno;
        public fixed ushort e_res[4];
        public ushort e_oemid;
        public ushort e_oeminfo;
        public fixed ushort e_res2[10];

        /// <summary>
        /// PE头标记偏移
        /// </summary>
        public uint e_lfanew;

        /// <summary>
        /// 获取MZ头合法性
        /// </summary>
        public readonly bool IsVaild => this.e_magic == 0x5A4D;
    }

    /// <summary>
    /// PE文件头结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 0x14)]
    public struct ImageFileHeader
    {
        /// <summary>
        /// 机器CPU类型
        /// </summary>
        public ImageMachineType Machine;
        /// <summary>
        /// 节的数量
        /// </summary>
        public ushort NumberOfSections;

        public uint TimeDataStamp;
        public uint PointerToSymbolTable;
        public uint NumberOfSymbols;

        /// <summary>
        /// PE可选头结构大小
        /// </summary>
        public ushort SizeOfOptionalHeader;
        /// <summary>
        /// 文件属性
        /// </summary>
        public ImageFileFlags Characteristics;

        /// <summary>
        /// 获取是否32位
        /// </summary>
        public readonly bool Is32Bit => this.Machine == ImageMachineType.IA32;
        /// <summary>
        /// 获取是否64位
        /// </summary>
        public readonly bool Is64Bit => this.Machine == ImageMachineType.AMD64;
    }

    /// <summary>
    /// 机器CPU类型枚举
    /// </summary>
    public enum ImageMachineType : ushort
    {
        AnyCPU = 0,
        IA32 = 0x014C,
        IA64 = 0x0200,
        AMD64 = 0x8664,
    }

    /// <summary>
    /// PE文件属性枚举
    /// </summary>
    [Flags]
    public enum ImageFileFlags : ushort
    {
        /// <summary>
        /// 移除重定位信息
        /// </summary>
        RelocationsStripped = 0x0001,
        /// <summary>
        /// 文件类型为exe文件
        /// </summary>
        ExecutableImage = 0x0002,

        LineNumbersStripped = 0x0004,
        LocalSymbolsStripped = 0x0008,
        AggresiveWSTrim = 0x0010,
        LargeAddressAware = 0x0020,
        BytesReversedLo = 0x0080,

        /// <summary>
        /// 32位机器类型
        /// </summary>
        Machine32bit = 0x0100,

        DebugStripped = 0x0200,
        RemovableRumFromSwap = 0x0400,
        NetRumFromSwap = 0x0800,

        /// <summary>
        /// 文件类型为sys系统文件
        /// </summary>
        FileSystem = 0x1000,
        /// <summary>
        /// 文件类型为dll文件
        /// </summary>
        FileDll = 0x2000,

        UpSystemOnly = 0x4000,
        BytesReversedHi = 0x8000
    }

    /// <summary>
    /// PE可选头结构(32位)
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct ImageOptionalHeader32
    {
        /// <summary>
        /// PE可选头标记
        /// </summary>
        [FieldOffset(0)]
        public ImageMagicType Magic;

        [FieldOffset(2)]
        public byte MajorLinkerVersion;
        [FieldOffset(3)]
        public byte MinorLinkerVersion;
        [FieldOffset(4)]
        public uint SizeOfCode;
        [FieldOffset(8)]
        public uint SizeOfInitializedData;
        [FieldOffset(12)]
        public uint SizeOfUninitializedData;

        /// <summary>
        /// 程序OEP入口点RVA
        /// </summary>
        [FieldOffset(16)]
        public uint AddressOfEntryPoint;

        [FieldOffset(20)]
        public uint BaseOfCode;
        [FieldOffset(24)]
        public uint BaseOfData;

        /// <summary>
        /// 内存首选基址
        /// </summary>
        [FieldOffset(28)]
        public uint ImageBase;

        /// <summary>
        /// 内存对齐大小
        /// </summary>
        [FieldOffset(32)]
        public uint SectionAlignment;

        /// <summary>
        /// 文件对齐大小
        /// </summary>
        [FieldOffset(36)]
        public uint FileAlignment;

        [FieldOffset(40)]
        public ushort MajorOperatingSystemVersion;
        [FieldOffset(42)]
        public ushort MinorOperatingSystemVersion;
        [FieldOffset(44)]
        public ushort MajorImageVersion;
        [FieldOffset(46)]
        public ushort MinorImageVersion;
        [FieldOffset(48)]
        public ushort MajorSubsystemVersion;
        [FieldOffset(50)]
        public ushort MinorSubsystemVersion;
        [FieldOffset(52)]
        public uint Win32VersionValue;

        /// <summary>
        /// 内存映像大小(内存对齐)
        /// </summary>
        [FieldOffset(56)]
        public uint SizeOfImage;

        /// <summary>
        /// PE头大小(文件对齐)
        /// </summary>
        [FieldOffset(60)]
        public uint SizeOfHeaders;

        [FieldOffset(64)]
        public uint CheckSum;

        [FieldOffset(68)]
        public ImageSubSystemType SubSystem;
        [FieldOffset(70)]
        public ImageDllCharacteristicsFlags DllCharacteristics;

        [FieldOffset(72)]
        public uint SizeOfStackReserve;
        [FieldOffset(76)]
        public uint SizeOfStackCommit;
        [FieldOffset(80)]
        public uint SizeOfHeapReserve;
        [FieldOffset(84)]
        public uint SizeOfHeapCommit;
        [FieldOffset(88)]
        public uint LoadFlags;

        /// <summary>
        /// 数据目录项数
        /// </summary>
        [FieldOffset(92)]
        public uint NumberOfRvaAndSizes;

        [FieldOffset(96)]
        public ImageDataDirectory ExportTable;
        [FieldOffset(104)]
        public ImageDataDirectory ImportTable;
        [FieldOffset(112)]
        public ImageDataDirectory ResourceTable;
        [FieldOffset(120)]
        public ImageDataDirectory ExceptionTable;
        [FieldOffset(128)]
        public ImageDataDirectory CertificateTable;
        [FieldOffset(136)]
        public ImageDataDirectory BaseRelocationTable;
        [FieldOffset(144)]
        public ImageDataDirectory Debug;
        [FieldOffset(152)]
        public ImageDataDirectory Architecture;
        [FieldOffset(160)]
        public ImageDataDirectory GlobalPtr;
        [FieldOffset(168)]
        public ImageDataDirectory ThreadLocalStorageTable;
        [FieldOffset(176)]
        public ImageDataDirectory LoadConfigTable;
        [FieldOffset(184)]
        public ImageDataDirectory BoundImport;
        [FieldOffset(192)]
        public ImageDataDirectory ImportAddressTable;
        [FieldOffset(200)]
        public ImageDataDirectory DelayImportDescriptor;
        [FieldOffset(208)]
        public ImageDataDirectory CLRRuntimeHeader;
        [FieldOffset(216)]
        public ImageDataDirectory Reserved;
    }

    /// <summary>
    /// PE可选头结构(64位)
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct ImageOptionalHeader64
    {
        /// <summary>
        /// PE可选头标记
        /// </summary>
        [FieldOffset(0)]
        public ImageMagicType Magic;

        [FieldOffset(2)]
        public byte MajorLinkerVersion;
        [FieldOffset(3)]
        public byte MinorLinkerVersion;
        [FieldOffset(4)]
        public uint SizeOfCode;
        [FieldOffset(8)]
        public uint SizeOfInitializedData;
        [FieldOffset(12)]
        public uint SizeOfUninitializedData;

        /// <summary>
        /// 程序OEP入口点RVA
        /// </summary>
        [FieldOffset(16)]
        public uint AddressOfEntryPoint;

        [FieldOffset(20)]
        public uint BaseOfCode;

        /// <summary>
        /// 内存首选基址
        /// </summary>
        [FieldOffset(24)]
        public ulong ImageBase;

        /// <summary>
        /// 内存对齐大小
        /// </summary>
        [FieldOffset(32)]
        public uint SectionAlignment;

        /// <summary>
        /// 文件对齐大小
        /// </summary>
        [FieldOffset(36)]
        public uint FileAlignment;

        [FieldOffset(40)]
        public ushort MajorOperatingSystemVersion;
        [FieldOffset(42)]
        public ushort MinorOperatingSystemVersion;
        [FieldOffset(44)]
        public ushort MajorImageVersion;
        [FieldOffset(46)]
        public ushort MinorImageVersion;
        [FieldOffset(48)]
        public ushort MajorSubsystemVersion;
        [FieldOffset(50)]
        public ushort MinorSubsystemVersion;
        [FieldOffset(52)]
        public uint Win32VersionValue;

        /// <summary>
        /// 装内存映像大小(内存对齐)
        /// </summary>
        [FieldOffset(56)]
        public uint SizeOfImage;

        /// <summary>
        /// PE头大小(文件对齐)
        /// </summary>
        [FieldOffset(60)]
        public uint SizeOfHeaders;

        [FieldOffset(64)]
        public uint CheckSum;

        [FieldOffset(68)]
        public ImageSubSystemType SubSystem;
        [FieldOffset(70)]
        public ImageDllCharacteristicsFlags DllCharacteristics;

        [FieldOffset(72)]
        public ulong SizeOfStackReserve;
        [FieldOffset(80)]
        public ulong SizeOfStackCommit;
        [FieldOffset(88)]
        public ulong SizeOfHeapReserve;
        [FieldOffset(96)]
        public ulong SizeOfHeapCommit;
        [FieldOffset(104)]
        public uint LoadFlags;

        /// <summary>
        /// 数据目录项数
        /// </summary>
        [FieldOffset(108)]
        public uint NumberOfRvaAndSizes;

        [FieldOffset(112)]
        public ImageDataDirectory ExportTable;
        [FieldOffset(120)]
        public ImageDataDirectory ImportTable;
        [FieldOffset(128)]
        public ImageDataDirectory ResourceTable;
        [FieldOffset(136)]
        public ImageDataDirectory ExceptionTable;
        [FieldOffset(144)]
        public ImageDataDirectory CertificateTable;
        [FieldOffset(152)]
        public ImageDataDirectory BaseRelocationTable;
        [FieldOffset(160)]
        public ImageDataDirectory Debug;
        [FieldOffset(168)]
        public ImageDataDirectory Architecture;
        [FieldOffset(176)]
        public ImageDataDirectory GlobalPtr;
        [FieldOffset(184)]
        public ImageDataDirectory ThreadLocalStorageTable;
        [FieldOffset(192)]
        public ImageDataDirectory LoadConfigTable;
        [FieldOffset(200)]
        public ImageDataDirectory BoundImport;
        [FieldOffset(208)]
        public ImageDataDirectory ImportAddressTable;
        [FieldOffset(216)]
        public ImageDataDirectory DelayImportDescriptor;
        [FieldOffset(224)]
        public ImageDataDirectory CLRRuntimeHeader;
        [FieldOffset(232)]
        public ImageDataDirectory Reserved;
    }

    /// <summary>
    /// PE可选头中数据表结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 0x08)]
    public struct ImageDataDirectory
    {
        /// <summary>
        /// 数据表的RVA
        /// </summary>
        public uint VirtualAddress;
        /// <summary>
        /// 数据块大小
        /// </summary>
        public uint Size;
    }

    /// <summary>
    /// PE可选头标记枚举
    /// </summary>
    public enum ImageMagicType : ushort
    {
        /// <summary>
        /// 32位可选PE头标记
        /// </summary>
        ImageOptionalHeader32Magic = 0x010B,
        /// <summary>
        /// 64位可选PE头标记
        /// </summary>
        ImageOptionalHeader64Magic = 0x020B,
    }

    /// <summary>
    /// 子系统类型枚举值
    /// </summary>
    public enum ImageSubSystemType : ushort
    {
        ImageSubsystemUnknown = 0,
        ImageSubsystemNative = 1,
        ImageSubsystemWindowsGui = 2,    //图形接口子系统
        ImageSubsystemWindowsCui = 3,
        ImageSubsystemOS2Cui = 5,
        ImageSubsystemPosixCui = 7,
        ImageSubsystemNativeWindows = 8,
        ImageSubsystemWindowsCeGui = 9,
        ImageSubsystemEfiApplication = 10,
        ImageSubsystemEfiBootServiceDriver = 11,
        ImageSubsystemEfiRuntimeDriver = 12,
        ImageSubsystemEfiRom = 13,
        ImageSubsystemXbox = 14,
        ImageSubsystemWindowsBootApplication = 16
    }

    /// <summary>
    /// Dll的文件属性枚举
    /// </summary>
    [Flags]
    public enum ImageDllCharacteristicsFlags : ushort
    {
        Reserved0 = 0x0001,
        Reserved1 = 0x0002,
        Reserved2 = 0x0004,
        Reserved3 = 0x0008,
        Unknow1 = 0x0010,
        Unknow2 = 0x0020,
        /// <summary>
        /// 动态基址标识
        /// </summary>
        ImageDllCharacteristicsDynamicBase = 0x0040,

        ImageDllCharacteristicsForceIntegrity = 0x0080,
        ImageDllCharacteristicsNxCompat = 0x0100,
        ImageDllcharacteristicsNoIsolation = 0x0200,
        ImageDllcharacteristicsNoSeh = 0x0400,
        ImageDllcharacteristicsNoBind = 0x0800,
        Reserved4 = 0x1000,
        ImageDllcharacteristicsWdmDriver = 0x2000,
        ImageDllcharacteristicsTerminalServerAware = 0x8000
    }

    /// <summary>
    /// 节表数据结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 0x28)]
    public unsafe struct ImageSectionHeader
    {
        /// <summary>
        /// 节的名字
        /// </summary>
        public fixed byte Name[8];
        /// <summary>
        /// 节内存大小
        /// </summary>
        public uint VirtualSize;
        /// <summary>
        /// 节数据起始位置内存映像RVA
        /// </summary>
        public uint VirtualAddress;
        /// <summary>
        /// 节数据文件对齐大小
        /// </summary>
        public uint SizeOfRawData;
        /// <summary>
        /// 节数据文件偏移
        /// </summary>
        public uint PointerOfRawData;

        public uint PointerOfRelocations;
        public uint PointerOfLinenumbers;
        public ushort NumberOfRelocations;
        public ushort NumberOfLinenumbers;

        /// <summary>
        /// 节属性
        /// </summary>
        public ImageSectionCharacteristicsFlags Characteristics;

        /// <summary>
        /// 获得节名称
        /// </summary>
        public readonly string SectionName
        {
            get
            {
                fixed(byte* ptr = this.Name)
                {
                    return Encoding.ASCII.GetString(ptr, 8).Trim('\0');
                }
            }
        }
    }

    /// <summary>
    /// 节属性枚举
    /// </summary>
    [Flags]
    public enum ImageSectionCharacteristicsFlags : uint
    {
        ContentCode = 0x00000020,
        ContentInitializedData = 0x00000040,
        ContentUninitializedData = 0x00000080,
        LinkExtendedRelocationOverflow = 0x01000000,
        MemoryDiscardable = 0x02000000,
        MemoryNotCached = 0x04000000,
        MemoryNotPaged = 0x08000000,
        MemoryShared = 0x10000000,

        /// <summary>
        /// 此节可执行
        /// </summary>
        MemoryExecute = 0x20000000,
        /// <summary>
        /// 此节可读
        /// </summary>
        MemoryRead = 0x40000000,
        /// <summary>
        /// 此节可写
        /// </summary>
        MemoryWrite = 0x80000000
    }
}
