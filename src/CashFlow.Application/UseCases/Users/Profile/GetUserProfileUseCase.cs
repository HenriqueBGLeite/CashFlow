using CashFlow.Communication.Responses;
using CashFlow.Domain.Services.LoggedUser;
using Mapster;

namespace CashFlow.Application.UseCases.Users.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;

    public GetUserProfileUseCase(ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.Get();

        return user.Adapt<ResponseUserProfileJson>();
    }
}
