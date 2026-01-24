using CashFlow.Application.Services.Mappings;

namespace CommonTestUtilities.Mapster;

public class MapsterBuilder
{
    public static void Build()
    {
        MapConfigurations.Configure();
    }
}
