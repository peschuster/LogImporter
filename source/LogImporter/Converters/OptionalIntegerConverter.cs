namespace LogImporter.Converters
{
    public class OptionalIntegerConverter : IConverter<int?>
    {
        private readonly IConverter<string> stringConverter;

        public OptionalIntegerConverter(IConverter<string> stringConverter)
        {
            this.stringConverter = stringConverter;
        }

        public int? Transform(string value)
        {
            string value2 = this.stringConverter.Transform(value);
            
            int result;

            if (value2 != null && int.TryParse(value2, out result))
                return result;

            return null;
        }
    }
}
