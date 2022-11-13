using System.Collections;

public class Program {
	public static void Main(string[] args) {
		//MockTest_BinaryClock();

		while(true) {
			DateTime now = DateTime.Now;
			BinaryClock(now);
			Thread.Sleep(1000);
			Console.Clear();
		}
	}

	public static void MockTest_BinaryClock() {
		/*		GOAL
			j:	   0 1 2 3 4 5
			i = 3:   0   0   1
			i = 2:   0 0 1 1 0
			i = 1: 0 0 1 1 0 0
			i = 0: 1 0 1 1 0 1
		*/
		string actual = BinaryClock(new DateTime(2000, 1, 1, 10, 37, 49), dev: true);
		string expected = "\r\n  0   0   1 \r\n  0 0 1 1 0 \r\n0 0 1 1 0 0 \r\n1 0 1 1 0 1 \r\n";

		if(actual == expected) Console.WriteLine("Test Passsed");
		else Console.WriteLine("Test Failed");
	}

	public static string BinaryClock(DateTime time, string chars = " 01", bool dev = false) {
		using(StringWriter stringWriter = new()) {
			Console.SetOut(stringWriter);

			(BitArray[] now, int[] lengths) = TransformDate(time);
			Console.WriteLine();
			ClockDisplay(now, lengths, chars);

			string output = stringWriter.ToString();
			StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
			standardOutput.AutoFlush = true;
			Console.SetOut(standardOutput);

			if(!dev) Console.WriteLine(output);

			return output;
		}
	}

	public static void ClockDisplay(BitArray[] time, int[] arrayLengths, string chars = " 01") {
		/*
			reverse i-for to reverse up-down
			reverse j-for to reverse left-right
		*/

		for(int i = 3; i >= 0; i--) {
			for(int j = 0; j < 6; j++) {
				if(arrayLengths[j] >= i) {
					if(time[j][i] == true) Console.Write($"{chars[2]} ");
					else Console.Write($"{chars[1]} ");
				} else
					Console.Write($"{chars[0]} ");
			}
			Console.WriteLine();
		}
	}

	public static (BitArray[], int[]) TransformDate(DateTime now) {
		int[] arraysLengths = new int[6];
		BitArray[] now_BitArray = {
			ConvertToUnsignedBinary(now.Hour / 10, 2),
			ConvertToUnsignedBinary(now.Hour % 10, 4),
			ConvertToUnsignedBinary(now.Minute / 10, 3),
			ConvertToUnsignedBinary(now.Minute % 10, 4),
			ConvertToUnsignedBinary(now.Second / 10, 3),
			ConvertToUnsignedBinary(now.Second % 10, 4)
		};
		for(int i = 0; i < 6; i++) arraysLengths[i] = now_BitArray[i].Length - 1;

		//	DEBUG
		//for(int i = 0; i < 6; i++) {
		//	for(int j = arraysLengths[i]; j >=0; j--)
		//		Console.Write($"{now_BitArray[i][j]} ");
		//	Console.WriteLine();
		//}

		return (now_BitArray, arraysLengths);
	}

	public static BitArray ConvertToUnsignedBinary(int number, int length) {
		int k = 0;
		int initNumber = number;
		BitArray toR = new BitArray(length);

		while(number != 0) {
			if(number % 2 == 1) toR[k] = true;
			else toR[k] = false;
			number /= 2; k++;
		}

		//	DEBUG
		//Console.WriteLine($"k: {k}");
		//Console.WriteLine(number);
		//Console.WriteLine(initNumber);
		//foreach(bool i in toR) {
		//	Console.Write($"{i} ");
		//}
		//Console.WriteLine(" ");

		return toR;
	}
}