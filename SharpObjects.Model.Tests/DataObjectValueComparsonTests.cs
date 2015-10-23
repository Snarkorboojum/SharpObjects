using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpObjects.Model.Tests
{
	[TestClass]
	public sealed class DataObjectValueComparsonTests
	{
		#region Test Methods

		[TestMethod]
		public void BooleanComparisonTest()
		{
			AssertComparison(false, false);
			AssertComparison(false, true);
			AssertComparison(true, false);
			AssertComparison(true, true);
		}

		[TestMethod]
		public void BooleanWithIntComparisonTest()
		{
			AssertComparison(false, -5);
			AssertComparison(false, -1);
			AssertComparison(false, 0);
			AssertComparison(false, 1);
			AssertComparison(false, 5);

			AssertComparison(true, -5);
			AssertComparison(true, -1);
			AssertComparison(true, 0);
			AssertComparison(true, 1);
			AssertComparison(true, 5);
		}

		[TestMethod]
		public void BooleanWithSingleComparisonTest()
		{
			AssertComparison(false, -5.45f);
			AssertComparison(false, -1f);
			AssertComparison(false, 0f);
			AssertComparison(false, 0.1f);
			AssertComparison(false, 1f);
			AssertComparison(false, 1.1f);
			AssertComparison(false, 5.12f);

			AssertComparison(true, -5.45f);
			AssertComparison(true, -1f);
			AssertComparison(true, -0.1f);
			AssertComparison(true, 1f);
			AssertComparison(true, 5f);
		}

		[TestMethod]
		public void BooleanWithStringComparisonTest()
		{
			AssertComparison(false, null);
			AssertComparison(false, String.Empty);
			AssertComparison(false, " ");
			AssertComparison(false, "abc");
			AssertComparison(false, "True");
			AssertComparison(false, "true");
			AssertComparison(false, "TRUE");
			AssertComparison(false, "FALSE");
			AssertComparison(false, "False");
			AssertComparison(false, "fALse");
			AssertComparison(false, "fALse");

			AssertComparison(true, null);
			AssertComparison(true, String.Empty);
			AssertComparison(true, " ");
			AssertComparison(true, "abc");
			AssertComparison(true, "True");
			AssertComparison(true, "true");
			AssertComparison(true, "TRUE");
			AssertComparison(true, "FALSE");
			AssertComparison(true, "False");
			AssertComparison(true, "fALse");
			AssertComparison(true, "fALse");
		}

		[TestMethod]
		public void IntComparisonTest()
		{
			AssertComparison(0, 0);

			AssertComparison(0, 1);
			AssertComparison(1, 0);
			AssertComparison(1, 1);

			AssertComparison(0, -1);
			AssertComparison(-1, 0);
			AssertComparison(-1, -1);

			AssertComparison(+1, -1);
			AssertComparison(-1, +1);


			AssertComparison(5, 5);
			AssertComparison(5, 6);
			AssertComparison(6, 5);
			AssertComparison(2, -3);
			AssertComparison(17, 129);
			AssertComparison(-14, 12);
		}

		[TestMethod]
		public void IntWithSignleComparisonTest()
		{
			AssertComparison(0, 0f);

			AssertComparison(0, 1f);
			AssertComparison(0, -1f);

			AssertComparison(1, 0f);
			AssertComparison(-1, 0f);

			AssertComparison(1, 1f);
			AssertComparison(-1, -1f);

			AssertComparison(1, -1f);
			AssertComparison(-1, 1f);

			AssertComparison(5, 5f);
			AssertComparison(5, 5.45f);
			AssertComparison(6, 5.45f);
			AssertComparison(-5, 5.45f);
			AssertComparison(-6, 5.45f);
		}

		[TestMethod]
		public void IntWithStringComparisonTest()
		{
			AssertComparison(0, null);
			AssertComparison(1, null);
			AssertComparison(-1, null);

			AssertComparison(0, "");
			AssertComparison(1, "");
			AssertComparison(-1, "");

			AssertComparison(0, " ");
			AssertComparison(1, " ");
			AssertComparison(-1, " ");

			AssertComparison(0, "0");
			AssertComparison(1, "0");
			AssertComparison(-1, "0");
			AssertComparison(2, "0");
			AssertComparison(5, "0");
			AssertComparison(-3, "0");

			AssertComparison(15, "15");
			AssertComparison(15, "16");
			AssertComparison(16, "16");
			AssertComparison(16, "15");
			AssertComparison(16, "-15");
			AssertComparison(-15, "-15");
			AssertComparison(-14, "-15");
		}

		[TestMethod]
		public void SingleWithStringComparisonTest()
		{
			AssertComparison(0f, null);
			AssertComparison(1f, null);
			AssertComparison(-1f, null);

			AssertComparison(0f, "");
			AssertComparison(1f, "");
			AssertComparison(-1f, "");

			AssertComparison(0f, " ");
			AssertComparison(1f, " ");
			AssertComparison(-1f, " ");

			AssertComparison(0f, "0");
			AssertComparison(1f, "0");
			AssertComparison(-1f, "0");
			AssertComparison(2f, "0");
			AssertComparison(5f, "0");
			AssertComparison(-3f, "0");

			AssertComparison(5.35f, "5.35");
			AssertComparison(5.35f, "5.34");
			AssertComparison(5.35f, "5.36");

			AssertComparison(15f, "15");
			AssertComparison(15f, "16");
			AssertComparison(16f, "16");
			AssertComparison(16f, "15");
			AssertComparison(16f, "-15");
			AssertComparison(-15f, "-15");
			AssertComparison(-14f, "-15");
		}

		[TestMethod]
		public void StringComparisonTest()
		{
			AssertComparison(null, "");
			AssertComparison("", "");
			AssertComparison("", " ");

			AssertComparison("a", " a");
			AssertComparison("a", " a ");
			AssertComparison("a", "a ");
			AssertComparison("a", "ba ");
			AssertComparison("a", "ab");
			AssertComparison("a", "ab");
			AssertComparison("A", "A");
			AssertComparison("A", "A");
			AssertComparison("abc", "abc");
			AssertComparison("abc", "abcd");

			AssertComparison("0.0", null);
			AssertComparison("1.0", null);
			AssertComparison("-1", null);

			AssertComparison("0.0", "");
			AssertComparison("1.0", "");
			AssertComparison("-1", "");

			AssertComparison("0.0", " ");
			AssertComparison("1.0", " ");
			AssertComparison("-1", " ");

			AssertComparison("0.0", "0");
			AssertComparison("1.0", "0");
			AssertComparison("-1", "0");
			AssertComparison("2.0", "0");
			AssertComparison("5", "0");
			AssertComparison("-3", "0");

			AssertComparison("5.5", "5.35");
			AssertComparison("5.5", "5.34");
			AssertComparison("5.5", "5.36");

			AssertComparison("15", "15");
			AssertComparison("15", "16");
			AssertComparison("16", "16");
			AssertComparison("16", "15");
			AssertComparison("16", "-15");
			AssertComparison("-1.0", "-15");
			AssertComparison("-1.0", "-15");
		}

		#endregion

		#region Assertion Helpers

		private static void AssertComparison(Boolean value1, Boolean value2)
		{
			var expectedComparisonResult = value1.CompareTo(value2);
			AssertComparison(new DataObjectValue(value1), new DataObjectValue(value2), expectedComparisonResult);
		}

		private static void AssertComparison(Boolean value1, Int32 value2)
		{
			var expectedComparisonResult = value1
				? (value2 > 0 ? 0 : +1)
				: (value2 <= 0 ? 0 : -1);

			AssertComparison(new DataObjectValue(value1), new DataObjectValue(value2), expectedComparisonResult);
		}

		private static void AssertComparison(Boolean value1, Single value2)
		{
			var expectedComparisonResult = value1
				? (value2 > 0f ? 0 : +1)
				: (value2 <= 0f ? 0 : -1);

			AssertComparison(new DataObjectValue(value1), new DataObjectValue(value2), expectedComparisonResult);
		}

		private static void AssertComparison(Boolean value1, String value2)
		{
			var expectedComparisonResult = Compare(value1, value2);
			AssertComparison(new DataObjectValue(value1), new DataObjectValue(value2), expectedComparisonResult);
		}

		private static void AssertComparison(Int32 value1, Single value2)
		{
			AssertComparison((Single)value1, value2);
		}

		private static void AssertComparison(Int32 value1, String value2)
		{
			var expectedComparisonResult = Compare(value1, value2);
			AssertComparison(new DataObjectValue(value1), new DataObjectValue(value2), expectedComparisonResult);
		}

		private static void AssertComparison(Single value1, String value2)
		{
			var expectedComparisonResult = Compare(value1, value2);
			AssertComparison(new DataObjectValue(value1), new DataObjectValue(value2), expectedComparisonResult);
		}

		private static void AssertComparison(Int32 value1, Int32 value2)
		{
			var expectedComparisonResult = value1.CompareTo(value2);
			AssertComparison(new DataObjectValue(value1), new DataObjectValue(value2), expectedComparisonResult);
		}

		private static void AssertComparison(Single value1, Single value2)
		{
			var expectedComparisonResult = value1.CompareTo(value2);
			AssertComparison(new DataObjectValue(value1), new DataObjectValue(value2), expectedComparisonResult);
		}

		private static void AssertComparison(String value1, String value2)
		{
			var expectedComparisonResult = String.Compare(value1, value2, StringComparison.Ordinal);

			if (String.Compare(value1, Boolean.TrueString, StringComparison.OrdinalIgnoreCase) == 0)
				expectedComparisonResult = Compare(true, value2);

			if (String.Compare(value1, Boolean.FalseString, StringComparison.OrdinalIgnoreCase) == 0)
				expectedComparisonResult = Compare(false, value2);

			Int32 parsedIntegerValue;
			if (Int32.TryParse(value1, out parsedIntegerValue))
			{
				expectedComparisonResult = Compare(parsedIntegerValue, value2);
			}
			else
			{
				Single parsedSingleValue;
				if (Single.TryParse(value1, out parsedSingleValue))
					expectedComparisonResult = Compare(parsedSingleValue, value2);
			}

			if (expectedComparisonResult < 0)
				expectedComparisonResult = -1;

			if (expectedComparisonResult > 0)
				expectedComparisonResult = +1;

			AssertComparison(new DataObjectValue(value1), new DataObjectValue(value2), expectedComparisonResult);
		}

		private static void AssertComparison(DataObjectValue value1, DataObjectValue value2, Int32 expectedComparionResult)
		{
			var forwardComparison = value1.CompareTo(value2);
			Assert.IsTrue(forwardComparison == expectedComparionResult, $"Comparison result of '{value1.ToString()}' and '{value2.ToString()}' is not '{expectedComparionResult}'. Actual value is '{forwardComparison}'");

			var backwardComparison = value2.CompareTo(value1);
			var backwardExpectedResult = -1 * expectedComparionResult;
			Assert.IsTrue(backwardComparison == backwardExpectedResult, $"Comparison result of '{value2.ToString()}' and '{value1.ToString()}' is not '{backwardExpectedResult}'. Actual value is '{backwardComparison}'");
		}

		#endregion

		#region Comparioson methods

		private static Int32 Compare(Boolean value1, String value2)
		{
			Int32 expectedComparisonResult;
			if (String.IsNullOrEmpty(value2))
			{
				expectedComparisonResult = value1 ? +1 : 0;
			}
			else
			{
				var isTrueString = String.Compare(Boolean.TrueString, value2, StringComparison.OrdinalIgnoreCase) == 0;
				var isFalseString = String.Compare(Boolean.FalseString, value2, StringComparison.OrdinalIgnoreCase) == 0;

				if (isTrueString)
				{
					expectedComparisonResult = value1 ? 0 : -1;
				}
				else if (isFalseString)
				{
					expectedComparisonResult = value1 ? 1 : 0;
				}
				else
				{
					expectedComparisonResult = value1 ? 0 : -1;
				}
			}
			return expectedComparisonResult;
		}

		private static Int32 Compare(Int32 value1, String value2)
		{
			if (String.IsNullOrEmpty(value2))
				return +1;

			if (Boolean.TrueString.Equals(value2, StringComparison.OrdinalIgnoreCase))
				return value1 > 0 ? 0 : -1;

			if (Boolean.FalseString.Equals(value2, StringComparison.OrdinalIgnoreCase))
				return value1 > 0 ? +1 : 0;

			Int32 parsedIntegerValue;
			if (Int32.TryParse(value2, out parsedIntegerValue))
				return value1.CompareTo(parsedIntegerValue);

			Single parsedSingleValue;
			if (Single.TryParse(value2, out parsedSingleValue))
				return ((Single)value1).CompareTo(parsedSingleValue);

			var intString = value1.ToString();
			var comparison = String.CompareOrdinal(intString, value2);

			if (comparison > 0)
				return 1;

			if (comparison < 0)
				return -1;

			return 0;
		}

		private static Int32 Compare(Single value1, String value2)
		{
			if (String.IsNullOrEmpty(value2))
				return +1;

			if (Boolean.TrueString.Equals(value2, StringComparison.OrdinalIgnoreCase))
				return value1 > 0 ? 0 : -1;

			if (Boolean.FalseString.Equals(value2, StringComparison.OrdinalIgnoreCase))
				return value1 > 0 ? +1 : 0;

			Single parsedSingleValue;
			if (Single.TryParse(value2, out parsedSingleValue))
				return value1.CompareTo(parsedSingleValue);

			var intString = value1.ToString(CultureInfo.InvariantCulture);
			var comparison = String.CompareOrdinal(intString, value2);

			if (comparison > 0)
				return 1;

			if (comparison < 0)
				return -1;

			return 0;
		}

		#endregion
	}
}