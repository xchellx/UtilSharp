using System;

namespace UtilSharp.Extensions
{
    public static class EnumExtensions
    {
        public static bool IsFlagsDefined(this Enum @this) => !ulong.TryParse(@this.ToString(), out _);

        public static bool IsFlagsDefined<T>(this T @this) where T : struct, Enum
            => !ulong.TryParse(@this.ToString(), out _);
    }
}
