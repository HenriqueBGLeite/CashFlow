using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;

    public UserRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistsActiveUserWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));

    public async Task<User> GetById(long id) => await _dbContext.Users.FirstAsync(user => user.Id == id);

    public async Task<User?> GetUserByEmail(string email) => await _dbContext.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(user => user.Email.Equals(email));

    public void Update(User user) => _dbContext.Users.Update(user);
}
