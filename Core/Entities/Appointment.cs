namespace salud_vital_proyecto1.Core.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        //public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(30);
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Requested;
        public string? Notes { get; set; }
        public string? MedicalSummary { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}

public enum AppointmentStatus
{
    Requested,
    Confirmed,
    Cancelled,
    Completed,
    Rejected
}
