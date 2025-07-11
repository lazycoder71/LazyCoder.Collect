using System;
using System.Text.RegularExpressions;

namespace LazyCoder.Collect
{
    [Serializable]
    public abstract class CollectStep
    {
        [Serializable]
        public enum AddType
        {
            Append = 0,
            Join = 1,
            Insert = 2,
        }

        public virtual string DisplayName { get { return Regex.Replace(ToString().Replace(typeof(CollectStep).ToString(), ""), "(?<!^)([A-Z])", " $1"); } }

        public abstract void Apply(CollectObject obj);
    }
}
