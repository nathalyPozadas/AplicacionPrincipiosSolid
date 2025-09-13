namespace salud_vital_proyecto1.Core.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string recipient, string message);
    }
}
