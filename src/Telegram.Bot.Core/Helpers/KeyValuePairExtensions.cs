#if NETSTANDARD2_0
using System.Collections.Generic;

namespace Telegram.Bot.Helpers
{
    internal static class KeyValuePairExtensions
    {
        internal static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key,
                                                       out TValue value)
        {
            key = pair.Key;
            value = pair.Value;
        }
    }
}
#endif
