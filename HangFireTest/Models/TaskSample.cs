using Hangfire.Server;

namespace HangFireTest.Models
{
    public class TaskSample
    {
        public void TaskMethod()
        {
            Console.WriteLine("Testando qualquer coisa!!!");
        }

        public void TaskMethod2(PerformContext? pfContext)
        {
            //pfContext.LogInformation("Testing Web Sample!!!");
            Console.WriteLine("Testandoooo!!!");
        }
    }
}