using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kappa.Core.System;

namespace SharpObjects.Model.Tests
{
	[TestClass]
	public sealed class DataObjectValueArithmeticsTests
	{
		#region Same types

		[TestMethod]
		public void EmptyValuesAddTest()
		{
			AssertAdd(default(DataObjectValue), default(DataObjectValue), default(DataObjectValue));
			AssertAdd(default(DataObjectValue), default(DataObjectValue), new DataObjectValue());

			AssertAdd(default(DataObjectValue), new DataObjectValue(), default(DataObjectValue));
			AssertAdd(default(DataObjectValue), new DataObjectValue(), new DataObjectValue());

			AssertAdd(new DataObjectValue(), new DataObjectValue(), default(DataObjectValue));
			AssertAdd(new DataObjectValue(), new DataObjectValue(), new DataObjectValue());
		}

		[TestMethod]
		public void BooleanValuesAddTest()
		{
			AssertAdd(true, true, true);
			AssertAdd(true, true, 1);
			AssertAdd(true, true, 1f);
			AssertAdd(true, true, "1");

			AssertAdd(false, false, false);
			AssertAdd(false, false, 0);
			AssertAdd(false, false, 0f);
			AssertAdd(false, false, "0");

			AssertAdd(true, false, true);
			AssertAdd(true, false, 1);
			AssertAdd(true, false, 1f);
			AssertAdd(true, false, "1");

			AssertAdd(false, true, true);
			AssertAdd(false, true, 1);
			AssertAdd(false, true, 1f);
			AssertAdd(false, true, "1");
		}

		[TestMethod]
		public void IntValuesAddTest()
		{
			AssertAdd(0, 0, DataObjectValue.Zero);
			AssertAdd(5, 5, 10);
			AssertAdd(1, 2, 3);
			AssertAdd(2, 1, 3);
			AssertAdd(new DataObjectValue(-5), 5, DataObjectValue.Zero);
		}

		[TestMethod]
		public void SingleValuesAddTest()
		{
			AssertAdd(0f, 0f, 0f);
			AssertAdd(5f, 5f, 10f);
			AssertAdd(1f, 2f, 3f);
			AssertAdd(2f, 1f, 3f);
			AssertAdd(new DataObjectValue(-5), 5, 0f);
			AssertAdd(new DataObjectValue(-5), 5, DataObjectValue.Zero);
		}

		[TestMethod]
		public void StringValuesAddTest()
		{
			AssertAdd("", "", DataObjectValue.Zero);

			// ReSharper disable once RedundantToStringCall
			AssertAdd("", new DataObjectValue(String.Empty.ToString()), DataObjectValue.Zero);;
			AssertAdd("abc", "ABC", "abcABC");
			AssertAdd("abc", "abc", "abcabc");
		}

		#endregion

		#region Cross typess

		[TestMethod]
		public void BooleanAndStringValuesAddTest()
		{
			AssertAdd(true, "True", true);
			AssertAdd(true, "TRUE", true);
			AssertAdd(true, "true", true);
			AssertAdd(true, "tRuE", true);
			AssertAdd(true, "1", true);
			AssertAdd(true, "1.0", true);
			AssertAdd(true, "5", "5");
			AssertAdd(true, "0.2", "0.2");
			AssertAdd(true, new DataObjectValue("-1"), new DataObjectValue("-1"));

			AssertAdd(true, "abc", "abc");
			AssertAdd(false, "abc", "abc");

			AssertAdd(false, "False", false);
			AssertAdd(false, "FALSE", false);
			AssertAdd(false, "false", false);
			AssertAdd(false, "fAlSe", false);
			AssertAdd(false, "0", false);
			AssertAdd(false, "0.0", false);
			AssertAdd(false, new DataObjectValue("-1.0"), false);
		}

		[TestMethod]
		public void IntAndBooleanValuesTest()
		{
			AssertAdd(1, true, 1);
			AssertAdd(5, true, 5);
			AssertAdd(0, false, 0);

			AssertAdd(3, false, 3);
		}

		[TestMethod]
		public void IntAndSingleValuesTest()
		{
			AssertAdd(0, 0f, DataObjectValue.Zero);
			AssertAdd(5, 5f, 10.0f);
			AssertAdd(new DataObjectValue(-5), new DataObjectValue(5f), DataObjectValue.Zero);

			AssertAdd(5, 5.01f, 10.01f);
		}

		[TestMethod]
		public void IntAndStringValuesTest()
		{
			AssertAdd(0, "0", DataObjectValue.Zero);
			AssertAdd(5, "5", 10);

			// ReSharper disable RedundantCast
			AssertAdd(0, new DataObjectValue((String)null), DataObjectValue.Zero);
			AssertAdd(0, String.Empty, DataObjectValue.Zero);
			// ReSharper restore RedundantCast

			AssertAdd(6, "5", 11);
			AssertAdd(6, "6asfasdf", "66asfasdf");

			AssertAdd(6, "abc", "6abc");
			AssertAdd("abc", 6, "abc6");
		}

		[TestMethod]
		public void SingleAndBooleanValuesAddTest()
		{
			AssertAdd(0f, false, 0f);
			AssertAdd(0f, false, false);
			AssertAdd(1.0f, true, true);
			AssertAdd(1.0f, true, 1.0f);

			AssertAdd(5f, true, 5f);
			AssertAdd(3f, false, 3f);
		}

		[TestMethod]
		public void SingleAndStringValuesAddTest()
		{
			AssertAdd(0f, "0", DataObjectValue.Zero);
			AssertAdd(0f, "0", DataObjectValue.Zero);
			AssertAdd(0f, "0.0", DataObjectValue.Zero);
			AssertAdd(4f, "6", 10);
			AssertAdd(4f, "6", 10f);
			AssertAdd(4f, "6", "10.0");
			AssertAdd(5.45f, "3.25", 8.7f);

			// ReSharper disable RedundantCast
			AssertAdd(0f, new DataObjectValue((String)null), DataObjectValue.Zero);
			AssertAdd(0.0f, String.Empty, DataObjectValue.Zero);
			// ReSharper restore RedundantCast
		}

		#endregion
		
		private static void AssertAdd(DataObjectValue a, DataObjectValue b, DataObjectValue expected)
		{
			var add1 = a.Add(b);
			//var add2 = b.Add(a);
			Assert.IsTrue(add1 == expected, $"'{a.ToString()}'.Add('{b.ToString()}') returns '{add1.ToString()}' | expected: '{expected.ToString()}'");
			//Assert.IsTrue(add2 == expected, $"'{b.ToString()}'.Add('{a.ToString()}') returns '{add2.ToString()}' | expected: '{expected.ToString()}'");

			var sum1 = a + b;
			//var sum2 = b + a;
			Assert.IsTrue(sum1 == expected, $"'{a.ToString()}'+'{b.ToString()}'='{sum1.ToString()}' | expected: '{expected.ToString()}'");
			//Assert.IsTrue(sum2 == expected, $"'{b.ToString()}'+'{a.ToString()}'='{sum2.ToString()}' | expected: '{expected.ToString()}'");
		}
	}
}