namespace CashFlow.Application.UseCases.Report.Excel;

public interface IGenerateExpensesReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
