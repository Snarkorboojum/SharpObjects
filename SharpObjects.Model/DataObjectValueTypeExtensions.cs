using System;
using System.Runtime.CompilerServices;

namespace Kappa.Core.System
{
	internal static class DataObjectValueTypeExtensions
	{
#if NETFX_45_AND_ABOVE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		internal static Boolean HasFlagFast(this DataObjectValue.DataObjectValueType source, DataObjectValue.DataObjectValueType flag)
		{
			var sourceValue = (UInt32)source;
			var flagValue = (UInt32)flag;

			return (sourceValue & flagValue) == flagValue;
		}

#if NETFX_45_AND_ABOVE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		internal static Boolean HasAnyOfFlags(this DataObjectValue.DataObjectValueType source, DataObjectValue.DataObjectValueType flags)
		{
			var sourceValue = (UInt32)source;
			var flagsValue = (UInt32)flags;

			return (sourceValue & flagsValue) != 0;
		}
	}
}