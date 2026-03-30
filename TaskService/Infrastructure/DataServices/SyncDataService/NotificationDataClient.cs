using System.Net.Mime;
using System.Text;
using System.Text.Json;
using TaskService.Dto;

namespace TaskService.Infrastructure.DataServices.SyncDataService
{
    public class NotificationDataClient : INotificationDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public NotificationDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendToReportService(SendNotificatioDto dto)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(dto),
                Encoding.UTF8, 
                MediaTypeNames.Application.Json);


            Console.WriteLine("SEND MESSAGE TO");
            Console.WriteLine(_configuration["NotificationService"]);

            var post = await _httpClient.PostAsync($"{_configuration["NotificationService"]}/Notifications", httpContent);

            if (post.IsSuccessStatusCode)
            {
                Console.WriteLine("Send to Report Service");
            }
            else
            {
                Console.WriteLine("Not send report to Service");
            }
        }
    }
}
