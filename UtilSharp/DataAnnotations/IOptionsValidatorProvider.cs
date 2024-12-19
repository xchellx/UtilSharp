namespace UtilSharp.DataAnnotations
{
    public interface IOptionsValidatorProvider
    {
        public IOptionsValidator Validator { get; }
    }
}
