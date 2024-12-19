using Microsoft.Extensions.Options;

namespace UtilSharp.DataAnnotations
{
    public interface IOptionsValidator
    {
        public ValidateOptionsResult Validate(string? name, object options);
    }
}
