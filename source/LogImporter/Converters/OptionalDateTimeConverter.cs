using System;

namespace LogImporter.Converters
{
    public class OptionalDateTimeConverter : IConverter<DateTime?>
    {
        private readonly IConverter<string> stringConverter;

        public OptionalDateTimeConverter(IConverter<string> stringConverter)
        {
            this.stringConverter = stringConverter;
        }

        public DateTime? Transform(string value)
        {
            string value2 = this.stringConverter.Transform(value);

            DateTime result;

            if (value2 != null && DateTime.TryParse(value2, out result))
                return result;

            return null;            
        }
    }
}
