using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using Shouldly;
using System.Net;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var useCase = CreateUseCase();
        var request = RequestRegisterUserJsonBuilder.Build();


        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

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
    public async Task Error_Email_Alread_Exists()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

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

    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncripter = new PasswordEncrypterBuilder().Build();
        var tokenGenerator = JwtTokenGeneratorBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder();

        if (string.IsNullOrWhiteSpace(email) == false)
            readRepository.ExistsActiveUserWithEmail(email);

        return new RegisterUserUseCase(passwordEncripter, readRepository.Build(), writeRepository, tokenGenerator, unitOfWork);
    }
}
