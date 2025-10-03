using System.Text.Json;
using MicrosoftGraphService.Model;
using MicrosoftGraphService.Shared;

namespace MicrosoftGraphServiceServer
{
    public class Server : PipeServer
    {
        public delegate Task<string> RequestHandler(string request);

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public DateTime LastRequest { get; private set; }

        private readonly Dictionary<RequestType, RequestHandler> handlers;

        public Server(string pipeName) : base(pipeName)
        {
            handlers = [];
        }

        public void Start()
        {
            logger.Trace("Starting server...");

            LastRequest = DateTime.Now;
            Listen();
        }

        public void SetRequestHandler(RequestType type, RequestHandler handler)
        {
            handlers[type] = handler;
        }

        protected override async Task<string> HandleRequest(string message)
        {
            logger.Trace("Resolving request...");

            LastRequest = DateTime.Now;

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
                logger.Error("Client has made a request that has no associated handler.");

                return JsonSerializer.Serialize(new ErrorResponse("This request does not have an associated handler."));
            }
        }
    }
}