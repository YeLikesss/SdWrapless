using System;
using System.IO;
using System.Text;

namespace SdWrapCore.Utils
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtend
    {
        /// <summary>
        /// 读取ASCII字符串(以\0结尾)
        /// </summary>
        /// <param name="bytes">数据</param>
        /// <param name="codePage">编码ID</param>
        /// <param name="limitLength">长度限制(含\0长度)(小于0 不限制)</param>
        public static string ReadASCIIString(this ReadOnlySpan<byte> bytes, int codePage, int limitLength)
        {
            int length = bytes.Length;
            if (limitLength >= 0)
            {
                length = Math.Min(length, limitLength);
            }

            //限制长度为0 空字符串
            if (length == 0 || bytes[0] == 0)
            {
                return string.Empty;
            }

            int index = 1;
            while(index < length && bytes[index] != 0)
            {
                ++index;
            }

            //结尾字节不是\0
            if(index == length && bytes[index - 1] != 0)
            {
                return string.Empty;
            }

            return Encoding.GetEncoding(codePage).GetString(bytes[..index]);
        }

        static StringExtend()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
