using System.Collections.Generic;

namespace LazyCoder.Collect
{
    public static class CollectDestinationManager
    {
        private static Dictionary<CollectConfig, Stack<CollectDestination>> s_destinationDict;

        public static void Push(CollectDestination destination)
        {
            if (s_destinationDict == null)
                s_destinationDict = new Dictionary<CollectConfig, Stack<CollectDestination>>();

            if (!s_destinationDict.ContainsKey(destination.Config))
                s_destinationDict.Add(destination.Config, new Stack<CollectDestination>());

            s_destinationDict[destination.Config].Push(destination);
        }

        public static void Pop(CollectDestination destination)
        {
            if (s_destinationDict == null)
                return;

            Stack<CollectDestination> stack;

            s_destinationDict.TryGetValue(destination.Config, out stack);

            if (stack == null || stack.Count == 0)
                return;

            stack.Pop();
        }

        public static CollectDestination Get(CollectConfig config)
        {
            if (s_destinationDict == null)
                return null;

            if (!s_destinationDict.ContainsKey(config))
                return null;

            Stack<CollectDestination> stack = s_destinationDict[config];

            if(stack == null || stack.Count == 0) 
                return null;

            return stack.Peek();
        }
    }
}
