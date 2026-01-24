using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class ExpenseBuilder
{
    public static List<Expense> Collection(User user, uint count = 2)
    {
        var list = new List<Expense>();

        if (count == 0)
            count = 1;

        var expenseId = 1;

        for (int i = 0; i < count; i++)
        {
            var expense = Build(user);
            expense.Id = expenseId++;

            list.Add(expense);
        }

        return list;
    }

    public static Expense Build(User user)
    {
        return new Faker<Expense>()
            .RuleFor(expense => expense.Id, _ => 1)
            .RuleFor(expense => expense.Title, faker => faker.Commerce.ProductName())
            .RuleFor(expense => expense.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(expense => expense.Date, faker => faker.Date.Past())
            .RuleFor(expense => expense.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(expense => expense.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(expense => expense.UserId, _ => user.Id)
            .RuleFor(expense => expense.Tags, faker => faker.Make(1, () => new CashFlow.Domain.Entities.Tag
            {
                Id = 1,
                Value = faker.PickRandom<CashFlow.Domain.Enums.Tag>(),
                ExpenseId = 1,
            }));
    }
}
