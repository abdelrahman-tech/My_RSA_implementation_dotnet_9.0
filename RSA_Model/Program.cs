using System;
using System.Numerics;
using System.Text; 
//using System.Reflection.Metadata;

namespace Rsa_Model
{
    //I LEFT SOME OF THE OLD BUGGY CODE THAT I TRIED TO USE SO THAT ANY OBSERVER CAN SEE HOW I THOUGHT AT THE FIRST WHILE I TRIED BUILDING THE PROGRAM.
    
    class Program
    {
        static void Main()
        {
            Console.Clear();
            Menu();
/*
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
                p = PrimesAndExponents.Prime(size, accuracy);
                q = PrimesAndExponents.Prime(size, accuracy);
                n = p * q;
                eulerTotientFunction = (p - 1) * (q - 1);
                e = PrimesAndExponents.PublicExp(size / 4, accuracy, eulerTotientFunction);
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
*/
        }

        private static void Menu()
        {
            System.Console.WriteLine("WELCOME TO MY RSA PROGRAM!!\nCHOOSE ONE OF THE FOLLOWING:");
            System.Console.WriteLine("A) See an example of how the program works.\nB) Generate Public and Private key\nC) Encrypte a message\nD) Decrypte a message");
            System.Console.Write("Your Choice: ");
            string choice = (Console.ReadLine() ?? "").Trim().ToLower();

            switch (choice)
            {
                case "a":
                    EncryptAndDecrypt.ShowExamplePage();
                    break;
                case "b":
                    EncryptAndDecrypt.GenerateKeys();
                    break;
                case "c":
                    EncryptAndDecrypt.EncryptMsgPage();
                    break;
                case "d":
                    EncryptAndDecrypt.DecryptMsgPage();
                    break;
                default: 
                    System.Console.WriteLine("Make sure you choosed correctly. Single letters only!");
                    break;
            }
        }

/*
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

            //FOR CONVERTING FROM ASCII
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

            //FOR CONVERTING TO ASCII
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
            //messagePrime == encrypted message
            BigInteger result = BigInteger.ModPow(messagePrime, key[0], key[1]);
            return result;
        }

*/

    }
}