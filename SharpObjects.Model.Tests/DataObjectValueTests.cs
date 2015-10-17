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
			var x = new DataObjectValue();
			var y = Marshal.SizeOf(x);
			Assert.AreEqual(8, y);
		}
	}
}
