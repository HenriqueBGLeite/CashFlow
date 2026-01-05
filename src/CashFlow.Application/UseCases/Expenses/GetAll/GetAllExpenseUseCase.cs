using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using Mapster;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetAllExpenseUseCase : IGetAllExpenseUseCase
{
    private readonly IExpensesReadOnlyRepository _repository;

    public GetAllExpenseUseCase(IExpensesReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseExpensesJson> Execute()
    {
        var result = await _repository.GetAll();

        return new ResponseExpensesJson
        {
            Expenses = result.Adapt<List<ResponseShortExpenseJson>>()
        };
    }
}
