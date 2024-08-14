using DialogGameConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogGameConsole.Util
{
    public static class Assert
    {
        private const double DoubleRoundingLimit = 0.001; 

        public static void IsUndefined<T>(T enumValue, string name) where T : struct
        {
            if (!Enum.IsDefined(typeof(T), enumValue))
                throw new ArgumentException($"Value {enumValue} for variable {name} is not defined for enum of type {typeof(T)}");
            var intValue = (int)(object)enumValue;
            if (intValue != 0) 
                throw new ArgumentException($"Enum for variable {name} of type {typeof(T)} needs to be Undefined");
        }

        public static void IsNotDefault<T>(T enumValue, string name) where T : struct
        {
            if (!Enum.IsDefined(typeof(T), enumValue))
                throw new ArgumentException($"Value {enumValue} for variable {name} is not defined for enum of type {typeof(T)}");
            var intValue = (int)(object)enumValue;
            if (intValue == 0) 
                throw new ArgumentException($"Enum for variable {name} of type {typeof(T)} can't be Undefined");
        }

        public static void IsDefault<T>(T enumValue, string name) where T : struct {
            if (!Enum.IsDefined(typeof(T), enumValue))
                throw new ArgumentException($"Value {enumValue} for variable {name} is not defined for enum of type {typeof(T)}");
            var intValue = (int)(object)enumValue;
            if (intValue != 0)
                throw new ArgumentException($"Enum for variable {name} of type {typeof(T)} needs to be the Default Value");
        }

        public static void IsEmpty(string value, string name)
        {
            if (value != "") 
                throw new ArgumentException($"String for variable {name} needs to be empty");
        }

        public static void IsNotEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"String for variable {name} cannot be null or empty");
        }

        public static void IsEqual<T>(T value, T expected, string name)
        {
            if (!value.Equals(expected))
                throw new ArgumentException($"Variable {name} needs to have value {expected} (actual {value})");
        }

        public static void IsNotEqual<T>(T value, T notExpected, string name)
        {
            if (value.Equals(notExpected))
                throw new ArgumentException($"Variable {name} cannot have value {notExpected}");
        }

        public static void IsPositive(double value, string name)
        {
            if (value <= 0)
                throw new ArgumentException($"Number for variable {name} needs to be positive. Value {value}");
        }

        public static void IsNonNegative(double value, string name)
        {
            if (value < 0)
                throw new ArgumentException($"Number for variable {name} needs to be non-negative. Value {value}");
        }

        public static void IsNotNull<T>(T value, string name)
        {
            if (value is null)
                throw new ArgumentException($"Variable {name} cannot be null");
        }

        public static void IsNull<T>(T value, string name) {
            if (value is not null)
                throw new ArgumentException($"Variable {name} needs to be null");
        }

        public static void IsWithin(double value, double lowerLimit, double upperLimit, string name)
        {
            if (value <= lowerLimit || value >= upperLimit)
                throw new ArgumentException($"Number for variable {name} needs to withing {lowerLimit} and {upperLimit}. Value {value}");
        }

        public static void IsBetween(double value, double lowerLimit, double upperLimit, string name)
        {
            if (value < lowerLimit || value > upperLimit)
                throw new ArgumentException($"Number for variable {name} needs to withing {lowerLimit} and {upperLimit}. Value {value}");
        }

        public static void IsNotEmpty<T>(IEnumerable<T> value, string name)
        {
            Assert.IsNotNull(value, name);
            if (!value.Any())
                throw new ArgumentException($"List for variable {name} cannot be empty");
        }

        public static void TotalEquals(IEnumerable<double> value, double total, string name)
        {
            if(Math.Abs(total - value.Sum()) > DoubleRoundingLimit)
                throw new ArgumentException($"List value doesn't match expected total: {value.Sum()} (list sum) vs {total} (total). List: {value}");
        }
    }
}
