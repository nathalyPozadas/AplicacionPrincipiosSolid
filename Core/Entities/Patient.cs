using System.Text.Json.Serialization;

namespace salud_vital_proyecto1.Core.Entities
{
    public class Patient : User
    {
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyPhone { get; set; }
        [JsonIgnore]
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        public Patient()
        {
            UserType = "Patient"; 
        }
    }
}
