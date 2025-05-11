namespace FamilyBudgetTracker.FE.Constants;

public static class LibraryMangaConstants
{
    public const string ImagePlaceHolder = "https://placehold.co/250x350?text=Placeholder"; //Placeholder image

    public static class DemographicType
    {
        public const string Shounen = "Shounen";
        public const string Seinen = "Seinen";
        public const string Shoujo = "Shoujo";
        public const string Josei = "Josei";

        public static readonly IReadOnlyList<string> DemographicTypes = new[]
        {
            Shounen,
            Seinen,
            Shoujo,
            Josei,
        };

        private static readonly List<(string Key, string Label)> KeyLabelMapping =
        [
            (Shounen, "Shounen"),
            (Seinen, "Seinen"),
            (Shoujo, "Shoujo"),
            (Josei, "Josei")
        ];

        public static string GetLabelByKey(string key)
        {
            return KeyLabelMapping.FirstOrDefault(x => x.Key == key).Label;
        }
    }

    public static class Type
    {
        public const string Manga = "Manga";
        public const string LightNovel = "LightNovel";
        public const string OneShot = "OneShot";

        public static readonly IReadOnlyList<string> ComicTypes = new List<string>
        {
            Manga,
            LightNovel,
            OneShot
        };

        private static readonly List<(string Key, string Label)> KeyLabelMapping =
        [
            (Manga, "Manga"),
            (LightNovel, "Light novel"),
            (OneShot, "One shot")
        ];

        public static string GetLabelByKey(string key)
        {
            return KeyLabelMapping.FirstOrDefault(x => x.Key == key).Label;
        }
    }

    public static class PublishingType
    {
        public const string Publishing = "Publishing";
        public const string Finished = "Finished";
        public const string OnHiatus = "OnHiatus";

        public static readonly IReadOnlyList<string> PublishingStatuses = new List<string>
        {
            Publishing,
            Finished,
            OnHiatus
        };

        private static readonly List<(string Key, string Label)> KeyLabelMapping =
        [
            (Publishing, "Publishing"),
            (Finished, "Finished"),
            (OnHiatus, "On Hiatus")
        ];
        
        public static string GetLabelByKey(string key)
        {
            return KeyLabelMapping.FirstOrDefault(x => x.Key == key).Label;
        }
    }
}