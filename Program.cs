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

	/// <summary>
	/// A self-contained test based on the wikipedia binary clock example
	/// </summary>
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

	/// <summary>
	/// Diplay the time with a binary clock
	/// </summary>
	/// <param name="time">The date in usual form (Y M D h m s)</param>
	/// <param name="chars">The chars to be used by the display function</param>
	/// <param name="dev">Switch for debugging and test running</param>
	/// <returns>The output from the display function</returns>
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

	/// <summary>
	/// Displays the time transformed by 'TransformDate' as a Binary Clock where (from left to right) are the digits and from bottom to top are the powers of 2 (from 0 to 3)
	/// </summary>
	/// <param name="time">The time to be diplayed as arrays from the transform function</param>
	/// <param name="arrayLengths">The lengths of the arrays from the transform function</param>
	/// <param name="chars">The chars to be used to display null, false and true values from arrays</param>
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

	/// <summary>
	/// Transforms the hour, minute and second of a date in a Array of binary digits stored as BitArrays
	/// </summary>
	/// <param name="now">The date to be transformed</param>
	/// <returns>A Tuple comprised of an Array of BitArrays and the Array of lengths of each BitArray</returns>
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

	/// <summary>
	/// Conversion from base(10) 32-bit integer to a base(2) unsigned {{length}} bits long - use only for clock module as bits order is reversed
	/// </summary>
	/// <param name="number">The integer we want to convert</param>
	/// <param name="length">The target length of the bit array</param>
	/// <returns>The BitArray coresponding to the reversed number in binary</returns>
	/// <exception cref="NotSupportedException">Number is to big, adjust length parameter</exception>
	public static BitArray ConvertToUnsignedBinary(int number, int length) {
		if(number < 0) throw new NotSupportedException("Number is negative, adjust number parameter");

		int k = 0;
		int initNumber = number;
		BitArray toR = new BitArray(length);

		while(number != 0 && k < length) {
			if(number % 2 == 1) toR[k] = true;
			else toR[k] = false;
			number /= 2; k++;
		}
		
		if(k >= length) throw new NotSupportedException("Number is to big, adjust length parameter");

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