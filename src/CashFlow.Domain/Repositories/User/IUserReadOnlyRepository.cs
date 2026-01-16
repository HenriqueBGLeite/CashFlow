namespace CashFlow.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistsActiveUserWithEmail(string email);
    Task<Entities.User?> GetUserByEmail(string email);
}
