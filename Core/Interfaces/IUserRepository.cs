using salud_vital_proyecto1.Core.Entities;

namespace salud_vital_proyecto1.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<T>> GetByTypeAsync<T>() where T : User;
        Task<T> GetByIdAsync<T>(int id) where T : User;
    }
}
