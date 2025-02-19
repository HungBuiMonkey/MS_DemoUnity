using System.Collections;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public static class Extensions
{
    private static System.Random rand = new System.Random();

    public static IEnumerable<T> Shuffle<T>(this IList<T> values)
    {
        for (int i = values.Count - 1; i > 0; i--)
        {
            int k = rand.Next(i + 1);
            T value = values[k];
            values[k] = values[i];
            values[i] = value;
        }
        return values;
    }

    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        var knownKeys = new HashSet<TKey>();
        return source.Where(element => knownKeys.Add(keySelector(element)));
    }

    public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue defaultValue = default(TValue))
    {
        TValue found;

        if (source.TryGetValue(key, out found))
        {
            return found;
        }

        return defaultValue;
    }

    public static T ConvertValue<T, U>(U value) where U : IConvertible
    {
        return (T)Convert.ChangeType(value, typeof(T));
    }

    public static T GetRandom<T>(this IList<T> list)
    {
        return list.ElementAtOrDefault(UnityEngine.Random.Range(0, list.Count));
    }
    public static KeyValuePair<T, U> GetRandom<T, U>(this Dictionary<T, U> dict)
    {
        return dict.ElementAt(UnityEngine.Random.Range(0, dict.Count));
    }

    public static void BringToFront<T>(this List<T> list, int index)
    {
        T item = list[index];
        list.RemoveAt(index);
        list.Insert(0, item);
    }
    public static void Swap<T>(this List<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    public static string ToJsonString<T>(this List<T> list, char separator = '[')
    {
        var closeSeparator = ']';
        closeSeparator = separator == '[' ? ']' : closeSeparator;
        closeSeparator = separator == '(' ? ')' : closeSeparator;
        closeSeparator = separator == '{' ? '}' : closeSeparator;
        var result = "";
        result += separator;
        foreach (var item in list)
        {
            result += item + ",";
        }
        result = result.Remove(result.Length - 1);
        result += closeSeparator;
        return result;
    }

    [Obsolete("change pivot without changing position, this function only work with anchors are not stretch")]
    public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
    {
        if (rectTransform == null) return;

        Vector2 size = rectTransform.rect.size;
        Vector2 deltaPivot = rectTransform.pivot - pivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPosition;
    }

    [Obsolete("change anchors without changing position, this function only work with anchors are not stretch")]
    public static void SetAnchorPointWithoutChangePosition(this RectTransform rectTransform, Vector2 anchor)
    {
        if (rectTransform == null) return;
        if (rectTransform.anchorMin != rectTransform.anchorMax) return;//make sure rect is not stretch
        var parentRect = rectTransform.parent.GetComponent<RectTransform>();
        if (parentRect == null) return;

        Vector2 parentSize = parentRect.sizeDelta;
        var newPos = rectTransform.anchoredPosition - (anchor - rectTransform.anchorMin) * parentSize;
        rectTransform.anchorMin = anchor;
        rectTransform.anchorMax = anchor;
        rectTransform.anchoredPosition = newPos;

    }

    //simulate "also set position" to center in unity editor
    public static void ToCenter(this RectTransform rectTransform)
    {
        var alpha = Vector2.one - rectTransform.anchorMax - rectTransform.anchorMin;
        var parentRect = rectTransform.parent.GetComponent<RectTransform>();
        var centerPosition = parentRect.sizeDelta * alpha / 2;

        var delta = Vector2.one / 2 - rectTransform.pivot;
        centerPosition -= rectTransform.sizeDelta * delta;

        rectTransform.anchoredPosition = centerPosition;
    }

}