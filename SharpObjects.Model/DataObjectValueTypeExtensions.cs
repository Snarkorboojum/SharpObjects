using System;
using System.Runtime.CompilerServices;

namespace SharpObjects.Model
{
	internal static class DataObjectValueTypeExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean HasFlag(this DataObjectValue.DataObjectValueType source, DataObjectValue.DataObjectValueType flag)
		{
			var sourceValue = (Byte)source;
			var flagValue = (Byte)flag;

			return (sourceValue & flagValue) == flagValue;
		}
	}
}