using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.IO;

using LibraryModel;

namespace MergeSortQuery {
	class MergeSortQuery {
		public Library Library { get; set; }
		public int ThreadCount { get; set; }
		public List<Copy> Copies => this.Library.Copies;

		public List<Copy> ExecuteQuery()
		{
			if (ThreadCount == 0) throw new InvalidOperationException("Threads property not set and default value 0 is not valid.");

			//CODE HERE
			MergeSortTreeNodeJob RootJob = new MergeSortTreeNodeJob(ThreadCount, 0, this.Copies.Count - 1, 0, Copies);
			RootJob.RunMainJob();
			return RootJob.Result;
		}
	}

	class ResultVisualizer {
		public static void PrintCopy(StreamWriter writer, Copy c) {
			writer.WriteLine("{0} {1}: {2} loaned to {3}, {4}.", c.OnLoan.DueDate.ToShortDateString(), c.Book.Shelf, c.Id, c.OnLoan.Client.LastName, System.Globalization.StringInfo.GetNextTextElement(c.OnLoan.Client.FirstName));
		}
	}
	class MergeSortTreeNodeJob
	{
		public List<Copy> Data { get; set; }
		private int NumThreads { get; set; }
		private int StartIndex { get; set; }
		private int EndIndex { get; set; }
		public List<Copy> Result { get; set; }
		int levelInTree;


		public MergeSortTreeNodeJob(int numThreads, int start, int end, int level, List<Copy> data)
		{
			this.Data = data;
			this.NumThreads = numThreads;
			this.StartIndex = start;
			this.EndIndex = end;
			this.levelInTree = level;
		}

		public void RunMainJob()
		{
			if (this.NumThreads <= 0)
			{
				throw new ImTotallyStupidException("Some mistake occured while recounting numbers of threads, its <= 0");
			}
			if (this.NumThreads == 1)
			{
				this.Result = FilterAndSort();
				return;
			}
			//recount indeces and nums of threads, span new threads and wait fot them
			int numThreads1 = this.NumThreads / 2;
			//second part of a tree might contain one more thread then the firts one
			int numThreads2 = (this.NumThreads / 2) + (this.NumThreads % 2);
			int numElements = (this.EndIndex - this.StartIndex) + 1;
			int start1 = this.StartIndex;
			//and it might also be responsible for sorting one more element
			int end1 = (numElements / 2) - 1 + this.StartIndex;
			int start2 = end1 + 1;
			int end2 = this.EndIndex;
			MergeSortTreeNodeJob JobNode1 = new MergeSortTreeNodeJob(numThreads1, start1, end1, (this.levelInTree+1), this.Data);
			Console.WriteLine($"The first thread of level {JobNode1.levelInTree} sorts {start1} - {end1}");
			MergeSortTreeNodeJob JobNode2 = new MergeSortTreeNodeJob(numThreads2, start2, end2, (this.levelInTree+1), this.Data);
			Console.WriteLine($"The second thread of level {JobNode2.levelInTree} sorts {start2} - {end2}");
			Thread thread1 = new Thread(JobNode1.RunMainJob);
			thread1.Name = "Thread of the level " + JobNode1.levelInTree;
			Thread thread2 = new Thread(JobNode2.RunMainJob);
			thread2.Name = "Thread of the level " + JobNode2.levelInTree;
			thread1.Start();
			thread2.Start();
			thread1.Join();
			thread2.Join();

			//MERGE
			Console.WriteLine($"{Thread.CurrentThread.Name} finished waiting for {thread1.Name} and {thread2.Name}");
			Console.WriteLine("Merging");
			List<Copy> merged = Merge(JobNode1.Result, JobNode2.Result);
			this.Result = merged;
			return;			
		}
		private List<Copy> FilterAndSort()
		{
			List<Copy> res = new List<Copy>();
			for (int i =this.StartIndex; i <= this.EndIndex; i++)
			{
				if (IsOnLoanAQCopy(this.Data[i]))
				{
					res.Add(this.Data[i]);
				}
			}
			res.Sort(CopyComparer.CompareOnLoanCopies);
			return res;
		}

		private List<Copy> Merge(List<Copy> firstList, List<Copy> secondList)
		{
			int firstIndex = 0;
			int secondIndex = 0;
			int resultIndex = 0;
			List<Copy> result = new List<Copy>();
			while(firstIndex < firstList.Count && secondIndex< secondList.Count)
			{
				if(firstList[firstIndex].LessThen(secondList[secondIndex]))
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
			if(firstIndex <firstList.Count)
			{
				for (int i = firstIndex; i  < firstList.Count; i ++)
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
		private bool IsOnLoanAQCopy(Copy copy)
		{
			if (copy.OnLoan == null)
			{
				//book is not on loan
				return false;
			}
			char shelfCodeChar = copy.Book.Shelf[2];
			if (shelfCodeChar < 65 || shelfCodeChar > 90)
			{
				throw new ArgumentException("shelf with invalid identifier char: " + shelfCodeChar);
			}
			if (shelfCodeChar>= 65 && shelfCodeChar<=81)
			{
				//book is on a shelf A-Q
				return true;
			}
			return false;
		}
	}
	
	 static class CopyComparer
	{
		public static bool LessThen(this Copy thisCopy, Copy secondCopy)
		{
			int res = CompareOnLoanCopies(thisCopy, secondCopy);
			if (res < 0)
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Compares ON LOAN copies accordning to given criteria -  due date, last name, firts name, shelf, copy id
		/// </summary>
		/// <param name="firstCopy"></param>
		/// <param name="secondCopy"></param>
		/// <returns>-1 when fistCopy< secondCopy, 0 when copies are equal in all checked criteria, 1 when firtsCopy > secondCopy</secondCopy></returns>
		public static int CompareOnLoanCopies(Copy firstCopy, Copy secondCopy)
		{
			if(firstCopy.OnLoan ==null || secondCopy.OnLoan== null)
			{
				throw new ArgumentException("Trying to compare copies that are not both on loan by a method designed to compare on loan copies");
			}
			//compare primary by a loan due date
			if (firstCopy.OnLoan.DueDate < secondCopy.OnLoan.DueDate)
			{
				return -1;
			}
			if (firstCopy.OnLoan.DueDate > secondCopy.OnLoan.DueDate)
			{
				return 1;
			}
			//due dates are equal, compare secondary by a client last name
			int comparedByLastName = String.Compare(firstCopy.OnLoan.Client.LastName, secondCopy.OnLoan.Client.LastName);
			if (comparedByLastName != 0)
			{
				return comparedByLastName;
			}
			//last names are equal, compare by the first names
			int comparedByFirstName = String.Compare(firstCopy.OnLoan.Client.FirstName, secondCopy.OnLoan.Client.FirstName);
			if(comparedByFirstName != 0)
			{
				return comparedByFirstName;
			}
			//first names are equal, compare by a shelf
			int compareByShelf = String.Compare(firstCopy.Book.Shelf, secondCopy.Book.Shelf);
			if(compareByShelf != 0)
			{
				return compareByShelf;
			}
			//shelves are equal, compare by a copy id
			return String.Compare(firstCopy.Id, secondCopy.Id);
		}
	}
	public class ImTotallyStupidException : Exception 
	{
		public ImTotallyStupidException() : base() { }
		public ImTotallyStupidException(string message) : base(message) { }
	}
}
