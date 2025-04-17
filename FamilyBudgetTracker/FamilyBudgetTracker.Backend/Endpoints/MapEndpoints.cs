using FamilyBudgetTracker.Backend.Endpoints.Personal;

namespace FamilyBudgetTracker.Backend.Endpoints;

public static class MapEndpoints
{
    public static void MapPersonalEndpoints(this WebApplication app)
    {
        var personalGroup = app.MapGroup("personal");
        
        personalGroup.MapCategoryEndpoints();
    }
}