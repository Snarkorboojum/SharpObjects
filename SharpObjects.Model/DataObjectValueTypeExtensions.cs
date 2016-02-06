using System;
using System.Runtime.CompilerServices;

namespace SharpObjects.Model
{
	internal static class DataObjectValueTypeExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Boolean HasFlagFast(this DataObjectValue.DataObjectValueType source, DataObjectValue.DataObjectValueType flag)
		{
			var sourceValue = (UInt32)source;
			var flagValue = (UInt32)flag;

			return (sourceValue & flagValue) == flagValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Boolean HasAnyOfFlags(this DataObjectValue.DataObjectValueType source, DataObjectValue.DataObjectValueType flags)
		{
			var sourceValue = (UInt32)source;
			var flagsValue = (UInt32)flags;

			return (sourceValue & flagsValue) != 0;
		}
	}
}