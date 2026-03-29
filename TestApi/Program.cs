using Microsoft.EntityFrameworkCore;
using TaskService.AsyncDataService;
using TaskService.Data;
using TaskService.Data.Interfaces;
using TaskService.Interceptors;
using TaskService.SyncDataService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddHttpClient<INotificationDataClient, NotificationDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddScoped<DomainEventInterceptor>();
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddGrpc();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultDBConnectionString")));
}

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();
PrepDb.PreparingPopulation(app, !app.Environment.IsDevelopment());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcTasksService>();
app.MapGet("/protos/tasks.proto", async context => 
    await context.Response.WriteAsync(
        File.ReadAllText("/Protos/tasks.proto"))
    );

app.Run();