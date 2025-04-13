namespace FamilyBudgetTracker.Backend.Data;

public static class DbContextConstants
{
    public const string CheckConstraintCategoryName = "CK_Category_Owner";

    public const string CheckConstraintCategory = """(("UserId" IS NOT NULL AND "FamilyId" IS NULL) OR """ +
                                                  """("UserId" IS NULL AND "FamilyId" IS NOT NULL))""";

    public const string CheckConstraintTransactionName = "CK_Category_Transaction";

    public const string CheckConstraintTransaction =
        """(("UserId" IS NOT NULL AND "FamilyId" IS NULL) OR """ +
        """("UserId" IS NULL AND "FamilyId" IS NOT NULL) OR """ +
        """("UserId" IS NOT NULL AND "FamilyId" IS NOT NULL))""";
}