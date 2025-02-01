namespace JsonParser
{
    public class JsonParser
    {
        private string fileDir;
        private string fileContents;
        private Stack<char> bracketTrack = new Stack<char>();
        public JsonParser(string fileDir)
        {
            this.fileDir = fileDir;
            this.fileContents = File.ReadAllText(fileDir);
        }

        public bool ValidateJson()
        {
            for (int i = 0; i < this.fileContents.Length; i++)
            {
                if (this.fileContents[i] == '{')
                {
                    bracketTrack.Push(this.fileContents[i]);
                }
                else if (this.fileContents[i] == '}' && bracketTrack.TryPeek(out var topOfStack) && topOfStack == '{')
                {
                    bracketTrack.Pop();
                }
            }

            if (bracketTrack.Count > 0) { return false; }
            else { return true; };
        }

/*        public bool ValidateKeyValuePairs() { }
*/    }
}