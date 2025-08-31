namespace MicrosoftGraphServiceClient
{
    // https://stakhov.pro/union-types-in-csharp/

    public struct Union<T1, T2>
    {
        private readonly T1 _value1;
        private readonly T2 _value2;
        private readonly byte _type; // 1 = T1, 2 = T2

        public Union(T1 value)
        {
            _value1 = value;
            _value2 = default!;
            _type = 1;
        }

        public Union(T2 value)
        {
            _value1 = default!;
            _value2 = value;
            _type = 2;
        }

        public bool Is<T>() =>
            _type == 1 && typeof(T) == typeof(T1) ||
            _type == 2 && typeof(T) == typeof(T2);

        public override string? ToString() =>
            _type == 1 ? _value1?.ToString() :
            _type == 2 ? _value2?.ToString() :
            null;

        public static implicit operator Union<T1, T2>(T1 value) => new(value);
        public static implicit operator Union<T1, T2>(T2 value) => new(value);

        public static implicit operator T1(Union<T1, T2> union) =>
            union._type == 1 ? union._value1 : throw new InvalidOperationException($"Union does not contain a value of type {typeof(T1)}");

        public static implicit operator T2(Union<T1, T2> union) =>
            union._type == 2 ? union._value2 : throw new InvalidOperationException($"Union does not contain a value of type {typeof(T2)}");

        public T Match<T>(Func<T1, T> matchT1, Func<T2, T> matchT2)
        {
            if (_type == 1)
                return matchT1(_value1);
            else if (_type == 2)
                return matchT2(_value2);

            throw new InvalidOperationException("Union is in an invalid state");
        }

        public Union<TResult, T2> MapFirst<TResult>(Func<T1, TResult> mapper)
        {
            return _type == 1
                ? new Union<TResult, T2>(mapper(_value1))
                : new Union<TResult, T2>(_value2);
        }

        public Union<T1, TResult> MapSecond<TResult>(Func<T2, TResult> mapper)
        {
            return _type == 2
                ? new Union<T1, TResult>(mapper(_value2))
                : new Union<T1, TResult>(_value1);
        }
    }
}
