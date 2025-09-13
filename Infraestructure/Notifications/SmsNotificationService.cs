using salud_vital_proyecto1.Core.Interfaces;

namespace salud_vital_proyecto1.Infraestructure.Notifications
{
    public class SmsNotificationService : INotificationService
    {
        public async Task SendNotificationAsync(string recipient, string message)
        {
            Console.WriteLine($"SMS enviado a {recipient}: {message}");
            await Task.Delay(100);
        }
    }
}
