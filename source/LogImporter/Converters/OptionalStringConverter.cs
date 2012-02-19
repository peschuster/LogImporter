namespace LogImporter.Converters
{
    public class OptionalStringConverter : IConverter<string>
    {
        public string Transform(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "-")
                return null;

            return value;
        }
    }
}
