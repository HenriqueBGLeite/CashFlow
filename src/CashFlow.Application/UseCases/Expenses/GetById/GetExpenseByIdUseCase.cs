using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using Mapster;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpensesRepository _repository;

    public GetExpenseByIdUseCase(IExpensesRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var result = await _repository.GetById(id);

        return result.Adapt<ResponseExpenseJson>();
    }
}
