using CashFlow.Communication.Requests;
using Mapster;

namespace CashFlow.Application.Services.Mappings;

public static class MapConfigurations
{
    public static void Configure()
    {
        TypeAdapterConfig<RequestRegisterUserJson, Domain.Entities.User>
            .NewConfig()
            .Ignore(dest => dest.Password);
    }
}
