using CashFlow.Application.Services.Mappings;
using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection serivces)
    {
        AddUseCases(serivces);
        AddMapperConfigurations();
    }

    public static void AddMapperConfigurations()
    {
        MapConfigurations.Configure();
    }

    public static void AddUseCases(IServiceCollection serivces)
    {
        serivces.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
        serivces.AddScoped<IGetAllExpenseUseCase, GetAllExpenseUseCase>();
        serivces.AddScoped<IGetExpenseByIdUseCase, GetExpenseByIdUseCase>();
        serivces.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();
    }
}
