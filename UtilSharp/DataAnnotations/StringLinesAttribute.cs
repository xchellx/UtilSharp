using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using UtilSharp.Extensions;

namespace BNRSharp.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class StringLinesAttribute(int maximumLength) : ValidationAttribute(() => ">The {0} field is required.")
    {
        public int MaximumLines { get; } = maximumLength;

        public int MinimumLines { get; set; }

        public override bool IsValid(object? value)
        {
            EnsureLegalLengths();

            if (value == null)
                return true;

            using StringReader reader = new StringReader((string) value);
            int lines = 0;
            while (reader.ReadLine() != null)
                lines++;

            return lines >= MinimumLines && lines <= MaximumLines;
        }

        public override string FormatErrorMessage(string name)
        {
            EnsureLegalLengths();

            bool useErrorMessageWithMinimum = MinimumLines != 0 && !this.IsCustomErrorMessageSet();
            string errorMessage = useErrorMessageWithMinimum
                ? "The field {0} must be a string with a minimum amount of lines of {2} and a maximum amount of lines of {1}."
                : ErrorMessageString;

            return string.Format(CultureInfo.CurrentCulture, errorMessage, name, MaximumLines, MinimumLines);
        }

        private void EnsureLegalLengths()
        {
            if (MaximumLines < 0)
                throw new InvalidOperationException("The maximum amount of lines must be a nonnegative integer.");

            if (MaximumLines < MinimumLines)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "The maximum value '{0}' must be greater than or equal to the minimum value '{1}'.", MaximumLines,
                    MinimumLines));
        }
    }
}
