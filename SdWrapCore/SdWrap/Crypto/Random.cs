
namespace SdWrapCore.SdWrap.Crypto
{
    /// <summary>
    /// 伪随机数V1
    /// </summary>
    internal class RandomV1
    {
        private int mPosition = 0;
        private readonly uint[] mData = new uint[521];

        /// <summary>
        /// 获取下一个随机值
        /// </summary>
        public uint MoveNext()
        {
            int pos = this.mPosition;

            ++pos;
            if (pos >= 521)
            {
                this.Transform();
                pos = 0;
            }
            this.mPosition = pos;

            return this.mData[pos];
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="seed">种子</param>
        private void Initialize(uint seed)
        {
            uint[] data = this.mData;

            for(int i = 0; i < 17; ++i)
            {
                uint v = 0;
                for(int j = 0; j < 32; ++j)
                {
                    seed = seed * 0x5D588B65u + 1;

                    v >>= 1;
                    v |= seed & 0x80000000u;
                }
                data[i] = v;
            }

            data[16] = data[15] ^ (data[0] >> 9) ^ (data[16] << 23);

            for(int i = 0; i < 504; ++i)
            {
                uint v0 = data[i + 0];
                uint v1 = data[i + 1];
                uint v2 = data[i + 16];

                data[i + 17] = v2 ^ (v1 >> 9) ^ (v0 << 23);
            }

            this.Transform();
            this.Transform();
            this.Transform();

            this.mPosition = 520;
        }

        /// <summary>
        /// 变换表
        /// </summary>
        private void Transform()
        {
            uint[] data = this.mData;

            for(int i = 0; i < 32; ++i)
            {
                data[i + 0] ^= data[i + 489];
            }

            for(int i = 0; i < 489; ++i)
            {
                data[i + 32] ^= data[i + 0];
            }
        }

        public RandomV1(uint seed = 0u)
        {
            this.Initialize(seed);
        }
    }
}
