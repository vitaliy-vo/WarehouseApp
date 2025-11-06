using Microsoft.EntityFrameworkCore;
using Warehouse.Core;
using Warehouse.Core.Dtos;

namespace Warehouse.DAL
{
    public class UserRepository : IUserRepository
    {
        private DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public UserDTO GetByUsername(string username)
        {
            var temp = _dataContext.Users
                 .AsNoTracking()
                 .Where(u => u.Login.Equals(username))
                 .FirstOrDefault();
            return temp;
        }

        public UserDTO Add(UserDTO userDTO)
        {
            var user = _dataContext.Users.Add(userDTO);
            _dataContext.SaveChanges();
            return userDTO;
        }
    }
}
