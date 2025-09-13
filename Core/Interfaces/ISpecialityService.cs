using salud_vital_proyecto1.Core.Entities;

namespace salud_vital_proyecto1.Core.Interfaces
{
    public interface ISpecialityService
    {
        Task<IEnumerable<Speciality>> GetSpecialitiesAsync();
    }
}
