using Phonix;

namespace Europa.Commons.Phonetic
{
    public static class PhonixWrapper
    {
        private static readonly CaverPhone encoder = new CaverPhone();

        public static string BuildKey(string word)
        {
            if (word == null)
            {
                return word;
            }
            if (word.Length < 3)
            {
                return word;
            }
            return encoder.BuildKey(word);
        }

        public static string BuildKeyWithoutNumbers(string word)
        {
            if (word == null)
            {
                return word;
            }
            if (word.Length < 3)
            {
                return word;
            }
            return encoder.BuildKey(word).Replace("1", "");
        }
    }
}
