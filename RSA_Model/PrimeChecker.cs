using System.Numerics;
using System;

namespace Rsa_Model
{
    /// <summary>
    /// PrimesAndExponents:
    ///     Prime: Return a prime number with the bit size that was chosen
    ///     PublicExp: Return the Public Exponent e
    ///     IsGcdOne: Checks if the gcd for a and b is one
    ///     ModInverse: Returns the Inverse of n with respect to modNumber as a modulus
    ///PrimeChecker:
    ///     MillerTest: Checks for the primality of maybePrime and runs the test k times
    /// </summary>
    internal class PrimesAndExponents : PrimeChecker
    {
        internal static BigInteger Prime(int size, int accuracy)
        {
            BigInteger x = Generator.RandomUsingBitLength(size);
            bool isPrime = MillerTest(x, accuracy);

            while (!isPrime)
            {
                x = Generator.RandomUsingBitLength(size);
                isPrime = MillerTest(x, accuracy);
            }

            return x;
        }

        internal static BigInteger PublicExp(int size, int accuracy, BigInteger phiFunction)
        {
            BigInteger possiblePrime = Prime(size, accuracy);
            bool tmp = IsGcdOne(phiFunction, possiblePrime);

            while(!tmp)
            {
                possiblePrime = Prime(size, accuracy);
                tmp = IsGcdOne(phiFunction, possiblePrime);
            }

            return possiblePrime;
        }

        private static bool IsGcdOne(BigInteger a, BigInteger b)
        {
            BigInteger Bigger, Smaller, tmp;
            if (b > a)
            {
                Bigger = b;
                Smaller = a;
            } 
            else
            {
              Bigger = a;
              Smaller = b;  
            }

            while(Smaller != 0)
            {
                tmp = Bigger % Smaller;
                Bigger = Smaller;
                Smaller = tmp;
            }

            if (Bigger == 1)
            {
                return true;
            }

            return false;
        }

        internal static BigInteger ModInverse(BigInteger n, BigInteger modNumber)
        {
            //FIND THE MODULAR INVERSE OF n relative to the modNumber
            // a*x + b*y = gcd(a, b) - EEA
            BigInteger a = modNumber, b = n;
            //Bézout coefficient
            BigInteger X0 = 0, X1 = 1;

            while (b != 0)
            {
                BigInteger quotient = a / b;
                (a, b) = (b, a - (quotient * b)); //Euclid step
                (X0, X1) = (X1, X0 - (quotient * X1)); //track Bézout coefficient
            }

            //Now a == gcd (e, phiFunction) == 1. And X0 is our inverse.
            if (X0 < 0)
                X0 += modNumber;

            return X0;
        }
    }
    internal class PrimeChecker
    {
        protected static bool MillerTest(BigInteger maybePrime, int k)
        {
            //The test's pseudocode was brought from wikipedia
            // maybePrime: an odd integer to be tested for primality
            // k the number of rounds of testing to perform
            
            if (maybePrime < 4 && maybePrime > 1)
                return true;

            //ensure oddity
            if (maybePrime % 2 == 0)
                return false;

            //let s > 0 and d odd > 0 such that (maybePrime − 1) = (2^s)*d
            BigInteger d = maybePrime - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            double ln_maybePrime = BigInteger.Log(maybePrime);
            BigInteger upperRangeLimit = new BigInteger(Math.Floor(2 * ln_maybePrime * ln_maybePrime));

            for (int i = 0; i < k; i++)
            {
                BigInteger a = Generator.RandomBigInteger(2, maybePrime - 2);
                BigInteger y = new BigInteger();
                BigInteger x = BigInteger.ModPow(a, d, maybePrime);
                for (int j = 0; j < s; j++)
                {
                    y = BigInteger.ModPow(x, 2, maybePrime);
                    if (y == 1 && x != 1 && x != (maybePrime - 1))
                    {
                        return false;
                    }
                    x = y;
                }
                if (y != 1)
                {
                    return false;
                }
            }
            return true;
        }
    }

}
