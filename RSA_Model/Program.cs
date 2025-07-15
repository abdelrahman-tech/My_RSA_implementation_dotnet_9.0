using System;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;

namespace RsaModel
{
    //I LEFT SOME OF THE OLD BUGGY CODE THAT I TRIED TO USE SO THAT ANY OBSERVER CAN SEE HOW I THOUGHT AT THE FIRST WHILE I TRIED BUILDING THE PROGRAM.
    
    class Program
    {
        static void Main()
        {
            // p & q are primes
            // e is a number s.t. gcd(e, eulerTotientFunction) = 1
            // so e can basicly be any suitable prime
            // if : 1 < e < eulerTotientFunction && gcd(e, eulerTotientFunction) = 1

            Console.Clear();
            System.Console.WriteLine("NOTE: THIS IS AN IMPLEMENTATION FOR THE RSA\nAFTER STUDYING IT IN 6.042J | Fall 2010 | Mathematics for Computer Science\nIT MAY BE ENHANCED IN THE FUTERE\n");

            System.Console.Write("Enter the text you want to encrypt: ");
            string plainMessage = Console.ReadLine() ?? "";

            BigInteger p, q, n, e, d, eulerTotientFunction;

            System.Console.Write("What is the desired size of the prime used in the encryption? (powers of 2 only): ");
            int size;
            bool validSize = Int32.TryParse(Console.ReadLine(), out size);

            System.Console.Write("How many time would you like to test the generated number for primality?: ");
            int accuracy;
            bool validAccuracy = Int32.TryParse(Console.ReadLine(), out accuracy);
            if (validSize && validAccuracy)
            {
                p = Checker.Prime(size, accuracy);
                q = Checker.Prime(size, accuracy);
                n = p * q;
                e = Checker.Prime(size / 4, accuracy);
                eulerTotientFunction = (p - 1) * (q - 1);
                d = ModInverse(e, eulerTotientFunction);

                if (!((e * d) % eulerTotientFunction == 1))
                    throw new ArgumentException("d is not e's multiplicative inverse mod phi");

            }
            else
            {
                System.Console.WriteLine("INVALID INPUT!\nTRY AGAIN!!\n");
                return;
            }
            BigInteger[] publicKey = [e, n];
            BigInteger[] privateKey = [d, n];

            BigInteger encryptedMessage = Encrypt(plainMessage, publicKey);
            string encryptedMessageText = NumberToText(encryptedMessage);
            string message = NumberToText(Decrypt(encryptedMessage, privateKey));

            System.Console.WriteLine($"The message: {plainMessage}\nThe Public Key: ({publicKey[0]}, {publicKey[1]})\nThe Private Key: ({privateKey[0]}, {privateKey[1]})\n");
            System.Console.WriteLine($"The Encrypted message:\nAs a text:{encryptedMessageText}\nAs a number: {encryptedMessage}\n\nThe Decrypted message: {message}");


            Console.ReadLine();
        }

        static BigInteger ModInverse(BigInteger e, BigInteger eulerTotientFunction)
        {
            // a*x + b*y = gcd(a, b) - EEA
            BigInteger a = eulerTotientFunction, b = e;
            //Bézout coefficient
            BigInteger X0 = 0, X1 = 1;

            while (b != 0)
            {
                BigInteger quotient = a / b;
                (a, b) = (b, a - (quotient * b)); //Euclid step
                (X0, X1) = (X1, X0 - (quotient * X1));//track Bézout coefficient
            }
            //Now a == gcd (e, eulerToti...) == 1. And X0 is our inverse.
            if (X0 < 0)
                X0 += eulerTotientFunction;

            return X0;
        }

        static BigInteger TextToNumber(string text)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(text);
            return new BigInteger(messageBytes, isBigEndian: true, isUnsigned: true);

