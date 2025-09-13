using Microsoft.EntityFrameworkCore;
using salud_vital_proyecto1.Core.Entities;
using salud_vital_proyecto1.Core.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace salud_vital_proyecto1.Core.Services
{
    public class SpecialityService : ISpecialityService
    {
        private readonly IRepository<Speciality> _specialityRepository;

        public SpecialityService(
            IRepository<Speciality> specialityRepository)
        {
            _specialityRepository = specialityRepository;
        }
        public async Task<IEnumerable<Speciality>> GetSpecialitiesAsync()
        {
            return await _specialityRepository.GetAllAsync();
        }


    }
}
