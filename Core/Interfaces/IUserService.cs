using salud_vital_proyecto1.Core.Entities;

namespace salud_vital_proyecto1.Core.Interfaces
{
    public interface IUserService
    {
        Task<Patient> RegisterPatientAsync(Patient patient);
        Task UpdatePatientAsync(Patient patient);

        Task<Patient> GetPatientAsync(int id);
        Task<Doctor> RegisterDoctorAsync(Doctor doctor);
        Task UpdateDoctorAsync(Doctor doctor);
        Task<Doctor?> GetDoctorAsync(int id);
        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        Task DeleteDoctorAsync(int id);

    }
}
