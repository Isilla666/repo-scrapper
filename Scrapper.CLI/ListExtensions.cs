namespace Scrapper.CLI;

public static class ListExtensions
{
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        var randomSeed = new System.Random();

        while (n > 1)
        {
            n--;
            int k = randomSeed.Next(n + 1);
            list.Swap(k, n);
        }

        return list;
    }
    public static void Swap<T>(this IList<T> list, int k, int n)
    {
        (list[k], list[n]) = (list[n], list[k]);
    }
    public static IList<T> Stretch<T>(this IList<T> list, int size)
    {
        var tempArray = list.Take(size).ToList();
        var tempIndex = 0;
        while (tempArray.Count < size)
        {
            tempArray.Add(list[tempIndex]);
            tempIndex = (tempIndex + 1) % list.Count;
        }
        return tempArray;
    }
}