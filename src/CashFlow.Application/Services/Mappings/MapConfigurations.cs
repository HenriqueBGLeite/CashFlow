using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using Mapster;

namespace CashFlow.Application.Services.Mappings;

public static class MapConfigurations
{
    public static void Configure()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private static void RequestToEntity()
    {
        TypeAdapterConfig<RequestRegisterUserJson, User>
            .NewConfig()
            .Ignore(dest => dest.Password);

        TypeAdapterConfig<RequestExpenseJson, Expense>
            .NewConfig()
            .Map(dest => dest.Tags, src => src.Tags.Distinct());

        TypeAdapterConfig<Communication.Enums.Tag, Tag>
            .NewConfig()
            .Map(dest => dest.Value, src => src);
    }

    private static void EntityToResponse()
    {
        TypeAdapterConfig<Expense, ResponseExpenseJson>
            .NewConfig()
            .Map(dest => dest.Tags, src => src.Tags.Select(tag => tag.Value));
    }
}
