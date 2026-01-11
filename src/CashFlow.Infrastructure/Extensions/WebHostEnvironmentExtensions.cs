using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CashFlow.Infrastructure.Extensions;

public static class WebHostEnvironmentExtensions
{
    public static bool IsTests(this IWebHostEnvironment environment) => environment.IsEnvironment("Tests");
}
