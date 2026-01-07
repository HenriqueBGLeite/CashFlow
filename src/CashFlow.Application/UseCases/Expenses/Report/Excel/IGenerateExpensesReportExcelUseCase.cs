namespace CashFlow.Application.UseCases.Expenses.Report.Excel;

public interface IGenerateExpensesReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
