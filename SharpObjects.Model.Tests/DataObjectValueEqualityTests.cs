using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpObjects.Model.Tests
{
	[TestClass]
	public sealed class DataObjectValueEqualityTests
	{
		#region Same types

		[TestMethod]
		public void EmptyValuesEqualityTest()
		{
			AssertAreEqual(default(DataObjectValue), default(DataObjectValue));
			AssertAreEqual(default(DataObjectValue), new DataObjectValue());
			AssertAreEqual(default(DataObjectValue), DataObjectValue.Nothing);

			AssertAreEqual(new DataObjectValue(), new DataObjectValue());
			AssertAreEqual(new DataObjectValue(), DataObjectValue.Nothing);
		}

		[TestMethod]
		public void ZeroValuesEqualityTest()
		{
			AssertAreEqual(false, DataObjectValue.Zero);
			AssertAreEqual(0, DataObjectValue.Zero);
			AssertAreEqual(0f, DataObjectValue.Zero);
			AssertAreEqual("0", DataObjectValue.Zero);
			AssertAreEqual("0.0", DataObjectValue.Zero);
		}

		[TestMethod]
		public void BooleanValuesEqualityTest()
		{
			AssertAreEqual(true, true);
			AssertAreEqual(false, false);

			AssertAreNotEqual(true, false);
		}

		[TestMethod]
		public void IntValuesEqualityTest()
		{
			AssertAreEqual(DataObjectValue.Zero, DataObjectValue.Zero);
			AssertAreEqual(5, 5);
		}

		[TestMethod]
		public void SingleValuesEqualityTest()
		{
			AssertAreEqual(0f, 0f);
			AssertAreEqual(5f, 5f);
			AssertAreEqual(5.45f, 5.45f);
			AssertAreNotEqual(5.45f, 5.4f);
		}

		[TestMethod]
		public void StringValuesEqualityTest()
		{
			AssertAreEqual("", "");
			AssertAreEqual("", String.Empty);
			AssertAreNotEqual("abc", "ABC");
			AssertAreEqual("abc", "abc");

			AssertAreEqual("True", "abc", skipHashCheck: true);
			AssertAreEqual("False", "", skipHashCheck: true);
			AssertAreEqual("False", null, skipHashCheck: true);
		}

		#endregion

		#region Cross typess

		[TestMethod]
		public void BooleanAndStringValuesEqualityTest()
		{
			AssertAreEqual(true, "True");
			AssertAreEqual(true, "TRUE");
			AssertAreEqual(true, "true");
			AssertAreEqual(true, "tRuE");
			AssertAreEqual(true, "1");
			AssertAreEqual(true, "1.0");
			AssertAreEqual(true, "5", skipHashCheck: true);
			AssertAreEqual(true, "abc", skipHashCheck: true);

			AssertAreEqual(false, "0.2", skipHashCheck: true);
			AssertAreEqual(false, (String)null);
			AssertAreEqual(false, "");
			AssertAreEqual(false, "False");
			AssertAreEqual(false, "FALSE");
			AssertAreEqual(false, "false");
			AssertAreEqual(false, "fAlSe");
			AssertAreEqual(false, "0");
			AssertAreEqual(false, "0.0");
			AssertAreEqual(false, new DataObjectValue("-1.0"), skipHashCheck: true);
		}

		[TestMethod]
		public void IntAndBooleanValuesEqualityTest()
		{
			AssertAreEqual(1, true);
			AssertAreEqual(5, true, skipHashCheck: true);
			AssertAreEqual(DataObjectValue.Zero, false);

			AssertAreNotEqual(3, false);
		}

		[TestMethod]
		public void IntAndSingleValuesEqualityTest()
		{
			AssertAreEqual(DataObjectValue.Zero, 0f);
			AssertAreEqual(5, 5f);
			AssertAreEqual(new DataObjectValue(-5), new DataObjectValue(-5f));

			AssertAreNotEqual(5, 5.001f);
		}

		[TestMethod]
		public void IntAndStringValuesEqualityTest()
		{
			AssertAreEqual(DataObjectValue.Zero, "0");
			AssertAreEqual(5, "5");

			// ReSharper disable RedundantCast
			AssertAreEqual(DataObjectValue.Zero, new DataObjectValue((String)null), skipHashCheck: true);
			AssertAreEqual(DataObjectValue.Zero, String.Empty, skipHashCheck: true);
			// ReSharper restore RedundantCast

			AssertAreNotEqual(6, "5");
			AssertAreNotEqual(6, "6asfasdf");
		}

		[TestMethod]
		public void SingleAndBooleanValuesEqualityTest()
		{
			AssertAreEqual(0f, false);
			AssertAreEqual(1.0f, true);

			AssertAreEqual(5f, true, skipHashCheck: true);
			AssertAreNotEqual(3f, false);
		}

		[TestMethod]
		public void SingleAndStringValuesEqualityTest()
		{
			AssertAreEqual(0f, null);
			AssertAreEqual(0f, "");
			AssertAreEqual(0f, "0");
			AssertAreEqual(0f, "0.0");
			AssertAreEqual(4f, "4");
			AssertAreEqual(4f, "4.0");
			AssertAreEqual(5.45f, "5.45");

			AssertAreNotEqual(5.46f, "5.45");
			AssertAreNotEqual(5.46f, "5");
			AssertAreNotEqual(5.46f, "6");


			AssertAreNotEqual(5.46f, "abcde", skipHashCheck: true);
			AssertAreNotEqual(5.46f, "abcdef", skipHashCheck: true);
			AssertAreNotEqual(5.88f, "abcdef", skipHashCheck: true);

			// ReSharper disable RedundantCast
			AssertAreEqual(0f, new DataObjectValue((String)null), skipHashCheck: true);
			AssertAreEqual(0.0f, String.Empty, skipHashCheck: true);
			// ReSharper restore RedundantCast
		}

		#endregion

		#region Assertion methods

		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		internal static void AssertAreEqual(DataObjectValue value1, DataObjectValue value2, Boolean skipHashCheck = false)
		{
			// Hash check
			var value1Hash = value1.GetHashCode();
			var value2Hash = value2.GetHashCode();

			if (!skipHashCheck)
				Assert.IsTrue(value1Hash == value2Hash, $"Different hash codes for '{value1.ToString()}' and '{value2.ToString()}'");

			// Equality operator check
			var areEqualOperatorForward = value1 == value2;
			var areEqualOperatorBackward = value2 == value1;

			Assert.IsTrue(areEqualOperatorForward, $"Value '{value1.ToString()}'(1) not equals to value '{value2.ToString()}'(2)");
			Assert.IsTrue(areEqualOperatorBackward, $"Value '{value2.ToString()}'(2) not equals to value '{value1.ToString()}'(1)");


			// DataObjectValue.Equals(DataObjectValue) check
			var areEqualForward = value1.Equals(value2);
			var areEqualBackward = value2.Equals(value1);

			Assert.IsTrue(areEqualForward, $"Value '{value1.ToString()}'(1) not equals to value '{value2.ToString()}'(2)");
			Assert.IsTrue(areEqualBackward, $"Value '{value2.ToString()}'(2) not equals to value '{value1.ToString()}'(1)");
			
			// Object.Equals(Object) override check
			var areEqualToObjectForward = value1.Equals((Object)value2);
			var areEqialToObjectBackward = value2.Equals((Object)value1);

			Assert.IsTrue(areEqualToObjectForward, $"Value '{value1.ToString()}'(1) not equals to object '{value2.ToString()}'(2)");
			Assert.IsTrue(areEqialToObjectBackward, $"Value '{value2.ToString()}'(2) not equals to object '{value1.ToString()}'(1)");
		}

		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		private static void AssertAreNotEqual(DataObjectValue value1, DataObjectValue value2, Boolean skipHashCheck = false)
		{
			// Hash check
			var value1Hash = value1.GetHashCode();
			var value2Hash = value2.GetHashCode();

			if (!skipHashCheck)
				Assert.IsTrue(value1Hash != value2Hash, $"Same hash codes for '{value1.ToString()}' and '{value2.ToString()}'");

			// Equality operator check
			var areEqualOperatorForward = value1 != value2;
			var areEqualOperatorBackward = value2 != value1;

			Assert.IsTrue(areEqualOperatorForward, $"Value '{value1.ToString()}'(1) equals to value '{value2.ToString()}'(2)");
			Assert.IsTrue(areEqualOperatorBackward, $"Value '{value2.ToString()}'(2) equals to value '{value1.ToString()}'(1)");

			// DataObjectValue.Equals(DataObjectValue) check
			var areEqualForward = value1.Equals(value2);
			var areEqualBackward = value2.Equals(value1);

			Assert.IsFalse(areEqualForward, $"Value '{value1.ToString()}'(1) equals to value '{value2.ToString()}'(2)");
			Assert.IsFalse(areEqualBackward, $"Value '{value2.ToString()}'(2) equals to value '{value1.ToString()}'(1)");
			
			// Object.Equals(Object) override check
			var areEqualToObjectForward = value1.Equals((Object)value2);
			var areEqialToObjectBackward = value2.Equals((Object)value1);

			Assert.IsFalse(areEqualToObjectForward, $"Value '{value1.ToString()}'(1) equals to object '{value2.ToString()}'(2)");
			Assert.IsFalse(areEqialToObjectBackward, $"Value '{value2.ToString()}'(2) equals to object '{value1.ToString()}'(1)");
		}

		#endregion
	}
}