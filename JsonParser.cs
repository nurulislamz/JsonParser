namespace JsonParser
{
    public class JsonParser
    {
        private readonly string fileContents;
        private Stack<char> bracketTrack = new Stack<char>();
        public JsonParser(string fileContents)
        {
            this.fileContents = fileContents;
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