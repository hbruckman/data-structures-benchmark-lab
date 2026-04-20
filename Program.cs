public class Program
{
	public static void Main()
	{
		var benchmark = new DataStructureBenchmarkReport();
		string report = benchmark.Run();
		Console.WriteLine(report);
	}
}
