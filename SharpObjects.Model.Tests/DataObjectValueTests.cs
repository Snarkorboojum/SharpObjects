using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;

namespace SharpObjects.Model.Tests
{
	[TestClass]
	public class DataObjectValueTests
	{
		[TestMethod]
		public void SizeTest()
		{
			var dataObjectValue = new DataObjectValue();
			var sizeOfDataObjectValue = Marshal.SizeOf(dataObjectValue);

			Assert.AreEqual(sizeof(UInt32) + sizeof(Double) + IntPtr.Size, sizeOfDataObjectValue);
		}
	}
}