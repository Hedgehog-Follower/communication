using Microsoft.Extensions.Logging;
using Polly;

namespace Sending.API.Policies
{
    public static class ContextExtensions
    {
        public static bool TryGetLogger(this Context context, out ILogger logger)
        {
            if (context.TryGetValue(ContextNames.Logger, out var loggerObject) && loggerObject is ILogger sourceLogger)
            {
                logger = sourceLogger;
                return true;
            }

            logger = null;
            return false;
        }
    }
}
