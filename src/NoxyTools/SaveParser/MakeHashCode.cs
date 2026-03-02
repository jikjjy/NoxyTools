namespace Gen.Utils
{
    /// <summary>
    /// 해시 코드 생성에 도움을 주는 핼퍼 클래스
    /// </summary>
    internal struct MakeHashCode
    {
        private readonly int value;

        /// <summary>
        /// 해시 코드 반환
        /// </summary>
        /// <param name="hashCode"></param>
        public static implicit operator int(MakeHashCode hashCode)
        {
            return hashCode.value;
        }

        /// <summary>
        /// 해시 코드 시작
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static MakeHashCode Of<T>(T item)
        {
            return new MakeHashCode(GetHashCode(item));
        }

        /// <summary>
        /// 해시 코드 단일 프로퍼티 추가
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public MakeHashCode And<T>(T item)
        {
            return new MakeHashCode(CombineHashCodes(value, GetHashCode(item)));
        }

        /// <summary>
        /// 해시 코드 컬랙션 프로퍼티 추가
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public MakeHashCode AndEach<T>(IEnumerable<T> items)
        {
            if (items == null)
            {
                return this;
            }

            var hashCode = items.Any() ? items.Select(GetHashCode).Aggregate(CombineHashCodes) : 0;
            return new MakeHashCode(CombineHashCodes(value, hashCode));
        }

        private MakeHashCode(int value)
        {
            this.value = value;
        }

        private static int CombineHashCodes(int h1, int h2)
        {
            unchecked
            {
                // Code copied from System.Tuple so it must be the best way to combine hash codes or at least a good one.
                return ((h1 << 5) + h1) ^ h2;
            }
        }

        private static int GetHashCode<T>(T item)
        {
            return item == null ? 0 : item.GetHashCode();
        }
    }
}
