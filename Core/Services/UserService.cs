using Microsoft.EntityFrameworkCore;
using salud_vital_proyecto1.Core.Entities;
using salud_vital_proyecto1.Core.Interfaces;
using salud_vital_proyecto1.Infraestructure.Repositories;

namespace salud_vital_proyecto1.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Speciality> _specialityRepository;

        public UserService(UserRepository userRepository, IRepository<Speciality> specialityRepository)
        {
            _userRepository = userRepository;
            _specialityRepository = specialityRepository;
        }
        public async Task<Doctor> RegisterDoctorAsync(Doctor doctor)
        {
            var speciality = await _specialityRepository.GetByIdAsync(doctor.SpecialityId);
            if (speciality == null) throw new InvalidOperationException("Especialidad no existe");

            var existingUser = await _userRepository.GetByEmailAsync(doctor.Email);
            if (existingUser != null) throw new InvalidOperationException("Email ya registrado");

            await _userRepository.AddAsync(doctor);
            await _userRepository.SaveChangesAsync();
            return doctor;
        }

        public async Task<Patient> RegisterPatientAsync(Patient patient)
        {
            var existingUser = await _userRepository.GetByEmailAsync(patient.Email);
            if (existingUser != null) throw new InvalidOperationException("Email ya registrado");

            await _userRepository.AddAsync(patient);
            await _userRepository.SaveChangesAsync();
            return patient;
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            await _userRepository.UpdateAsync(patient);
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            await _userRepository.UpdateAsync(doctor);
        }

        public async Task<Doctor?> GetDoctorAsync(int id)
        {
            return await _userRepository.GetByIdAsync<Doctor>(id);
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _userRepository.GetByTypeAsync<Doctor>();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public Task<Patient> GetPatientAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
