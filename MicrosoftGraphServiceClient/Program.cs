using MicrosoftGraphService.Model;
using System.Text;

namespace MicrosoftGraphServiceClient
{
    static class Program
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args) {
            try
            {
                Client client = new Client(
                    "MicrosoftGraphService",
                    new Credentials(
                        "",
                        "",
                        "",
                        ""
                    )
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(1);
            }
        }
    }
}