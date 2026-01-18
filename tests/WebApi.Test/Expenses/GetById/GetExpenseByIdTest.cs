using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.GetById;

public class GetExpenseByIdTest : CashFlowClassFixture
{
    private const string BaseUrl = "api/expenses";

    private readonly string _token;
    private readonly long _expenseId;

    public GetExpenseByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(baseUrl: $"{BaseUrl}/{_expenseId}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().ShouldBe(_expenseId);
        response.RootElement.GetProperty("title").GetString().ShouldNotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("description").GetString().ShouldNotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("date").GetDateTime().ShouldBeLessThan(DateTime.Today);
        response.RootElement.GetProperty("amount").GetDecimal().ShouldBeGreaterThan(0);

        var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();
        Enum.IsDefined(typeof(PaymentType), paymentType).ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var result = await DoGet(baseUrl: $"{BaseUrl}/1000", token: _token, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => string.IsNullOrWhiteSpace(error.GetString()) == false && error.GetString()!.Equals(expectedMessage));
        });
    }
}
