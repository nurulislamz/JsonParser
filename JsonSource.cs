using System.Globalization;

namespace JsonParser
{
    public interface IJsonSource
    {
        string GetJson();
    }

    public class FileJsonSource : IJsonSource
    {
        private readonly string filePath;
        private string fileContent;

        public FileJsonSource(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            this.filePath = filePath;
            this.fileContent = fileContent;
        }

        public string GetJson()
        {
            return File.ReadAllText(filePath);
        } 
    }

    public class StringJsonSource : IJsonSource
    {

        private readonly string jsonString;

        public StringJsonSource(string jsonString)
        {
            this.jsonString = jsonString;
        }

        public string GetJson()
        {
            return jsonString;
        } 
    }
}