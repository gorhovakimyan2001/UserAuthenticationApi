using TestProjectDbPart.Models;
using TestProjectServicePart.ModelDtos;
namespace TestProjectServicePart.IServices
{
    public interface IUserService
    {
        Task<bool> AddUser(UserModelDto user);

        Task<bool> RemoveUser(int userId);

        Task<bool> EditUser(UserModel user);

        Task<UserModelDto> GetUser(int userId);

        IEnumerable<UserModelDto> GetUsers();
    }
}
