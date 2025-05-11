namespace FamilyBudgetTracker.FE.Constants;

public class OrderConstants
{
    public static class Status
    {
        public const string Created = "Created";

        public const string OnTheWay = "OnTheWay";

        public const string Delivered = "Delivered";

        public static readonly IReadOnlyList<string> OrderStatuses = new[]
        {
            Created,
            OnTheWay,
            Delivered
        };
        
        private static readonly List<(string Key, string Label)> KeyLabelMapping =
        [
            (Created, "Created"),
            (OnTheWay, "On the way"),
            (Delivered, "Delivered"),
        ];

        public static string GetLabelByKey(string key)
        {
            return KeyLabelMapping.FirstOrDefault(x => x.Key == key).Label;
        }
    }
}