namespace FamilyBudgetTracker.FE.Constants;

public static class UserMangaConstants
{
    public static class ReadingStatus
    {
        public const string Reading = "Reading";
        public const string Finished = "Finished";
        public const string OnHold = "OnHold";
        public const string Dropped = "Dropped";
        public const string PlanToRead = "PlanToRead";

        public static readonly IReadOnlyCollection<string> ReadingStatuses = new[]
        {
            Reading, Finished, OnHold, Dropped, PlanToRead
        };


        private static readonly List<(string Key, string Label)> KeyLabelMapping =
        [
            (Reading, "Reading"),
            (Finished, "Finished"),
            (OnHold, "On hold"),
            (Dropped, "Dropped"),
            (PlanToRead, "Plan to read")
        ];

        public static string GetLabelByKey(string key)
        {
            return KeyLabelMapping.FirstOrDefault(x => x.Key == key).Label;
        }
    }


    public static class CollectingStatus
    {
        public const string Collected = "Collected";
        public const string InProgress = "InProgress";
        public const string PlanToCollect = "PlanToCollect";

        public static readonly IReadOnlyCollection<string> CollectingStatuses = new[]
        {
            Collected, InProgress, PlanToCollect,
        };

        private static readonly List<(string Key, string Label)> KeyLabelMapping =
        [
            (Collected, "Collected"),
            (InProgress, "In progress"),
            (PlanToCollect, "Plan to collect")
        ];

        public static string GetLabelByKey(string key)
        {
            return KeyLabelMapping.FirstOrDefault(x => x.Key == key).Label;
        }
    }
}