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
			Assert.AreEqual(12, sizeOfDataObjectValue); // 12 for x86 and 16 for x64
		}
	}
}