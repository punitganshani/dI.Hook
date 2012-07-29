using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dIHook.Utilities
{
    internal static class ArgumentHelper
    {
        internal static void ValidateNotNull<T>(T value, string propertyName)
        {
            if (value == null)
                throw new ArgumentNullException(propertyName);
        }

        internal static void ValidateIsEqual<T>(T existingValue, T expectedValue, string propertyName)
        {
            if (existingValue == null)
                throw new ArgumentNullException(propertyName);

            if (!existingValue.Equals(expectedValue))
                throw new ArgumentException("Unexpected value found for argument:" + propertyName);
        }
    }
}
