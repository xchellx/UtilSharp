using System.ComponentModel.DataAnnotations;

namespace UtilSharp.Extensions
{
    public static class AttributeExtensions
    {
        public static bool IsCustomErrorMessageSet(this ValidationAttribute @this) => @this != null
            && (@this.ErrorMessage != null || @this.ErrorMessageResourceName != null
            || @this.ErrorMessageResourceType != null);

        public static bool IsCustomErrorMessageSet<T>(this T @this) where T : ValidationAttribute => @this != null
            && (@this.ErrorMessage != null || @this.ErrorMessageResourceName != null
            || @this.ErrorMessageResourceType != null);
    }
}
