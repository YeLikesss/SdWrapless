using System;

namespace SdWrapCore.SdWrap.Crypto
{
    /// <summary>
    /// 解密器V1
    /// </summary>
    internal class SdWrapDecryptorV1
    {
        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="bytes">数据</param>
        public static void Decrypt(in Span<byte> bytes, uint key)
        {
            RandomV1 random = new(key);
            for (int i = 0; i < bytes.Length; ++i)
            {
                bytes[i] ^= (byte)random.MoveNext();
            }
        }
    }
}
