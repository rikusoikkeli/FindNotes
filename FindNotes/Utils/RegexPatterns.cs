namespace FindNotes.Utils
{
    public static class RegexPatterns
    {
        public static readonly string PathParameterName = @"[-](path|p)";
        public static readonly string PathParameterValueWithQuotes = @"""(\w{1}[:]).+""";
        public static readonly string PathParameterValueWithoutQuotes = @"(\w{1}[:])(.|\d)+.+(?=-q|-query)|(\w{1}[:])(.|\d)+.+(?=\n)";
        public static readonly string QueryParameterValueAndNameWithQuotes = @"[-](query|q)\s""[\w\s\d-]+""";
        public static readonly string QueryParameterValueAndNameWithoutQuotes = @"[-](query|q){1}\s+\w+";
        public static readonly string FilenameExtension = @"[{1}.][\w]*";
        public static readonly string SavePathParameterName = @"[-](save|s)";
        public static readonly string NicknameParameterName = @"[-](nickname|n)";
        public static readonly string SavePathWithOrWithoutQuotes = @"(?<=-s|-save)\s[\""]?(\w{1}[:]).+[\""]?\s(?=-nickname|-n)";
        public static readonly string NicknameWithOrWithoutQuotes = @"(?<=nickname|n)\s[\""]?.+[\""]?\s";
        public static readonly string ListParameterName = @"[-](list|l)";
        public static readonly string ListSavedPaths = @"(?<=list|l)\s(savedPaths)\s";
        public static readonly string PathNicknameValueWithOrWithoutQuotes = @"(?<=path|p)\s[\""]?[\w\d]+[\""]?\s";
    }
}
