using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

public class DataStructureBenchmarkReport
{
	private const int N = 10000;
	private const int Iterations = 1000;
	private const int MissingValue = int.MinValue; // excluded from generated range

	private readonly Random _random = new Random();

	public string Run()
	{
		int[] sourceArray = GenerateRandomArray(N);
		var results = new List<BenchmarkSummary>();

		BenchmarkArrayList(sourceArray, results);
		BenchmarkLinkedList(sourceArray, results);
		BenchmarkHashtable(sourceArray, results);

		return BuildReport(sourceArray, results);
	}

	private int[] GenerateRandomArray(int count)
	{
		int[] values = new int[count];

		for (int i = 0; i < count; i++)
		{
			values[i] = _random.Next(-1_000_000, 1_000_001);
		}

		return values;
	}

	private void BenchmarkArrayList(int[] sourceArray, List<BenchmarkSummary> results)
	{
		BenchmarkOperation(
			"ArrayList",
			"Add all values",
			results,
			() => new List<int>(),
			arrayList =>
			{
				foreach (int value in sourceArray)
				{
					arrayList.Add(value);
				}
				return "";
			});

		BenchmarkOperation(
			"ArrayList",
			"Linear search",
			results,
			() => new List<int>(sourceArray),
			arrayList =>
			{
				int linearIndex = arrayList.IndexOf(MissingValue);
				return linearIndex.ToString();
			});

		BenchmarkOperation(
			"ArrayList",
			"Sort",
			results,
			() => new List<int>(sourceArray),
			arrayList =>
			{
				arrayList.Sort();
				return "";
			});

		BenchmarkOperation(
			"ArrayList",
			"Binary search",
			results,
			() =>
			{
				var arrayList = new List<int>(sourceArray);
				arrayList.Sort();
				return arrayList;
			},
			arrayList =>
			{
				int binaryIndex = arrayList.BinarySearch(MissingValue);
				return binaryIndex.ToString();
			});

		BenchmarkOperation(
			"ArrayList",
			"Access index n/2",
			results,
			() => new List<int>(sourceArray),
			arrayList =>
			{
				int middleValue = arrayList[arrayList.Count / 2];
				return middleValue.ToString();
			});

		BenchmarkOperation(
			"ArrayList",
			"Remove first",
			results,
			() => new List<int>(sourceArray),
			arrayList =>
			{
				int firstValue = arrayList[0];
				arrayList.RemoveAt(0);
				return firstValue.ToString();
			});

		BenchmarkOperation(
			"ArrayList",
			"Remove last",
			results,
			() => new List<int>(sourceArray),
			arrayList =>
			{
				int lastValue = arrayList[arrayList.Count - 1];
				arrayList.RemoveAt(arrayList.Count - 1);
				return lastValue.ToString();
			});
	}

	private void BenchmarkLinkedList(int[] sourceArray, List<BenchmarkSummary> results)
	{
		BenchmarkOperation(
			"LinkedList",
			"Add all values",
			results,
			() => new LinkedList<int>(),
			linkedList =>
			{
				foreach (int value in sourceArray)
				{
					linkedList.AddLast(value);
				}
				return "";
			});

		BenchmarkOperation(
			"LinkedList",
			"Linear search",
			results,
			() => new LinkedList<int>(sourceArray),
			linkedList =>
			{
				bool linearFound = LinearSearchLinkedList(linkedList, MissingValue);
				return linearFound.ToString();
			});

		BenchmarkOperation(
			"LinkedList",
			"Sort",
			results,
			() => new LinkedList<int>(sourceArray),
			linkedList =>
			{
				SortLinkedList(linkedList);
				return "";
			});

		BenchmarkOperation(
			"LinkedList",
			"Binary search",
			results,
			() =>
			{
				var linkedList = new LinkedList<int>(sourceArray);
				SortLinkedList(linkedList);
				return linkedList;
			},
			linkedList =>
			{
				int binaryIndex = BinarySearchLinkedList(linkedList, MissingValue);
				return binaryIndex.ToString();
			});

		BenchmarkOperation(
			"LinkedList",
			"Access index n/2",
			results,
			() => new LinkedList<int>(sourceArray),
			linkedList =>
			{
				int middleValue = GetValueAt(linkedList, linkedList.Count / 2);
				return middleValue.ToString();
			});

		BenchmarkOperation(
			"LinkedList",
			"Remove first",
			results,
			() => new LinkedList<int>(sourceArray),
			linkedList =>
			{
				int firstValue = linkedList.First!.Value;
				linkedList.RemoveFirst();
				return firstValue.ToString();
			});

		BenchmarkOperation(
			"LinkedList",
			"Remove last",
			results,
			() => new LinkedList<int>(sourceArray),
			linkedList =>
			{
				int lastValue = linkedList.Last!.Value;
				linkedList.RemoveLast();
				return lastValue.ToString();
			});
	}

