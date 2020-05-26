using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using Xunit.Abstractions;

namespace ProjectEuler
{
    public class Problems1To10 : Problems
    {
        public Problems1To10(ITestOutputHelper output) : base(output) { }

        [Theory, InlineData(1000)]
        public void MultiplesOf3And5(int limit)
        {
            Output.WriteLine($"{Enumerable.Range(3, limit-3).Where(v => v % 3 == 0 || v % 5 == 0).Sum()}");
        }

        [Theory, InlineData(4_000_000)]
        public void EvenFibonacciNumbers(int limit)
        {
            (int One, int Two) sequence = (1, 2);
            var result = 2;

            while (sequence.Two < limit) {
                var newValue = sequence.One + sequence.Two;
                if (newValue % 2 == 0) result += newValue;

                sequence.One = sequence.Two;
                sequence.Two = newValue;
            }

            Output.WriteLine($"{result}");
        }

        [Theory, InlineData(600_851_475_143 )]
        public void LargetPrimeFactor(long limit)
        {
            long prime = 2;
            while (limit > prime*prime) {
                if (limit % prime != 0) {
                    prime++;
                    continue;
                }

                limit /= prime;
            }

            if (limit > prime) prime = limit;
            
            Output.WriteLine($"{prime}");
        }

        [Theory, InlineData(3)]
        public void LargestPalindromeProduct(int digits)
        {
            var max = Math.Pow(10, digits) - 1;
            var min = Math.Pow(10, digits-1) - 1;
            var current = max;

            double result = 0;
            while (max > min && current > min) {
                var product = current * max;
                if (IsPalindrome(product)) {
                    min = current;
                    if (product > result) result = product;
                }

                if (--current <= min) {
                    max--;
                    current = max;
                }
            }
            
            Output.WriteLine($"{result}");

            bool IsPalindrome(double number)
            {
                if (number < 10) return true;
                
                char[] digits = number.ToString().ToArray();
                for (int i = 0; i < digits.Length / 2; i++) {
                    if (digits[i] != digits[digits.Length -1 - i]) return false;
                }

                return true;
            }
        }

        [Theory, InlineData(20)]
        public void SmallestMultiple(int n)
        {
            int[] divisors = Enumerable.Range(1, n).ToArray();
            long result = n;

            while (!DivisibleByAll(result)) result += n;

            Output.WriteLine($"{result}");

            bool DivisibleByAll(long number) => divisors.All(d => number % d == 0);
        }

        [Theory, InlineData(100)]
        public void SumSquareDifference(int n)
        {
            var sum = 0;
            var square = 0;

            for (int i = 1; i <= n; i++) {
                sum += i;
                square += i * i;
            }
            
            Output.WriteLine($"{(sum*sum) - square}");
        }

        [Theory, InlineData(10_001)]
        public void TenThousandFirstPrime(int n)
        {
            var primeCount = 1;
            long currentPrime = 2;
            long currentNumber = currentPrime;

            while (primeCount < n) {
                if (IsPrime(++currentNumber)) primeCount++;
            }
            
            Output.WriteLine($"{currentNumber}");

            bool IsPrime(long value)
            {
                for (long i = 2; i <= Math.Sqrt(value); i++) {
                    if (value % i == 0) return false;
                }

                return true;
            }
        }

        [Theory, InlineData(13)]
        public void LargestProductInASeries(int n)
        {
            var number = new StringBuilder("73167176531330624919225119674426574742355349194934");
            number.Append("96983520312774506326239578318016984801869478851843");
            number.Append("85861560789112949495459501737958331952853208805511");
            number.Append("12540698747158523863050715693290963295227443043557");
            number.Append("66896648950445244523161731856403098711121722383113");
            number.Append("62229893423380308135336276614282806444486645238749");
            number.Append("30358907296290491560440772390713810515859307960866");
            number.Append("70172427121883998797908792274921901699720888093776");
            number.Append("65727333001053367881220235421809751254540594752243");
            number.Append("52584907711670556013604839586446706324415722155397");
            number.Append("53697817977846174064955149290862569321978468622482");
            number.Append("83972241375657056057490261407972968652414535100474");
            number.Append("82166370484403199890008895243450658541227588666881");
            number.Append("16427171479924442928230863465674813919123162824586");
            number.Append("17866458359124566529476545682848912883142607690042");
            number.Append("24219022671055626321111109370544217506941658960408");
            number.Append("07198403850962455444362981230987879927244284909188");
            number.Append("84580156166097919133875499200524063689912560717606");
            number.Append("05886116467109405077541002256983155200055935729725");
            number.Append("71636269561882670428252483600823257530420752963450");

            long result = 0;
            for (int i = 0; i < number.Length; i++) {
                if (number.Length - i < n) { 
                    Output.WriteLine($"{result}");
                    return;
                }

                long current = int.Parse(number[i].ToString());
                for (int j = 1; j < n; j++) {
                    current *= int.Parse(number[j + i].ToString());
                    if (current == 0) {
                        i = i+j;
                        break;
                    }
                }

                if (current > result) result = current;
            }
            
            Output.WriteLine($"{result}");
        }

        [Theory, InlineData(1000)]
        public void SpecialPythagoreanTriplet(int n)
        {
            int a = 0;
            int b;

            double triplet;
            do {
                a++;
                b = a;
                
                do {
                    triplet = a + ++b + Math.Sqrt(a * a + b * b);
                } while (triplet < n);
            } while (triplet > n || triplet % 1 != 0);
            
            
            Output.WriteLine($"{(triplet-a-b) * a * b}");
        }

        [Theory, InlineData(2_000_000)]
        public void SummationOfPrimes(int n)
        {
            var primes = new bool[n -1];

            for (int i = 1; i < n - 1; i++) {
                long currentNumber = i + 1;
                if (primes[i]) continue;
                for (long j = currentNumber*currentNumber; j < n; j += currentNumber) {
                    primes[j-1] = true;
                }
            }

            long sum = 0;
            for (int i = 1; i < n - 1; i++) {
                if (!primes[i]) sum += i + 1;
            }
            
            Output.WriteLine($"{sum}");
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