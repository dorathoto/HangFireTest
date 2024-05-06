
using Hangfire;
using Hangfire.Server;
using Hangfire.Storage.SQLite;
using HangFireTest.Models;

namespace HangFireTest
{
    public class Program
    {
        [Obsolete]
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //ativando hangfire
            builder.Services.AddHangfire(configuration => configuration
                .UseRecommendedSerializerSettings()
                .UseSQLiteStorage("Hangfire.db")); //poderia ser o MSSQL do projeto

            var app = builder.Build();

            app.UseHangfireDashboard(); //dashboad do hangfire


            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
         

            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Executando agora"));

            //agendamentos
            BackgroundJob.Schedule(
                        () => Console.WriteLine("Job Agendado, 2  minutos após o início da aplicação"),
                        TimeSpan.FromMinutes(2));


            var rnd = new Random().Next(1, 200);
            RecurringJob.AddOrUpdate(
                        "Meu job recorrente",
                        () => Console.WriteLine((rnd % 2 == 0)
                            ? "Job recorrente gerou um número par"
                            : "Job recorrente gerou um número ímpar"),
                        Cron.Minutely,
                        TimeZoneInfo.Local);


            RecurringJob.AddOrUpdate("TaskMethod()", (TaskSample t) => t.TaskMethod(), Cron.Minutely);
            RecurringJob.AddOrUpdate("TaskMethod2()", (TaskSample t) => t.TaskMethod2(null), Cron.Minutely);

            var t = app.Services.GetService<IBackgroundJobClient>();
            t.Enqueue(queue: "test_queue_1", methodCall: () => Console.WriteLine("Testing......"));
            t.Enqueue(queue: "test_queue_1", methodCall: () => Console.WriteLine("Testing......"));
            t.Enqueue(queue: "test_queue_1", methodCall: () => Console.WriteLine("Testing......"));
            t.Enqueue(queue: "test_queue_1", methodCall: () => Console.WriteLine("Testing......"));
            t.Enqueue(queue: "test_queue_1", methodCall: () => Console.WriteLine("Testing......"));
            t.Enqueue(queue: "test_queue_1", methodCall: () => Console.WriteLine("Testing......"));

            app.Run();
        }
    }
}
