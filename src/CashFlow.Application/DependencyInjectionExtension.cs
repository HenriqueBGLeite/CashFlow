using CashFlow.Application.Services.Mappings;
using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Application.UseCases.Users.Profile;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Application.UseCases.Users.Update;
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
        serivces.AddScoped<IUpdateExpenseUseCase, UpdateExpenseUseCase>();

        serivces.AddScoped<IGenerateExpensesReportExcelUseCase, GenerateExpensesReportExcelUseCase>();
        serivces.AddScoped<IGenerateExpensesReportPdfUseCase, GenerateExpensesReportPdfUseCase>();

        serivces.AddScoped<IGenerateExpensesReportPdfUseCase, GenerateExpensesReportPdfUseCase>();
        
        serivces.AddScoped<IDoLoginUseCase, DoLoginUseCase>();

        serivces.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        serivces.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        serivces.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        serivces.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
    }
}
