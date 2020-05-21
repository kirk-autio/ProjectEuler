using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace ProjectEuler
{
    public class Problems
    {
        public Problems(ITestOutputHelper output) => Output = output;
        
        public ITestOutputHelper Output { get; }
        
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
    }
}