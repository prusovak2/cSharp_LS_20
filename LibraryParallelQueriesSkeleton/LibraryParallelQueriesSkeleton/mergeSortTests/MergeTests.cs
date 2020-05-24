using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using LibraryModel;
using MergeSortQuery;
using System;
using System.IO;

namespace mergeSortTests
{
    [TestClass]
    public class MergeTests
    {
		[TestMethod]
		public void AsciiTest()
		{
			char A = 'A';
			Assert.AreEqual(65, A);
			string test = "AHOJ";
			Assert.AreEqual(65, test[0]);
		}
        [TestMethod]
        public void MergeTest()
        {
			List<int> list1 = new List<int>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < 11; i++)
			{
				list1.Add(i);
				list2.Add(2 * i);
			}
			list1.Add(42);
			list1.Add(73);
			foreach (var item in list1)
			{
				Console.Write(item + " ");
			}
			Console.WriteLine();
			foreach (var item in list2)
			{
				Console.Write(item + " ");
			}
			Console.WriteLine();
			var res = Merge(list1, list2);
			foreach (var item in res)
			{
				Console.Write(item+ " ");
			}
        }
		public List<int> Merge(List<int> firstList, List<int> secondList)
		{
			int firstIndex = 0;
			int secondIndex = 0;
			int resultIndex = 0;
			List<int> result = new List<int>();
			while (firstIndex < firstList.Count && secondIndex < secondList.Count)
			{
				if (firstList[firstIndex].LessThen(secondList[secondIndex]))
				{
					result.Add(firstList[firstIndex]);
					firstIndex++;
				}
				else
				{
					result.Add(secondList[secondIndex]);
					secondIndex++;
				}
				resultIndex++;
			}
			if (firstIndex < firstList.Count)
			{
				for (int i = firstIndex; i < firstList.Count; i++)
				{
					result.Add(firstList[i]);
				}
			}
			if (secondIndex < secondList.Count)
			{
				for (int i = secondIndex; i < secondList.Count; i++)
				{
					result.Add(secondList[i]);
				}
			}
			return result;
		}


	}
    public static class intExt
    {
        public static bool LessThen(this int a, int b)
        {
            if (a < b)
            {
                return true;
            }
            return false;
        }
    }
}
