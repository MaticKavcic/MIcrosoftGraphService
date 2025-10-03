using MicrosoftGraphService.Model;

namespace MicrosoftGraphServiceClient
{
    static class Program
    {
        static async Task Main(string[] args) {
            try
            {
                Client client = new Client(
                    args.Length > 0 ? args[0] : "MicrosoftGraphService",
                    new Credentials(
                        "",
                        "",
                        "",
                        ""
                    )
                );

                await client.GetEmails(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(1);
            }
        }
    }
}