namespace salud_vital_proyecto1.DTOs
{
    public class UpdatePatientRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyPhone { get; set; }
    }
}
