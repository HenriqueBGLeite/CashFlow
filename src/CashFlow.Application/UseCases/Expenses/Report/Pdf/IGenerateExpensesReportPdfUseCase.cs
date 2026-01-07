namespace CashFlow.Application.UseCases.Expenses.Report.Pdf;

public interface IGenerateExpensesReportPdfUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
