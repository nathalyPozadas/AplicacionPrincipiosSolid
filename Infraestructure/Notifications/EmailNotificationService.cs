using salud_vital_proyecto1.Core.Interfaces;

namespace salud_vital_proyecto1.Infraestructure.Notifications
{
    public class EmailNotificationService : INotificationService
    {
        public async Task SendNotificationAsync(string recipient, string message)
        {
            // Implementación real de envío de email
            Console.WriteLine($"Email enviado a {recipient}: {message}");
            await Task.Delay(100);
        }
    }
}
