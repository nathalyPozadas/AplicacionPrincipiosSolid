using salud_vital_proyecto1.Core.Entities;

namespace salud_vital_proyecto1.Core.Interfaces
{
    public interface IAppointmentService
    {
        Task<Appointment> CreateAppointmentAsync(int patientId, int doctorId, DateTime date);
        Task<bool> CancelAppointmentAsync(int appointmentId);
        Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate);
        Task<bool> ApproveAppointmentAsync(int appointmentId);
        Task<bool> RejectAppointmentAsync(int appointmentId);
        Task<IEnumerable<Appointment>> GetDoctorDailyAppointmentsAsync(int doctorId, DateOnly date);
        Task<bool> CompleteAppointmentAsync(int appointmentId);
        Task<bool> AddMedicalSummaryAsync(int appointmentId, string medicalSummary);
    }
}
