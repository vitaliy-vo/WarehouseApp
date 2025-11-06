using Warehouse.Core.Dtos;

namespace Warehouse.DAL
{
    public interface IUserRepository
    {
        UserDTO GetByUsername(string username);
        UserDTO Add(UserDTO userDTO);
    }
}