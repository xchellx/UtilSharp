namespace UtilSharp.DataAnnotations
{
    public interface IOptionsValidatorProvider<TOptions> : IOptionsValidatorProvider where TOptions : class
    {
        public new IOptionsValidator<TOptions> Validator { get; }
    }
}
