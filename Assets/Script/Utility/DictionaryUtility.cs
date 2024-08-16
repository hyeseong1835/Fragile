using System.Collections.Generic;

public static class DictionaryUtility
{
    public static TValue GetValueOrAddKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
    {
        if (dictionary.TryGetValue(key, out TValue value) == false)
        {
            dictionary.Add(key, defaultValue);
            value = defaultValue;
        }
        return value;
    }
    public static bool TryGetValueOrAddKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, out TValue value, TValue defaultValue = default)
    {
        if (dictionary.TryGetValue(key, out value))
        {
            return true;
        }
        else
        {
            dictionary.Add(key, defaultValue);

            value = defaultValue;
            return false;
        }
    }

}
