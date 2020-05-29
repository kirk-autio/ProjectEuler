using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace ProjectEuler
{
    public class Problems11to20 : Problems
    {
        public Problems11to20(ITestOutputHelper output) : base(output) { }

        [Theory, InlineData(4)]
        public void LargestProductInAGrid(int n)
        {
            var arr = new[,] { { 1, 2 }, { 1, 2 } };
            var array = new[] {
                GetArrayFrom("08 02 22 97 38 15 00 40 00 75 04 05 07 78 52 12 50 77 91 08")
                , GetArrayFrom("49 49 99 40 17 81 18 57 60 87 17 40 98 43 69 48 04 56 62 00")
                , GetArrayFrom("81 49 31 73 55 79 14 29 93 71 40 67 53 88 30 03 49 13 36 65")
                , GetArrayFrom("52 70 95 23 04 60 11 42 69 24 68 56 01 32 56 71 37 02 36 91")
                , GetArrayFrom("22 31 16 71 51 67 63 89 41 92 36 54 22 40 40 28 66 33 13 80")
                , GetArrayFrom("24 47 32 60 99 03 45 02 44 75 33 53 78 36 84 20 35 17 12 50")
                , GetArrayFrom("32 98 81 28 64 23 67 10 26 38 40 67 59 54 70 66 18 38 64 70")
                , GetArrayFrom("67 26 20 68 02 62 12 20 95 63 94 39 63 08 40 91 66 49 94 21")
                , GetArrayFrom("24 55 58 05 66 73 99 26 97 17 78 78 96 83 14 88 34 89 63 72")
                , GetArrayFrom("21 36 23 09 75 00 76 44 20 45 35 14 00 61 33 97 34 31 33 95")
                , GetArrayFrom("78 17 53 28 22 75 31 67 15 94 03 80 04 62 16 14 09 53 56 92")
                , GetArrayFrom("16 39 05 42 96 35 31 47 55 58 88 24 00 17 54 24 36 29 85 57")
                , GetArrayFrom("86 56 00 48 35 71 89 07 05 44 44 37 44 60 21 58 51 54 17 58")
                , GetArrayFrom("19 80 81 68 05 94 47 69 28 73 92 13 86 52 17 77 04 89 55 40")
                , GetArrayFrom("04 52 08 83 97 35 99 16 07 97 57 32 16 26 26 79 33 27 98 66")
                , GetArrayFrom("88 36 68 87 57 62 20 72 03 46 33 67 46 55 12 32 63 93 53 69")
                , GetArrayFrom("04 42 16 73 38 25 39 11 24 94 72 18 08 46 29 32 40 62 76 36")
                , GetArrayFrom("20 69 36 41 72 30 23 88 34 62 99 69 82 67 59 85 74 04 36 16")
                , GetArrayFrom("20 73 35 29 78 31 90 01 74 31 49 71 48 86 81 16 23 57 05 54")
                , GetArrayFrom("01 70 54 71 83 51 54 69 16 92 33 48 61 43 52 01 89 19 67 48")
            };

            long product = 0;
            for (int i = 0; i < array.Length; i++) {
                for (int j = 0; j < array[i].Length; j++) {
                    long horizontal, upDiagonal, downDiagonal;
                    var vertical = horizontal = upDiagonal = downDiagonal = array[i][j];
                    
                    for (int k = 1; k < n; k++) {
                        if (i + n <= array.Length) vertical *= array[i + k][j];
                        if (j + n <= array[i].Length) horizontal *= array[i][j + k];
                        if (i + n <= array.Length && j + n <= array[i].Length) upDiagonal *= array[i + k][j + k];
                        if (i + n <= array.Length && j - n >= 0) downDiagonal *= array[i + k][j - k];
                    }

                    if (product < vertical) product = vertical;
                    if (product < horizontal) product = horizontal;
                    if (product < upDiagonal) product = upDiagonal;
                    if (product < downDiagonal) product = downDiagonal;
                }
            }
            
            Output.WriteLine($"{product}");

           int[] GetArrayFrom(string value) => value.Split(' ').Select(int.Parse).ToArray();
        }

        [Theory, InlineData(500)]
        public void HighlyDivisibleTriangularNumber(int n)
        {
            var next = 0;
            long number = 0;
            var factors = new List<long>();
            do {
                number += ++next;
                factors.Clear();
                for (long i = 1; i < Math.Sqrt(number); i++) {
                    if (number % i == 0) factors.AddRange(new[] { i, number / i });
                }
            } while (factors.Count <= n);

            Output.WriteLine($"{number}");
        }
        
        [Theory, InlineData(10)]
        public void LargeSum(int n)
        {
            List<long> numbers = new List<long>();
            
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ProjectEuler.largeNumber.txt"))
            using (var reader = new StreamReader(stream)) {
                while (!reader.EndOfStream) if (long.TryParse(reader.ReadLine().Substring(0, 12), out var num)) numbers.Add(num);
            }

            long sum = 0;
            for (int i = 0; i < numbers.Count; i++) {
                sum += numbers[i];
            }
            Output.WriteLine($"{sum}".Substring(0, 10));
        }

        [Theory, InlineData(999_999)]
        public void LongestCollatzSequence(int n)
        {
            Dictionary<long, int> collatzLengths = new Dictionary<long, int> {{1,1}};
            
            var longestLength = 0;
            int greatestStartingNumber = 1;
            for (int i = n; i > 1; i--) {
                var length = CollatzLength(i);
                if (length > longestLength) {
                    longestLength = length;
                    greatestStartingNumber = i;
                }
            }
            
            Output.WriteLine($"{greatestStartingNumber}");

            int CollatzLength(long number)
            {
                if (collatzLengths.ContainsKey(number)) return collatzLengths[number];

                var collatzLength = 1 + CollatzLength(number % 2 == 0 ? number / 2 : number * 3 + 1);
                if (number == 1500040) ;
                collatzLengths.Add(number, collatzLength);
                return collatzLength;
            }
        }

        [Theory, InlineData(20)]
        public void LatticePaths(int n)
        {
            double result = 1;
            for (int i = n+n; i > n; i--) {
                result *= i;
                result /= i - n;
            }
            
            Output.WriteLine($"{Math.Round(result)}");
        }

        [Theory, InlineData(1000)]
        public void PowerDigitSum(int n)
        {
            double exponential = Math.Pow(2, n);
            
            var sum = 0;
            foreach (var digit in exponential.ToString("N")) {
                sum += char.IsDigit(digit) ? (int) char.GetNumericValue(digit) : 0;
            }
            
            Output.WriteLine($"{sum}");
        }

        [Theory, InlineData(1000)]
        public void NumberLetterCounts(int n)
        {
            string[] onesStrings = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] teensStrings = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tensStrings = { "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            var length = 0;
            for (int i = 1; i <= n; i++) length += AsString(i).Length;
            
            Output.WriteLine($"{length}");
                
            string AsString(int value)
            {
                var thousands = value / 1000;
                var hundreds = value%1000 / 100;
                var lessThanHundred = value % 100;
                var tens = (value%100) / 10;
                var ones = value % 10;
                    
                StringBuilder builder = new StringBuilder();
                if (thousands > 0) builder.Append($"{onesStrings[thousands - 1]}thousand");
                if (hundreds > 0) builder.Append($"{onesStrings[hundreds - 1]}hundred");
                if (hundreds > 0 && lessThanHundred > 0) builder.Append("and");
                if (tens > 1) builder.Append($"{tensStrings[tens -1]}");
                if (tens == 1 && ones > 0) builder.Append($"{teensStrings[ones-1]}");
                if (tens == 1 && ones == 0) builder.Append($"{tensStrings[ones]}");
                if (tens != 1 && ones > 0) builder.Append($"{onesStrings[ones - 1]}");

                return builder.ToString();
            }
        }
        
        [Fact]
        public void MaximumPathSum1()
        {
            List<List<int>> values = new List<List<int>>();
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ProjectEuler.MaxPathSum1.txt"))
            using (var reader = new StreamReader(stream)) {
                while (!reader.EndOfStream) {
                    values.Add(reader.ReadLine()?.Trim(' ').Split(' ').Select(int.Parse).ToList());
                }
            }

            for (int i = values.Count-2; i >= 0; i--) {
                for (int j = 0; j <= i; j++) {
                    values[i][j] += Math.Max(values[i + 1][j], values[i + 1][j + 1]);
                }
            }
            
            Output.WriteLine($"{values[0][0]}");
        }

        [Theory, InlineData(1901, 2000)]
        public void CountingSundays(int yearFrom, int yearTo)
        {
            var months = new[]{ 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            var currentDay = 2;
            var sundays = 0;

            for (int year = yearFrom; year <= yearTo; year++) {
                for (int month = 0; month < months.Length; month++) {
                    var daysInMonth = month ==1 && year % 4 == 0 && (year % 100 != 0 || year % 400 == 0) ? months[month] + 1 : months[month];
                    currentDay += daysInMonth % 7;
                    if (currentDay % 7 == 0) sundays++;
                }
            }
            
            Output.WriteLine($"{sundays}");
        }

        private void Multiply(int number, List<int> currentNumber)
        {
            int carry = 0;

            for (int i = 0; i < currentNumber.Count; i++) {
                int result = carry + currentNumber[i] * number;
                currentNumber[i] = result % 10;
                carry = result / 10;
            }

            while (carry != 0) {
                currentNumber.Add(carry % 10);
                carry /= 10;
            }
        }
        
        [Theory, InlineData(100)]
        public void FactorialDigitSum(uint n)
        {
            var digit = new List<int> {1};

            for (int i = 2; i < n; i++) {
                Multiply(i, digit);
            }

            Output.WriteLine($"{digit.Sum()}");
        }
    }
}