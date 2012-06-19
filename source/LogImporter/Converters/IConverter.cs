namespace LogImporter.Converters
{
    public interface IConverter<T>
    {
        T Transform(string value);
    }
}
