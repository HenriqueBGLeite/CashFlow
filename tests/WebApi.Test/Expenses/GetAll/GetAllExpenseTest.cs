using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.GetAll;

public class GetAllExpenseTest : CashFlowClassFixture
{
    private const string BaseUrl = "api/expenses";

    private readonly string _token;

    public GetAllExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(baseUrl: BaseUrl, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("expenses").EnumerateArray().ShouldNotBeEmpty();
    }
}
