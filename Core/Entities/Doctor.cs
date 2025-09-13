using System.Text.Json.Serialization;

namespace salud_vital_proyecto1.Core.Entities
{
    public class Doctor : User
    {
        public int SpecialityId { get; set; }
        public Speciality Speciality { get; set; }
        public string LicenseNumber { get; set; }
        public bool IsActive { get; set; } = true;
        [JsonIgnore]

        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        public Doctor()
        {
            UserType = "Doctor";
        }
    }
}
