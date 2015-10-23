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
		public void EmptyValuesTest()
		{
			AssertAreEqual(default(DataObjectValue), default(DataObjectValue));
			AssertAreEqual(default(DataObjectValue), new DataObjectValue());
			AssertAreEqual(new DataObjectValue(), new DataObjectValue());
		}

		[TestMethod]
		public void BooleanValuesTest()
		{
			AssertAreEqual(new DataObjectValue(true), new DataObjectValue(true));
			AssertAreEqual(new DataObjectValue(false), new DataObjectValue(false));

			AssertAreNotEqual(new DataObjectValue(true), new DataObjectValue(false));
		}

		[TestMethod]
		public void IntValuesTest()
		{
			AssertAreEqual(new DataObjectValue(0), new DataObjectValue(0));
			AssertAreEqual(new DataObjectValue(5), new DataObjectValue(5));
		}

		[TestMethod]
		public void SingleValuesTest()
		{
			AssertAreEqual(new DataObjectValue(0f), new DataObjectValue(0f));
			AssertAreEqual(new DataObjectValue(5f), new DataObjectValue(5f));
			AssertAreEqual(new DataObjectValue(5.45f), new DataObjectValue(5.45f));
			AssertAreNotEqual(new DataObjectValue(5.45f), new DataObjectValue(5.4f));
		}

		[TestMethod]
		public void StringValuesTest()
		{
			AssertAreEqual(new DataObjectValue(""), new DataObjectValue(""));

			// ReSharper disable once RedundantToStringCall
			AssertAreEqual(new DataObjectValue(""), new DataObjectValue(String.Empty.ToString()));
			AssertAreNotEqual(new DataObjectValue("abc"), new DataObjectValue("ABC"));
			AssertAreEqual(new DataObjectValue("abc"), new DataObjectValue("abc"));
		}

		#endregion

		#region Cross typess

		[TestMethod]
		public void BooleanAndStringValuesTest()
		{
			AssertAreEqual(new DataObjectValue(true), new DataObjectValue("True"));
			AssertAreEqual(new DataObjectValue(true), new DataObjectValue("TRUE"));
			AssertAreEqual(new DataObjectValue(true), new DataObjectValue("true"));
			AssertAreEqual(new DataObjectValue(true), new DataObjectValue("tRuE"));
			AssertAreEqual(new DataObjectValue(true), new DataObjectValue("1"));
			AssertAreEqual(new DataObjectValue(true), new DataObjectValue("1.0"));
			AssertAreEqual(new DataObjectValue(true), new DataObjectValue("5"), skipHashCheck: true);
			AssertAreEqual(new DataObjectValue(true), new DataObjectValue("0.2"), skipHashCheck: true);

			AssertAreEqual(new DataObjectValue(false), new DataObjectValue("False"));
			AssertAreEqual(new DataObjectValue(false), new DataObjectValue("FALSE"));
			AssertAreEqual(new DataObjectValue(false), new DataObjectValue("false"));
			AssertAreEqual(new DataObjectValue(false), new DataObjectValue("fAlSe"));
			AssertAreEqual(new DataObjectValue(false), new DataObjectValue("0"));
			AssertAreEqual(new DataObjectValue(false), new DataObjectValue("0.0"));
			AssertAreEqual(new DataObjectValue(false), new DataObjectValue("-1.0"), skipHashCheck: true);
		}

		public void IntAndBooleanValuesTest()
		{
			AssertAreEqual(new DataObjectValue(1), new DataObjectValue(true));
			AssertAreEqual(new DataObjectValue(5), new DataObjectValue(true), skipHashCheck: true);
			AssertAreEqual(new DataObjectValue(0), new DataObjectValue(false));

			AssertAreNotEqual(new DataObjectValue(3), new DataObjectValue(false));
		}

		[TestMethod]
		public void IntAndSingleValuesTest()
		{
			AssertAreEqual(new DataObjectValue(0), new DataObjectValue(0f));
			AssertAreEqual(new DataObjectValue(5), new DataObjectValue(5f));
			AssertAreEqual(new DataObjectValue(-5), new DataObjectValue(-5f));

			AssertAreNotEqual(new DataObjectValue(5), new DataObjectValue(5.001f));
		}

		[TestMethod]
		public void IntAndStringValuesTest()
		{
			AssertAreEqual(new DataObjectValue(0), new DataObjectValue("0"));
			AssertAreEqual(new DataObjectValue(5), new DataObjectValue("5"));

			// ReSharper disable RedundantCast
			AssertAreNotEqual(new DataObjectValue(0), new DataObjectValue((String)null), skipHashCheck: true);
			AssertAreNotEqual(new DataObjectValue(0), new DataObjectValue(String.Empty), skipHashCheck: true);
			// ReSharper restore RedundantCast

			AssertAreNotEqual(new DataObjectValue(6), new DataObjectValue("5"));
			AssertAreNotEqual(new DataObjectValue(6), new DataObjectValue("6asfasdf"));
		}

		public void SingleAndBooleanValuesTest()
		{
			AssertAreEqual(new DataObjectValue(0f), new DataObjectValue(false));
			AssertAreEqual(new DataObjectValue(1.0f), new DataObjectValue(true));

			AssertAreEqual(new DataObjectValue(5f), new DataObjectValue(true), skipHashCheck: true);
			AssertAreNotEqual(new DataObjectValue(3f), new DataObjectValue(false));
		}

		[TestMethod]
		public void SingleAndStringValuesTest()
		{
			AssertAreEqual(new DataObjectValue(0f), new DataObjectValue("0"));
			AssertAreEqual(new DataObjectValue(0f), new DataObjectValue("0.0"));
			AssertAreEqual(new DataObjectValue(4f), new DataObjectValue("4"));
			AssertAreEqual(new DataObjectValue(4f), new DataObjectValue("4.0"));
			AssertAreEqual(new DataObjectValue(5.45f), new DataObjectValue("5.45"));

			AssertAreNotEqual(new DataObjectValue(5.46f), new DataObjectValue("5.45"));
			AssertAreNotEqual(new DataObjectValue(5.46f), new DataObjectValue("5"));
			AssertAreNotEqual(new DataObjectValue(5.46f), new DataObjectValue("6"));

			// ReSharper disable RedundantCast
			AssertAreNotEqual(new DataObjectValue(0f), new DataObjectValue((String)null), skipHashCheck: true);
			AssertAreNotEqual(new DataObjectValue(0.0f), new DataObjectValue(String.Empty), skipHashCheck: true);
			// ReSharper restore RedundantCast
		}

		#endregion

		#region Assertion methods

		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		private static void AssertAreEqual(DataObjectValue value1, DataObjectValue value2, Boolean skipHashCheck = false)
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


			// DataObjectValue.Equsls(DataObjectValue) check
			var areEqualForward = value1.Equals(value2);
			var areEqualBackward = value2.Equals(value1);

			Assert.IsTrue(areEqualForward, $"Value '{value1.ToString()}'(1) not equals to value '{value2.ToString()}'(2)");
			Assert.IsTrue(areEqualBackward, $"Value '{value2.ToString()}'(2) not equals to value '{value1.ToString()}'(1)");

			// DataDataObjectValue.CompareTo(DataObjectValue) check
			var areEqualComparedForward = value1.CompareTo(value2);
			var areEqualComparedBackward = value2.CompareTo(value1);

			Assert.IsTrue(areEqualComparedForward == 0, $"Value '{value1.ToString()}'(1) compared with value '{value2.ToString()}'(2) is '{areEqualComparedForward}'");
			Assert.IsTrue(areEqualComparedBackward == 0, $"Value '{value2.ToString()}'(2) compared with value '{value1.ToString()}'(1) is '{areEqualComparedBackward}'");

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

			// DataObjectValue.Equsls(DataObjectValue) check
			var areEqualForward = value1.Equals(value2);
			var areEqualBackward = value2.Equals(value1);

			Assert.IsFalse(areEqualForward, $"Value '{value1.ToString()}'(1) equals to value '{value2.ToString()}'(2)");
			Assert.IsFalse(areEqualBackward, $"Value '{value2.ToString()}'(2) equals to value '{value1.ToString()}'(1)");

			// DataDataObjectValue.CompareTo(DataObjectValue) check
			var areEqualComparedForward = value1.CompareTo(value2);
			var areEqualComparedBackward = value2.CompareTo(value1);

			Assert.IsTrue(areEqualComparedForward != 0, $"Value '{value1.ToString()}'(1) compared with value '{value2.ToString()}'(2) is '{areEqualComparedForward}'");
			Assert.IsTrue(areEqualComparedBackward != 0, $"Value '{value2.ToString()}'(2) compared with value '{value1.ToString()}'(1) is '{areEqualComparedBackward}'");

			// Object.Equals(Object) override check
			var areEqualToObjectForward = value1.Equals((Object)value2);
			var areEqialToObjectBackward = value2.Equals((Object)value1);

			Assert.IsFalse(areEqualToObjectForward, $"Value '{value1.ToString()}'(1) equals to object '{value2.ToString()}'(2)");
			Assert.IsFalse(areEqialToObjectBackward, $"Value '{value2.ToString()}'(2) equals to object '{value1.ToString()}'(1)");
		}

		#endregion
	}
}