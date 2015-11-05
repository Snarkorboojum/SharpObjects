using System;
using System.Runtime.CompilerServices;

namespace SharpObjects.Model
{
	internal static class DataObjectValueTypeExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Boolean HasFlagFast(this DataObjectValue.DataObjectValueType source, DataObjectValue.DataObjectValueType flag)
		{
			var sourceValue = (Byte)source;
			var flagValue = (Byte)flag;

			return (sourceValue & flagValue) == flagValue;
		}
	}
}