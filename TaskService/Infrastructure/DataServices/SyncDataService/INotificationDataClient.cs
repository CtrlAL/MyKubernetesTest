using TaskService.Dto;

namespace TaskService.Infrastructure.DataServices.SyncDataService
{
    public interface INotificationDataClient
    {
        Task SendToReportService(SendNotificatioDto dto);
    }
}
