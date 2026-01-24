using Newtonsoft.Json.Linq;
using Shouldly;
using System.Net;

namespace WebApi.Test.Users.Delete;

public class DeleteUserTest : CashFlowClassFixture
{
    private const string BaseUrl = "api/User";

    private readonly string _token;

    public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(BaseUrl, _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
