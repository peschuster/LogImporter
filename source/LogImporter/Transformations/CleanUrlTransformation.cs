using System;
using System.Text.RegularExpressions;

namespace LogImporter.Transformations
{
    public class CleanUrlTransformation : ITransformation
    {
        public void Apply(LogEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            if (!string.IsNullOrEmpty(entry.csUriStem))
            {
                entry.CleanUri = this.RemoveTrailingSlash(
                    this.ReplaceGuids(entry.csUriStem));
            }
        }

        private string ReplaceGuids(string input)
        {
            string pattern = @"(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})";

            return Regex.Replace(input, pattern, string.Empty)
                .Replace(@"\\", @"\")
                .Replace(@"//", @"/");
        }

        private string RemoveTrailingSlash(string input)
        {
            if (input.Length > 1 && (input.EndsWith("\\") || input.EndsWith("/")))
                return input.Substring(0, input.Length - 1);

            return input;
        }
    }
}
