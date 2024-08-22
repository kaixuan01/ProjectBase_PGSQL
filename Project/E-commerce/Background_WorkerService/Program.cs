using Background_WorkerService.Extension;
using Background_WorkerService.Worker;
using DAL;
using Utils.Tools;

var builder = Host.CreateApplicationBuilder(args);

// Add services to the container.
var sqlConnString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSqlServer<MyDbContext>(sqlConnString);

// Register AuthToken as a singleton
builder.Services.AddSingleton<AuthToken>();
builder.Services.AddSingleton<EncryptionHelper>();
builder.Services.AddHostedService<SendEmailWorker>();

// Auto Add All Services
builder.Services.AddAllService();
builder.Services.AddHttpContextAccessor();


var host = builder.Build();


// Auto Create Database or Update Database table
// Not sure can use in production or not
host.CreatOrUpdateDatabase();

host.Run();
