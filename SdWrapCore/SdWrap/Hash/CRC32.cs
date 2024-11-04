using System;

namespace SdWrapCore.SdWrap.Hash
{
    /// <summary>
    /// SdWrap CRC32
    /// </summary>
    internal class CRC32
    {
        private readonly static uint[] smCRCTable = new uint[256];   //CRC32表

        static CRC32()
        {
            int count = 0;
            for(int i = 0; i < 256; ++i)
            {
                int v = count << 24;
                for (int j = 0; j < 8; ++j)
                {
                    if (v >= 0)
                    {
                        v <<= 1;
                    }
                    else
                    {
                        v <<= 1;
                        v ^= 0x4C11DB7;
                    }
                }
                CRC32.smCRCTable[i] = (uint)v;

                ++count;
            }
        }

        /// <summary>
        /// 计算CRC32值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>Hash值</returns>
        public static uint Hash(in ReadOnlySpan<byte> data)
        {
            uint crc = 0xFFFFFFFFu;
            for(int i = 0; i < data.Length; ++i)
            {
                crc = CRC32.smCRCTable[data[i] ^ (crc >> 24)] ^ (crc << 8);
            }
            return ~crc;
        }
    }
}
