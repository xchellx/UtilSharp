using Microsoft.Extensions.Options;

namespace UtilSharp.DataAnnotations
{
    public interface IOptionsValidator<TOptions> : IOptionsValidator, IValidateOptions<TOptions> where TOptions : class
    {
        ValidateOptionsResult IOptionsValidator.Validate(string? name, object options)
            => Validate(name, (TOptions) options);
    }
}
