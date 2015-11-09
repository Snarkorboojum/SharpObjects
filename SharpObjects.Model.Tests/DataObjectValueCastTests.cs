using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpObjects.Model.Tests
{
	[TestClass]
	public sealed class DataObjectValueCastTests
	{
		[TestMethod]
		public void DefalutValuesCastTest()
		{
			var testedValue1 = default(DataObjectValue);
			AssertCast(testedValue1, false, 0, 0f, null);

			var testedValue2 = new DataObjectValue();
			AssertCast(testedValue2, false, 0, 0f, null);
		}

		[TestMethod]
		public void BooleanValuesCastTest()
		{
			var testedTrueValue = new DataObjectValue(true);
			AssertCast(testedTrueValue, true, 1, 1.0f, "True");

			var testedFalseValue = new DataObjectValue(false);
			AssertCast(testedFalseValue, false, 0, 0.0f, "False");
		}

		[TestMethod]
		public void IntValuesCastTest()
		{
			var testedValue1 = DataObjectValue.Zero;
			AssertCast(testedValue1, false, 0, 0.0f, "0");

			var testedValue2 = new DataObjectValue(-1);
			AssertCast(testedValue2, false, -1, -1.0f, "-1");

			var testedValue3 = new DataObjectValue(1);
			AssertCast(testedValue3, true, 1, 1f, "1");

			var testedValue4 = new DataObjectValue(5);
			AssertCast(testedValue4, true, 5, 5f, "5");
		}

		[TestMethod]
		public void SingleValuesCastTest()
		{
			var testedValue1 = new DataObjectValue(0f);
			AssertCast(testedValue1, false, 0, 0.0f, "0");

			var testedValue2 = new DataObjectValue(-1f);
			AssertCast(testedValue2, false, -1, -1.0f, "-1");

			var testedValue3 = new DataObjectValue(1f);
			AssertCast(testedValue3, true, 1, 1f, "1");

			var testedValue4 = new DataObjectValue(5f);
			AssertCast(testedValue4, true, 5, 5f, "5");

			var testedValue5 = new DataObjectValue(0.1f);
			AssertCast(testedValue5, false, 0, 0.1f, "0.1");

			var testedValue6 = new DataObjectValue(0.6f);
			AssertCast(testedValue6, false, 0, 0.6f, "0.6");

			var testedValue7 = new DataObjectValue(4.6f);
			AssertCast(testedValue7, true, 4, 4.6f, "4.6");
		}

		[SuppressMessage("ReSharper", "RedundantCast")]
		[TestMethod]
		public void StringValuesCastTest()
		{
			AssertCast(new DataObjectValue((String)null), false, 0, 0.0f, null);
			AssertCast(new DataObjectValue(String.Empty), false, 0, 0.0f, "");
			AssertCast(new DataObjectValue("abc"), true, 3, 3.0f, "abc");
			AssertCast(new DataObjectValue("0"), false, 0, 0.0f, "0");
			AssertCast(new DataObjectValue("1"), true, 1, 1.0f, "1");
			AssertCast(new DataObjectValue("-1"), false, -1, -1.0f, "-1");
			AssertCast(new DataObjectValue("2.34"), true, 2, 2.34f, "2.34");
			AssertCast(new DataObjectValue("4.721"), true, 4, 4.721f, "4.721");
			AssertCast(new DataObjectValue("-4.721"), false, -4, -4.721f, "-4.721");
			AssertCast(new DataObjectValue("True"), true, 1, 1.0f, "True");
			AssertCast(new DataObjectValue("FALSE"), false, 0, 0.0f, "FALSE");
			AssertCast(new DataObjectValue("faLSe"), false, 0, 0.0f, "faLSe");
		}

		#region Assertion Methods

		private void AssertCast(DataObjectValue value, Boolean expectedValue)
		{
			var testedValue = (Boolean)value;
			Assert.IsTrue(testedValue == expectedValue, $"Value '{value.ToString()}' not equals to expected {nameof(Boolean)} value '{expectedValue}'");
		}

		private void AssertCast(DataObjectValue value, Int32 expectedValue)
		{
			var testedValue = (Int32)value;
			Assert.IsTrue(testedValue == expectedValue, $"Value '{value.ToString()}' not equals to expected {nameof(Int32)} value '{expectedValue}'");
		}

		[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
		private void AssertCast(DataObjectValue value, Single expectedValue)
		{
			var testedValue = (Single)value;
			Assert.IsTrue(testedValue == expectedValue, $"Value '{value.ToString()}' not equals to expected {nameof(Single)} value '{expectedValue}'");
		}

		private void AssertCast(DataObjectValue value, String expectedValue)
		{
			var testedValue = (String)value;
			//var toStringValue = value.ToString();
			//Assert.IsTrue(testedValue == toStringValue, $"Different result of '{nameof(String.ToString)}' method result and explict cast to '{nameof(String)}'");
			Assert.IsTrue(testedValue == expectedValue, $"Value '{value.ToString()}' not equals to expected {nameof(String)} value '{expectedValue}'");
		}

		private void AssertCast(DataObjectValue value, Boolean expectedBooleanValue, Int32 expectedIntegerValue, Single expectedFloatValue, String expectedStringValue)
		{
			AssertExpectedValueConsistenscy(expectedBooleanValue, expectedIntegerValue, expectedFloatValue, expectedStringValue);

			AssertCast(value, expectedBooleanValue);
			AssertCast(value, expectedIntegerValue);
			AssertCast(value, expectedFloatValue);
			AssertCast(value, expectedStringValue);
		}

		private void AssertExpectedValueConsistenscy(DataObjectValue expectedBooleanValue, DataObjectValue expectedIntegerValue, DataObjectValue expectedFloatValue, DataObjectValue expectedStringValue)
		{
			DataObjectValueEqualityTests.AssertAreEqual(expectedBooleanValue, expectedIntegerValue, skipHashCheck: true);
			DataObjectValueEqualityTests.AssertAreEqual(expectedBooleanValue, expectedFloatValue, skipHashCheck: true);
			DataObjectValueEqualityTests.AssertAreEqual(expectedBooleanValue, expectedStringValue, skipHashCheck: true);

			DataObjectValueEqualityTests.AssertAreEqual(expectedIntegerValue, expectedBooleanValue, skipHashCheck: true);
			//DataObjectValueEqualityTests.AssertAreEqual(expectedIntegerValue, expectedFloatValue, skipHashCheck: true);
			//DataObjectValueEqualityTests.AssertAreEqual(expectedIntegerValue, expectedStringValue, skipHashCheck: true);

			DataObjectValueEqualityTests.AssertAreEqual(expectedFloatValue, expectedBooleanValue, skipHashCheck: true);
			//DataObjectValueEqualityTests.AssertAreEqual(expectedFloatValue, expectedIntegerValue, skipHashCheck: true);
			//DataObjectValueEqualityTests.AssertAreEqual(expectedFloatValue, expectedStringValue, skipHashCheck: true);

			DataObjectValueEqualityTests.AssertAreEqual(expectedStringValue, expectedBooleanValue, skipHashCheck: true);
			//DataObjectValueEqualityTests.AssertAreEqual(expectedStringValue, expectedIntegerValue, skipHashCheck: true);
			//DataObjectValueEqualityTests.AssertAreEqual(expectedStringValue, expectedFloatValue, skipHashCheck: true);
		}

		#endregion
	}
}