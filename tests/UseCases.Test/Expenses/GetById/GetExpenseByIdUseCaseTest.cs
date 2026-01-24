using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Communication.Enums;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapster;
using CommonTestUtilities.Repositories;
using Shouldly;
using System.Net;

namespace UseCases.Test.Expenses.GetById;

public class GetExpenseByIdUseCaseTest
{
    public GetExpenseByIdUseCaseTest()
    {
        MapsterBuilder.Build();
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);

        var result = await useCase.Execute(expense.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(expense.Id);
        result.Title.ShouldBe(expense.Title);
        result.Description.ShouldBe(expense.Description);
        result.Date.ShouldBe(expense.Date);
        result.Amount.ShouldBe(expense.Amount);
        result.PaymentType.ShouldBe((PaymentType)expense.PaymentType);
        result.Tags.ShouldNotBeEmpty();
        //TODO - Revisar como funciona no Shouldly
        //result.Tags.ShouldBeEquivalentTo(expense.Tags.Select(tag => tag.Value));
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.ShouldSatisfyAllConditions(exception =>
        {
            exception.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
            exception.GetErrors().ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            });
        });
    }

    private GetExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetExpenseByIdUseCase(repository, loggedUser);
    }
}
