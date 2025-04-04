using UsersAndAuth.Models;

namespace UsersAndAuth.Services;

public interface IUserService
{
	Task<List<User>> GetUsersAsync();
	Task<User?> GetUserByIdAsync(string userId);
	Task<User?> GetUserByUserNameAsync(string userName);
}