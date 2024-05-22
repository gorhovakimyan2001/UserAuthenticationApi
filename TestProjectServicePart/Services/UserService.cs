using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using TestProjectDbPart.Data;
using TestProjectDbPart.Models;
using TestProjectServicePart.IServices;
using TestProjectServicePart.ModelDtos;
namespace TestProjectServicePart.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;

        public UserService( UserContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUser(UserModelDto user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                throw new Exception("Email can not be empty");
            }

            UserModel userDb = new UserModel() 
            { 
                Age = user.Age,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };

            await _context.Users.AddAsync(userDb);
            int response = _context.SaveChanges();
            return response > 0;
        }
        public async Task<bool> EditUser(UserModel user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);

            if (existingUser != null)
            {
                _context.Entry(existingUser).State = EntityState.Detached;
                _context.Users.Attach(user);
                _context.Entry(user).State = EntityState.Modified;
                int response = _context.SaveChanges();
                return response > 0;
            }
            else
            {
                return false;
            }
        }

        public async Task<UserModelDto> GetUser(int userId)
        {
            UserModel userDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (userDb == null)
            {
                return null;
            }

            UserModelDto user = new UserModelDto()
            {
                Age = userDb.Age,
                FirstName = userDb.FirstName,
                LastName = userDb.LastName,
                Email = userDb.Email,
            };

            return user;
        }

        public IEnumerable<UserModelDto> GetUsers()
        {
            List<UserModelDto> users = new List<UserModelDto>();

            foreach (UserModel user in _context.Users)
            {
                users.Add(new UserModelDto()
                {
                    Age = user.Age,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                });
            }

            return users;
        }

        public async Task<bool> RemoveUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
               return false;
            }

            _context.Users.Remove(user);
            bool response = _context.SaveChanges() > 0;
            return response;
        }
    }
}
