using System.Collections.Generic;

public static class ListLib
{
		private static System.Random rng = new System.Random();

		public static void Shuffle<T>(this IList<T> list)
		{
				int n = list.Count;
				while (n > 1)
				{
						n--;
						int k = rng.Next(n + 1);
						T value = list[k];
						list[k] = list[n];
						list[n] = value;
				}
		}
		public static T RandomFromArray<T>(this T[] list)
		{
				return list[UnityEngine.Random.Range(0, list.Length)];
		}

		/// <summary>
		/// Adds all the elements in the enumerable to this list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="otherList"></param>
		public static void AddMany<T>(this IList<T> list, IEnumerable<T> otherList)
		{
				foreach (var item in otherList)
				{
						list.Add(item);
				}
		}
}