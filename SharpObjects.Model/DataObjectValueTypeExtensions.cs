using System;
using System.Runtime.CompilerServices;

namespace SharpObjects.Model
{
	internal static class DataObjectValueTypeExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Boolean HasFlagFast(this DataObjectValue.DataObjectValueType source, DataObjectValue.DataObjectValueType flag)
		{
			var sourceValue = (UInt64)source;
			var flagValue = (UInt64)flag;

			return (sourceValue & flagValue) == flagValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Boolean HasAnyOfFlags(this DataObjectValue.DataObjectValueType source, DataObjectValue.DataObjectValueType flags)
		{
			var sourceValue = (UInt64)source;
			var flagsValue = (UInt64)flags;

			return (sourceValue & flagsValue) != 0;
		}
	}
}