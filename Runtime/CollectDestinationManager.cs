using System.Collections.Generic;

namespace LazyCoder.Collect
{
    public static class CollectDestinationManager
    {
        // The most recently enabled destination of a config wins, it sits last in its list
        private static Dictionary<CollectConfig, List<CollectDestination>> s_destinationDict;

        public static void Push(CollectDestination destination)
        {
            if (s_destinationDict == null)
                s_destinationDict = new Dictionary<CollectConfig, List<CollectDestination>>();

            if (!s_destinationDict.TryGetValue(destination.Config, out List<CollectDestination> destinations))
            {
                destinations = new List<CollectDestination>();

                s_destinationDict.Add(destination.Config, destinations);
            }

            destinations.Add(destination);
        }

        public static void Pop(CollectDestination destination)
        {
            if (s_destinationDict == null)
                return;

            if (!s_destinationDict.TryGetValue(destination.Config, out List<CollectDestination> destinations))
                return;

            // Destinations do not always disable in the reverse order they enabled, remove this one exactly
            destinations.Remove(destination);
        }

        public static CollectDestination Get(CollectConfig config)
        {
            if (s_destinationDict == null)
                return null;

            if (!s_destinationDict.TryGetValue(config, out List<CollectDestination> destinations))
                return null;

            if (destinations.Count == 0)
                return null;

            return destinations[^1];
        }
    }
}
