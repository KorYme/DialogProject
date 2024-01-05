namespace KorYmeLibrary.Utilities
{
    public static class SerializationTextUtility
    {
        const string ALLOWED_CHARACTERS = "_";

        public static bool IsWhitespace(this char character)
        {
            switch (character)
            {
                case '\u0020':
                case '\u00A0':
                case '\u1680':
                case '\u2000':
                case '\u2001':
                case '\u2002':
                case '\u2003':
                case '\u2004':
                case '\u2005':
                case '\u2006':
                case '\u2007':
                case '\u2008':
                case '\u2009':
                case '\u200A':
                case '\u202F':
                case '\u205F':
                case '\u3000':
                case '\u2028':
                case '\u2029':
                case '\u0009':
                case '\u000A':
                case '\u000B':
                case '\u000C':
                case '\u000D':
                case '\u0085':
                        return true;
                default:
                        return false;
            }
        }

        public static string RemoveWhitespaces(this string text)
        {
            string newString = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].IsWhitespace()) continue;
                newString += text[i];
            }
            return newString;
        }

        public static string RemoveNonSerializableCharacters(this string text)
        {
            string newString = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (!text[i].IsSerializableFriendly()) continue;
                newString += text[i];
            }
            return newString;
        }

        public static bool IsSerializableFriendly(this string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!text[i].IsSerializableFriendly()) return false;
            }
            return true;
        }

        public static bool IsSerializableFriendly(this char character) 
            => char.IsLetterOrDigit(character) || ALLOWED_CHARACTERS.Contains(character);
    }
}