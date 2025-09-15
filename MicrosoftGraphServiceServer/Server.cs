using System.Text.Json;
using MicrosoftGraphService.Model;
using MicrosoftGraphService.Shared;

namespace MicrosoftGraphServiceServer
{
    class Server : PipeServer
    {
        public delegate Task<string> RequestHandler(string request);

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Dictionary<RequestType, RequestHandler> handlers;

        public Server(string pipeName) : base(pipeName)
        {
            handlers = [];
        }

        public void SetRequestHandler(RequestType type, RequestHandler handler)
        {
            handlers[type] = handler;
        }

        protected override async Task<string> HandleRequest(string message)
        {
            Request? request = JsonSerializer.Deserialize<Request>(message);
            if (request == null)
            {
                logger.Error("Failed to deserialize request.");

                return JsonSerializer.Serialize(new ErrorResponse("Failed to deserialize request."));
            }

            if (handlers.TryGetValue(request.Type, out RequestHandler? handler))
            {
                return await handler(message);
            }
            else
            {
                logger.Error("Client has made a request that has no registered handler.");

                return JsonSerializer.Serialize(new ErrorResponse("This request does not have a registered handler."));
            }
        }
    }
}