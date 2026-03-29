using TaskService.Dto;

namespace TaskService.SyncDataService
{
    public interface INotificationDataClient
    {
        Task SendToReportService(SendNotificatioDto dto);
    }
}
