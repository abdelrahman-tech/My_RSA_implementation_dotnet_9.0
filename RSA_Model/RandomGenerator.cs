using System;
using System.Numerics;

namespace Rsa_Model
{
    internal static class Generator
    {
        internal static BigInteger RandomUsingBitLength(int size)
        {
            BigInteger min = BigInteger.Pow(2, size - 1) + 1;
            BigInteger max = BigInteger.Pow(2, size) - 1;
            BigInteger bigRandomNumber = RandomBigInteger(min, max);
            return bigRandomNumber;
        }

        private static BigInteger NextBigInteger(int bitLength)
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
        internal static BigInteger RandomBigInteger(BigInteger start, BigInteger end)
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