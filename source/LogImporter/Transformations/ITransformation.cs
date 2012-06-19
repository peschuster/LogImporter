namespace LogImporter.Transformations
{
    public interface ITransformation
    {
        void Apply(LogEntry entry);
    }
}
