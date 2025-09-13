using Microsoft.EntityFrameworkCore;
using salud_vital_proyecto1.Core.Entities;
using salud_vital_proyecto1.Core.Interfaces;
using salud_vital_proyecto1.Infraestructure.Data;

namespace salud_vital_proyecto1.Infraestructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User> GetByEmailAsync(string email) =>
            await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<IEnumerable<T>> GetByTypeAsync<T>() where T : User =>
            await _dbSet.OfType<T>().ToListAsync();

        public async Task<T> GetByIdAsync<T>(int id) where T : User =>
            await _dbSet.OfType<T>().FirstOrDefaultAsync(u => u.Id == id);
    }
}
