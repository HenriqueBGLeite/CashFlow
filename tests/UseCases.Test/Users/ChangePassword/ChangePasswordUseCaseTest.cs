using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;

namespace UseCases.Test.Users.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;

        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.ShouldSatisfyAllConditions(exception =>
        {
            exception.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            exception.GetErrors().ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(ResourceErrorMessages.INVALID_PASSWORD);
            });
        });
    }

    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.ShouldSatisfyAllConditions(exception =>
        {
            exception.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            exception.GetErrors().ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD);
            });
        });
    }

    private ChangePasswordUseCase CreateUseCase(CashFlow.Domain.Entities.User user, string? password = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncripter = new PasswordEncrypterBuilder().Verify(password).Build();

        return new ChangePasswordUseCase(loggedUser, updateRepository, passwordEncripter, unitOfWork);
    }
}
