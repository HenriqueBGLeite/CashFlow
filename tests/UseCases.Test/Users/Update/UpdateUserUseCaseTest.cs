using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;

namespace UseCases.Test.Users.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();

        user.Name.ShouldBe(request.Name);
        user.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var user = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.ShouldSatisfyAllConditions(exception =>
        {
            exception.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            exception.GetErrors().ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(ResourceErrorMessages.NAME_EMPTY);
            });
        });
    }

    [Fact]
    public async Task Error_Email_Already_Exist()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.ShouldSatisfyAllConditions(exception =>
        {
            exception.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            exception.GetErrors().ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);
            });
        });
    }

    private UpdateUserUseCase CreateUseCase(CashFlow.Domain.Entities.User user, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var readRepository = new UserReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (string.IsNullOrWhiteSpace(email) == false)
            readRepository.ExistsActiveUserWithEmail(email);

        return new UpdateUserUseCase(loggedUser, updateRepository, readRepository.Build(), unitOfWork);
    }
}
