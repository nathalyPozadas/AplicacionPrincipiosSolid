using salud_vital_proyecto1.Core.Interfaces;
using salud_vital_proyecto1.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace salud_vital_proyecto1.Core.Services
{
    public class AppointmentService : IAppointmentService
    {

        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogService _loggerService;

        public AppointmentService(
            IRepository<Appointment> appointmentRepository,
            IRepository<Patient> patientRepository,
            IRepository<Doctor> doctorRepository,
            INotificationService notificationService,
            ILogService loggerService)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _notificationService = notificationService;
            _loggerService = loggerService;
        }

        public async Task<Appointment> CreateAppointmentAsync(int patientId, int doctorId, DateTime date)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
                throw new ArgumentException("Paciente no encontrado", nameof(patientId));

            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
                throw new ArgumentException("Doctor no encontrado", nameof(doctorId));

            var existingAppointments = await GetDoctorAppointmentsAsync(doctorId, date.Date);
            var requestedTime = date.TimeOfDay;

            

            var localDate = date.ToLocalTime();
    
            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentDate = localDate,
                Status = AppointmentStatus.Requested
            };

            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            // Notificar al doctor
            await _notificationService.SendNotificationAsync(
                doctor.Email,
                $"Nueva solicitud de cita del paciente {patient.Name} para el {date:dd/MM/yyyy HH:mm}"
            );

            await _loggerService.LogAsync(
                $"Cita {appointment.Id} creada para paciente {patientId} con doctor {doctorId}",
                LogLevelType.Info
            );

            return appointment;
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                return false;

            if (appointment.Status != AppointmentStatus.Requested &&
                appointment.Status != AppointmentStatus.Confirmed)
                return false;

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();
            var patient = await _patientRepository.GetByIdAsync(appointment.PatientId);
            var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId);

            if (patient != null)
            {
                await _notificationService.SendNotificationAsync(
                    patient.Email,
                    $"Su cita del {appointment.AppointmentDate:dd/MM/yyyy HH:mm} ha sido cancelada"
                );
            }

            if (doctor != null)
            {
                await _notificationService.SendNotificationAsync(
                    doctor.Email,
                    $"La cita {appointmentId} con {patient?.Name} ha sido cancelada"
                );
            }

            await _loggerService.LogAsync(
                $"Cita {appointmentId} cancelada",
                LogLevelType.Info
            );
            return true;
        }

        public async Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                return false;

            if (newDate <= DateTime.Now)
                throw new ArgumentException("La nueva fecha debe ser futura", nameof(newDate));

            var existingAppointments = await GetDoctorAppointmentsAsync(appointment.DoctorId, newDate.Date);
            var requestedTime = newDate.TimeOfDay;

            foreach (var existingAppointment in existingAppointments.Where(a => a.Id != appointmentId))
            {
                var existingTime = existingAppointment.AppointmentDate.TimeOfDay;
                if (Math.Abs((existingTime - requestedTime).TotalMinutes) < 30)
                    throw new InvalidOperationException("El doctor no está disponible en el nuevo horario");
            }

            var oldDate = appointment.AppointmentDate;

            appointment.AppointmentDate = newDate;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            var patient = await _patientRepository.GetByIdAsync(appointment.PatientId);
            var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId);

            if (patient != null)
            {
                await _notificationService.SendNotificationAsync(
                    patient.Email,
                    $"Su cita ha sido reprogramada de {oldDate:dd/MM/yyyy HH:mm} a {newDate:dd/MM/yyyy HH:mm}"
                );
            }

            if (doctor != null)
            {
                await _notificationService.SendNotificationAsync(
                    doctor.Email,
                    $"La cita {appointmentId} con {patient?.Name} ha sido reprogramada para el {newDate:dd/MM/yyyy HH:mm}"
                );
            }

            await _loggerService.LogAsync(
                $"Cita {appointmentId} reprogramada de {oldDate} a {newDate}",
                LogLevelType.Info
            );

            return true;
        }

        public async Task<bool> ApproveAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                return false;

            if (appointment.Status != AppointmentStatus.Requested)
                return false;

            appointment.Status = AppointmentStatus.Confirmed;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            var patient = await _patientRepository.GetByIdAsync(appointment.PatientId);
            var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId);

            if (patient != null)
            {
                await _notificationService.SendNotificationAsync(
                    patient.Email,
                    $"Su cita con el Dr. {doctor?.Name} para el {appointment.AppointmentDate:dd/MM/yyyy HH:mm} ha sido confirmada"
                );
            }

            await _loggerService.LogAsync(
                $"Cita {appointmentId} aprobada por el doctor",
                LogLevelType.Info
            );

            return true;
        }

        public async Task<bool> RejectAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                return false;

            // Solo se pueden rechazar citas en estado Requested
            if (appointment.Status != AppointmentStatus.Requested)
                return false;

            appointment.Status = AppointmentStatus.Rejected;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            // Notificar al paciente
            var patient = await _patientRepository.GetByIdAsync(appointment.PatientId);
            var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId);

            if (patient != null)
            {
                await _notificationService.SendNotificationAsync(
                    patient.Email,
                    $"Su cita con el Dr. {doctor?.Name} para el {appointment.AppointmentDate:dd/MM/yyyy HH:mm} ha sido rechazada"
                );
            }

            await _loggerService.LogAsync(
                $"Cita {appointmentId} rechazada por el doctor",
                LogLevelType.Info
            );

            return true;
        }

        public async Task<IEnumerable<Appointment>> GetDoctorDailyAppointmentsAsync(int doctorId, DateOnly date)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
                throw new ArgumentException("Doctor no encontrado", nameof(doctorId));

            var startDate = date.ToDateTime(TimeOnly.MinValue); 
            var endDate = date.ToDateTime(TimeOnly.MaxValue);   

            var appointments = await _appointmentRepository.Query()
                .Where(a => a.DoctorId == doctorId &&
                            a.AppointmentDate >= startDate &&
                            a.AppointmentDate <= endDate &&
                            a.Status != AppointmentStatus.Cancelled &&
                            a.Status != AppointmentStatus.Rejected)
                .Include(a => a.Patient)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            return appointments;
        }

        private async Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(int doctorId, DateTime date)
        {
            return await _appointmentRepository.Query()
                .Where(a => a.DoctorId == doctorId &&
                           a.AppointmentDate.Date == date.Date &&
                           a.Status != AppointmentStatus.Cancelled &&
                           a.Status != AppointmentStatus.Rejected)
                .ToListAsync();
        }

        public async Task<bool> AddMedicalSummaryAsync(int appointmentId, string medicalSummary)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                return false;

            if (appointment.Status != AppointmentStatus.Completed)
                return false;

            appointment.MedicalSummary = medicalSummary;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            await _loggerService.LogAsync(
                $"Resumen médico agregado a la cita {appointmentId}",
                LogLevelType.Info
            );

            return true;
        }

        public async Task<bool> CompleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                return false;

            if (appointment.Status != AppointmentStatus.Confirmed)
                return false;

            appointment.Status = AppointmentStatus.Completed;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            await _loggerService.LogAsync(
                $"Cita {appointmentId} marcada como completada",
                LogLevelType.Info
            );

            return true;
        }
    }

}
