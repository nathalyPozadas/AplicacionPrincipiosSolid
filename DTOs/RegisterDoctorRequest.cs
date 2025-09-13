namespace salud_vital_proyecto1.DTOs
{
    public class RegisterDoctorRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int SpecialityId { get; set; }
        public string LicenseNumber { get; set; }
    }
}
