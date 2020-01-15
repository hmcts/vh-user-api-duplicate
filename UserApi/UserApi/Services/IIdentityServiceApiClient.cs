using System.Collections.Generic;
using System.Threading.Tasks;
using UserApi.Services.Models;

namespace UserApi.Services
{
    public interface IIdentityServiceApiClient
    {
        Task<IEnumerable<string>> GetUsernamesStartingWithAsync(string text);
        Task<NewAdUserAccount> CreateUserAsync(string username, string firstName, string lastName, string displayName, string recoveryEmail);
        Task DeleteUserAsync(string username);
        Task UpdateUserAsync(string username);
    }
}