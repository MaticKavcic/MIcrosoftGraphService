using MicrosoftGraphService.Model;

namespace MicrosoftGraphServiceServer
{
    static class Program
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args) {
            try
            {
                Server server = new Server(args.Length > 0 ? args[0] : "MicrosoftGraphService");

                server.SetRequestHandler(
                    RequestType.GET_EMAILS,
                    ServerHandlers.GetEmailsHandler
                );
                server.SetRequestHandler(
                    RequestType.GET_EMAILS_DETAILED,
                    ServerHandlers.GetEmailsDetailedHandler
                );
                server.SetRequestHandler(
                    RequestType.SEND_EMAIL,
                    ServerHandlers.SendEmailHandler
                );
                server.SetRequestHandler(
                    RequestType.DELETE_EMAIL,
                    ServerHandlers.DeleteEmailHandler
                );

                server.Start();

                int timeout = args.Length > 1 ? Convert.ToInt32(args[1]) : 5;
                int minutesPassed = 0;

                do
                {
                    minutesPassed = Convert.ToInt32(DateTime.Now.Subtract(server.LastRequest).TotalMinutes);

                    logger.Info($"The server is waiting for a connection. {timeout - minutesPassed}m remaining.");

                    Thread.Sleep(60 * 1000);
                } while (minutesPassed < timeout);

                logger.Info($"No request recived in {timeout}m. Terminating server...");
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                Environment.Exit(1);
            }
        }
    }
}