	private void BenchmarkHashtable(int[] sourceArray, List<BenchmarkSummary> results)
	{
		BenchmarkOperation(
			"Hashtable",
			"Add all values",
			results,
			() => new Dictionary<int, int>(),
			hashTable =>
			{
				foreach (int value in sourceArray)
				{
					hashTable[value] = value;
				}
				return "";
			});

		BenchmarkOperation(
			"Hashtable",
			"Hash search",
			results,
			() =>
			{
				var hashTable = new Dictionary<int, int>();
				foreach (int value in sourceArray)
				{
					hashTable[value] = value;
				}
				return hashTable;
			},
			hashTable =>
			{
				bool linearFound = hashTable.ContainsKey(MissingValue);
				return linearFound.ToString();
			});
	}

	private void BenchmarkOperation<TStructure>(
		string structure,
		string operation,
		List<BenchmarkSummary> results,
		Func<TStructure> setup,
		Func<TStructure, string> measuredOperation)
	{
		long[] samples = new long[Iterations];
		string result = "";

		for (int i = 0; i < Iterations; i++)
		{
			TStructure dataStructure = setup();

			var sw = Stopwatch.StartNew();
			result = measuredOperation(dataStructure);
			sw.Stop();

			samples[i] = sw.ElapsedTicks;
		}

		results.Add(new BenchmarkSummary(structure, operation, samples, result));
	}

	private bool LinearSearchLinkedList(LinkedList<int> list, int target)
	{
		foreach (int item in list)
		{
			if (item == target)
			{
				return true;
			}
		}
		return false;
	}

	private int BinarySearchLinkedList(LinkedList<int> list, int target)
	{
		int left = 0;
		int right = list.Count - 1;

		while (left <= right)
		{
			int mid = left + (right - left) / 2;
			int value = GetValueAt(list, mid);

			if (value == target)
				return mid;

			if (value < target)
				left = mid + 1;
			else
				right = mid - 1;
		}

		return -1;
	}

	private int GetValueAt(LinkedList<int> list, int index)
	{
		if (index < 0 || index >= list.Count)
			throw new ArgumentOutOfRangeException(nameof(index));

		LinkedListNode<int>? current = list.First;
		for (int i = 0; i < index; i++)
		{
			current = current!.Next;
		}

		return current!.Value;
	}

	private void SortLinkedList(LinkedList<int> list)
	{
		List<int> temp = new List<int>(list);
		temp.Sort();

		list.Clear();
		foreach (int value in temp)
		{
			list.AddLast(value);
		}
	}

	private string BuildReport(int[] sourceArray, List<BenchmarkSummary> rows)
	{
		var sb = new StringBuilder();

		sb.AppendLine("DATA STRUCTURE BENCHMARK REPORT");
		sb.AppendLine(new string('=', 120));
		sb.AppendLine($"Generated integers: {sourceArray.Length}");
		sb.AppendLine($"Iterations per operation: {Iterations}");
		sb.AppendLine($"Missing value searched: {MissingValue}");
		sb.AppendLine();

		sb.AppendLine(
			Pad("Structure", 15) +
			Pad("Operation", 22) +
			Pad("Mean", 14) +
			Pad("Std Dev", 14) +
			Pad("Min", 12) +
			Pad("Max", 12) +
			"Result");

		sb.AppendLine(new string('-', 95));

		foreach (var row in rows)
		{
			sb.AppendLine(
				Pad(row.Structure, 15) +
				Pad(row.Operation, 22) +
				Pad(row.MeanTicks.ToString("F2"), 14) +
				Pad(row.StandardDeviationTicks.ToString("F2"), 14) +
				Pad(row.MinTicks.ToString(), 12) +
				Pad(row.MaxTicks.ToString(), 12) +
				row.Result);
		}

		sb.AppendLine();
		sb.AppendLine("Totals by structure (sum of means)");
		sb.AppendLine(new string('-', 50));

		foreach (var structure in rows.Select(r => r.Structure).Distinct())
		{
			double totalMeanTicks = rows
				.Where(r => r.Structure == structure)
				.Sum(r => r.MeanTicks);

			sb.AppendLine($"{structure,-15} Total Mean Ticks: {totalMeanTicks:F2}");
		}

		return sb.ToString();
	}

	private string Pad(string text, int width)
	{
		return text.PadRight(width);
	}

	private sealed class BenchmarkSummary
	{
		public string Structure { get; }
		public string Operation { get; }
		public string Result { get; }

		public long[] Samples { get; }

		public double MeanTicks { get; }
		public double StandardDeviationTicks { get; }
		public long MinTicks { get; }
		public long MaxTicks { get; }

		public BenchmarkSummary(string structure, string operation, long[] samples, string result = "")
		{
			Structure = structure;
			Operation = operation;
			Samples = samples;
			Result = result;

			MeanTicks = samples.Average();

			double variance = samples
				.Select(sample => Math.Pow(sample - MeanTicks, 2))
				.Average();

			StandardDeviationTicks = Math.Sqrt(variance);
			MinTicks = samples.Min();
			MaxTicks = samples.Max();
		}
	}
}
