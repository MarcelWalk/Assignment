using AntonPaar.Poco;

namespace AntonPaar.Model
{
    public class KeyValueMapper
    {
        public static WordCountEntry MapToWordCountEntry(string key, int value)
        {
            return new WordCountEntry
            {
                Word = key,
                Count = value
            };
        }
    }
}