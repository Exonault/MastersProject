namespace FamilyBudgetTracker.FE.Constants;

public static class AuthorConstants
{
    public static class AuthorRole
    {
        public const string Story = "Story";
        public const string Art = "Art";
        public const string StoryAndArt = "StoryAndArt";

        public static readonly IReadOnlyList<string> AuthorRoles = new[]
        {
            Story, Art, StoryAndArt
        };

        private static Dictionary<string, string> tempDictionary = new Dictionary<string, string>()
        {
            { Story, "Story" },
            { Art, "Art" },
            { StoryAndArt, "Story and Art" },
        };

        public static string GetLabelByKey(string key)
        {
            return tempDictionary.FirstOrDefault(x => x.Key == key).Value;
        }
    }
}