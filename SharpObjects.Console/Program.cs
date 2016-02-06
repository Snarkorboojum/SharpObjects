using Kappa.Core.System;
using System;
using System.Runtime.InteropServices;
using static System.Console;

namespace SharpObjects.Console
{
	public class Program
	{
		public static void Main(String[] args)
		{
			var dataObjectValue = new DataObjectValue();
			var sizeOfDataObjectValue = Marshal.SizeOf(dataObjectValue);
			WriteLine(sizeOfDataObjectValue);	

			ReadLine();
		}
	}
}