            // int n = 0;
            // int[] asciiNum = new int[text.Length];
            // string strWholeAscii = "";
            // BigInteger numWholeAscii;
            // foreach (char item in text)
            // {
            //     asciiNum[n] = (int)item;
            //     strWholeAscii += ((int)item).ToString("D3"); //pad 3 digits for saft
            //     n++;
            // }
            // numWholeAscii = BigInteger.Parse(strWholeAscii);

            // return numWholeAscii;
        }

        static string NumberToText(BigInteger messageAsNumber)
        {
            byte[] bytes = messageAsNumber.ToByteArray(isBigEndian: true, isUnsigned: true);
            return Encoding.UTF8.GetString(bytes);

            // string message = messageAsNumber.ToString();
            // string result = "";
            // for (int i = 0; i < message.Length - 2; i += 3)
            // {
            //     string subString = message.Substring(i, 3);
            //     int asciiCode = Int32.Parse(subString);
            //     result += (char)asciiCode;
            // }
            // return result;
        }

        static BigInteger Encrypt(string message, BigInteger[] key)
        {
            BigInteger messageAsNumber = TextToNumber(message);
            BigInteger encryptedMessage = BigInteger.ModPow(messageAsNumber, key[0], key[1]);
            return encryptedMessage;
        }

        static BigInteger Decrypt(BigInteger messagePrime, BigInteger[] key)
        {
            //messagePrime == decrypted message
            BigInteger result = BigInteger.ModPow(messagePrime, key[0], key[1]);
            return result;
        }

    }

    internal class Checker
    {
        public static BigInteger Prime(int size, int accuracy)
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
        static bool MillerTest(BigInteger maybePrime, int k)
        {
            // maybePrime: an odd integer to be tested for primality
            // k the number of rounds of testing to perform
            if (maybePrime < 4 && maybePrime > 1)
                return true;

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

    internal class Generator
    {
        public static BigInteger RandomUsingBitLength(int size)
        {
            BigInteger min = BigInteger.Pow(2, size - 1) + 1;
            BigInteger max = BigInteger.Pow(2, size) - 1;
            BigInteger bigRandomNumber = RandomBigInteger(min, max);
            return bigRandomNumber;
        }

        public static BigInteger NextBigInteger(int bitLength)
        {
            if (bitLength < 1) return BigInteger.Zero;

            int bytes = bitLength / 8; // calculate the full needed bytes
            int bits = bitLength % 8; // calculate the remaining bits

            // Generates enough random bytes to cover our bits.
            Random randomNumber = new Random();
            byte[] bs = new byte[bytes + 1]; //create array of bytes with size (byte + 1)
            randomNumber.NextBytes(bs);
            // Mask out the unnecessary bits.
            byte mask = (byte)(0xFF >> (8 - bits)); // >> : right shift operator
            bs[bs.Length - 1] &= mask; //bitwise AND: turns off any bits that aren't allowed by the mask

            return new BigInteger(bs);
        }

        // Random Integer Generator within the given range
        public static BigInteger RandomBigInteger(BigInteger start, BigInteger end)
        {
            if (start == end) return start;

            BigInteger range = end;

            // Swap start and end if given in reverse order.
            if (start > end)
            {
                end = start;
                start = range;
                range = end - start;
            }
            else
                // The distance between start and end to generate a random BigIntger between 0 and (end-start) (non-inclusive).
                range -= start;

            // convert the range to bytes to analyze its bit length
            byte[] bs = range.ToByteArray();

            // Count the number of bits necessary for range.
            int bits = 8;
            byte mask = 0x7F; // 0x7F => 01111111
            while ((bs[bs.Length - 1] & mask) == bs[bs.Length - 1] && mask == 0)
            {
                //The loop figures out how many leading zero bits exist in the highest byte of the number
                //The condition checks if all bits outside the mask are zero
                bits--;
                mask >>= 1;
            }
            bits += 8 * bs.Length;

            // Generate a random BigInteger that is the first power of 2 larger than range, 
            // then scale the range down to the size of range,
            // finally add start back on to shift back to the desired range and return.
            return ((NextBigInteger(bits + 1) * range) / BigInteger.Pow(2, bits + 1)) + start;
        }
    }
}