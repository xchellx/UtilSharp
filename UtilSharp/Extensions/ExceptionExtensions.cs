using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace UtilSharp.Extensions
{
    public static class ExceptionExtensions
    {
        public static Exception? PopulateException(this Exception? @this)
        {
            if (@this == null)
                return @this; // Nothing to populate

            try
            {
                ExceptionDispatchInfo.Throw(@this);
                throw @this;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        [DoesNotReturn]
        public static void Rethrow(this Exception? e) => ExceptionDispatchInfo.Throw(e!);
    }
}
