using System.Numerics;
using System.Text; 
using System;

//I LEFT SOME OF THE OLD BUGGY CODE THAT I TRIED TO USE SO THAT ANY OBSERVER CAN SEE HOW I THOUGHT AT THE FIRST WHILE I TRIED BUILDING THE PROGRAM.

namespace Rsa_Model
{
    class EncryptAndDecrypt
    {
        // p & q are primes
        // e is a number s.t. gcd(e, eulerTotientFunction) = 1
        // so e can basicly be any suitable prime
        // if : 1 < e < eulerTotientFunction && gcd(e, eulerTotientFunction) = 1

        private static BigInteger p, q, n, e, d, eulerTotientFunction;
        private static BigInteger[] publicKey = new BigInteger[2], privateKey = new BigInteger[2];
            
        internal static void ShowExamplePage()
        {
            Console.Clear();
            System.Console.Write("Enter the text you want to encrypt: ");
            string plainMessage = Console.ReadLine() ?? "";

            

            System.Console.Write("What is the desired size(in bits) of the prime used in the encryption? (powers of 2 only): ");
            int size;
            bool validSize = Int32.TryParse(Console.ReadLine(), out size);

            System.Console.Write("How many time would you like to test the generated number for primality?: ");
            int accuracy;
            bool validAccuracy = Int32.TryParse(Console.ReadLine(), out accuracy);
            
            try
            {
                if (validSize && validAccuracy)
                {
                    p = PrimesAndExponents.Prime(size, accuracy);
                    q = PrimesAndExponents.Prime(size, accuracy);
                    n = p * q;
                    eulerTotientFunction = (p - 1) * (q - 1);
                    e = PrimesAndExponents.PublicExp(size / 4, accuracy, eulerTotientFunction);
                    d = PrimesAndExponents.ModInverse(e, eulerTotientFunction);

                    if (!((e * d) % eulerTotientFunction == 1)) {throw new ArgumentException("d is not e's multiplicative inverse mod phi!\nTRY AGAIN!!");}
                }
                else
                {
                    System.Console.WriteLine("INVALID INPUT!\nTRY AGAIN!!\n");
                    return;
                }
            
                publicKey[0] = e;
                publicKey[1] = n;

                privateKey[0] = d;
                privateKey[1] = n;

                BigInteger encryptedMessage = Encrypt(plainMessage, publicKey, n);
                string encryptedMessageText = NumberToText(encryptedMessage);
                string message = NumberToText(Decrypt(encryptedMessage, privateKey));
            
                //System.Console.WriteLine($"The message: {plainMessage}\nThe Public Key: ({publicKey[0]}, {publicKey[1]})\nThe Private Key: ({privateKey[0]}, {privateKey[1]})\n");
                //System.Console.WriteLine($"The Encrypted message:\nAs a text:{encryptedMessageText}\nAs a number: {encryptedMessage}\n\nThe Decrypted message: {message}"); 
                TxTFileManager.Create($"The message: {plainMessage}\nThe Public Key: ({publicKey[0]}, {publicKey[1]})\nThe Private Key: ({privateKey[0]}, {privateKey[1]})\n\n" + $"The Encrypted message:\nAs a text:{encryptedMessageText}\nAs a number: {encryptedMessage}\n\nThe Decrypted message: {message}");
            }
            catch (DivideByZeroException)
            {
                if(p == 0) {throw new DivideByZeroException("The first prime is zero (somehow) !!!");}
                else if (q == 0) {throw new DivideByZeroException("The second prime is zero (somehow) !!!");}
                else if (n == 0) {throw new DivideByZeroException("The modulus is zero !!!");}
                else if (e == 0) {throw new DivideByZeroException("The public exponent is zero !!!");}
                else if (d == 0) {throw new DivideByZeroException("The private exponent is zero !!!");}
                else if (eulerTotientFunction == 0) {throw new DivideByZeroException("The phi function is zero !!!");}
                else if (publicKey[0] == 0) {throw new DivideByZeroException("The public exponent's place in the public key is zero !!!");}
                else if (publicKey[1] == 0) {throw new DivideByZeroException("The modulus' place in the public key is zero !!!");}
                else if (privateKey[0] == 0) {throw new DivideByZeroException("The private exponent's place in the private key is zero !!!");}
                else if (privateKey[1] == 0) {throw new DivideByZeroException("The modulus' place in the private key is zero !!!");}
                else {throw new DivideByZeroException("The compiler says their are a division by zero but all the variables are NOT zero!");}
            }
 
            Reset();
            Console.ReadLine();
        }

        internal static void EncryptMsgPage()
        {
            Console.Clear();

            System.Console.Write("WELCOME TO MY RSA ENCRYPTION PROGRAM\n\nENTER YOUR MESSAGE");
            //string message = Console.ReadLine() ?? "";
            string message = TxTFileManager.GetTheInputInTextEditor();

            System.Console.Write("\nDo you have a public key?\n[YES: y]\t[NO: n]: ");
            string choice = (Console.ReadLine() ?? "").Trim().ToLower();
            
            if(choice == "" || choice == null)
            {
                throw new ArgumentNullException();
            }

            if(choice == "y" || choice == "yes")
            {
                Reset();

                System.Console.WriteLine("ENTER THE REQUIRED DATA");

                System.Console.Write("Public exponent: ");
                bool validPublicExponent = BigInteger.TryParse((Console.ReadLine() ?? "").Trim(), out e);
                System.Console.WriteLine();

                System.Console.Write("Modulus: ");
                bool validModulus = BigInteger.TryParse((Console.ReadLine() ?? "").Trim(), out n);
                System.Console.WriteLine();


                try
                {
                    if(validModulus && validPublicExponent)
                    {
                        publicKey[0] = e;
                        publicKey[1] = n;
                        BigInteger encryptedMessageAsNumber = Encrypt(message, publicKey, n);
                        string encryptedMessageAsTxt = NumberToText(encryptedMessageAsNumber);

                        System.Console.WriteLine("THE ENCRYPTED MESSAGE:");
                        System.Console.WriteLine(encryptedMessageAsTxt);

                        Reset();
                        Console.ReadLine();
                    }
                    else
                    {
                        Reset();
                        throw new ArgumentException("Either Public Exponent or Modulus is invalid");
                    }
                }
                catch (DivideByZeroException)
                {
                    if (publicKey[1] == 0) {throw new DivideByZeroException("The modulus' place in the public key is zero !!!");}
                    else if (n == 0) {throw new DivideByZeroException("The modulus is zero !!!");}
                    else if (e == 0) {throw new DivideByZeroException("The public exponent is zero !!!");}
                    else if (publicKey[0] == 0) {throw new DivideByZeroException("The public exponent's place in the public key is zero !!!");}
                    else {throw new DivideByZeroException("The compiler says their are a division by zero but all the variables are NOT zero!");}
                }
            } 
            else if (choice == "n" || choice == "no")
            {
                System.Console.WriteLine("HERE IS A PUBLIC KEY FOR YOU\n");
                System.Console.Write("What is the desired size(in bits) of the prime used in the encryption? (powers of 2 only): ");
                int size;
                bool validSize = Int32.TryParse(Console.ReadLine(), out size);

                System.Console.Write("How many time would you like to test the generated number for primality?: ");
                int accuracy;
                bool validAccuracy = Int32.TryParse(Console.ReadLine(), out accuracy);


                try
                {
                    if(validSize && validAccuracy)
                    {
                    p = PrimesAndExponents.Prime(size, accuracy);
                    q = PrimesAndExponents.Prime(size, accuracy);
                    n = p * q;
                    eulerTotientFunction = (p - 1) * (q - 1);
                    e = PrimesAndExponents.PublicExp(size / 4, accuracy, eulerTotientFunction);
                    publicKey[0] = e;
                    publicKey[1] = n;

                    //System.Console.WriteLine($"\nPublic Exponent:\n{publicKey[0]}\n\nModulus:\n{publicKey[1]}\n");

                    BigInteger encryptedMessageAsNumber = Encrypt(message, publicKey, n);
                    string encryptedMessageAsTxt = NumberToText(encryptedMessageAsNumber);

                    //System.Console.WriteLine("THE ENCRYPTED MESSAGE:");
                    //System.Console.WriteLine(encryptedMessageAsTxt);

                    TxTFileManager.Create($"\nPublic Exponent:\n{publicKey[0]}\n\nModulus:\n{publicKey[1]}\n" + "THE ENCRYPTED MESSAGE:\n" + encryptedMessageAsTxt);
                    Reset();

                    System.Console.WriteLine("Do you need the private key?\n[YES: y]\t[NO: n]: ");
                    choice = (Console.ReadLine() ?? "").Trim().ToLower();

                    if (choice == "y" || choice == "yes")
                    {
                        d = PrimesAndExponents.ModInverse(e, eulerTotientFunction);

                        if (!((e * d) % eulerTotientFunction == 1)) {throw new ArgumentException("d is not e's multiplicative inverse mod phi!\nTRY AGAIN!!");}

                        privateKey[0] = d;
                        privateKey[1] = n;
                        System.Console.WriteLine($"\nPrivate Exponent:\n{privateKey[0]}\n\nModulus:\n{privateKey[1]}\n");

                        Reset();
                        Console.ReadLine();
                    }
                    }
                }
                catch (DivideByZeroException)
                {
                    if(p == 0) {throw new DivideByZeroException("The first prime is zero (somehow) !!!");}
                    else if (q == 0) {throw new DivideByZeroException("The second prime is zero (somehow) !!!");}
                    else if (n == 0) {throw new DivideByZeroException("The modulus is zero !!!");}
                    else if (e == 0) {throw new DivideByZeroException("The public exponent is zero !!!");}
                    else if (d == 0) {throw new DivideByZeroException("The private exponent is zero !!!");}
                    else if (eulerTotientFunction == 0) {throw new DivideByZeroException("The phi function is zero !!!");}
                    else if (publicKey[0] == 0) {throw new DivideByZeroException("The public exponent's place in the public key is zero !!!");}
                    else if (publicKey[1] == 0) {throw new DivideByZeroException("The modulus' place in the public key is zero !!!");}
                    else if (privateKey[0] == 0) {throw new DivideByZeroException("The private exponent's place in the private key is zero !!!");}
                    else if (privateKey[1] == 0) {throw new DivideByZeroException("The modulus' place in the private key is zero !!!");}
                    else {throw new DivideByZeroException("The compiler says their are a division by zero but all the variables are NOT zero!");}
                }
            }
        }

        internal static void DecryptMsgPage()
        {
            Console.Clear();
            System.Console.WriteLine("WELCOME TO MY RSA PROGRAM\n\nENTER YOUR ENCRYPTED MESSAGE AS A NUMBER");
            //string encryptedMessage = Console.ReadLine() ?? "";
            BigInteger encryptedMessage = BigInteger.Parse(TxTFileManager.GetTheInputInTextEditor());

            System.Console.WriteLine("ENTER THE PRIVATE EXPONENT: ");
            bool validPrivateExponent = BigInteger.TryParse((Console.ReadLine() ?? "").Trim(), out d);
            System.Console.WriteLine();

            System.Console.Write("Modulus: ");
            bool validModulus = BigInteger.TryParse((Console.ReadLine() ?? "").Trim(), out n);
            System.Console.WriteLine();

            try
            {
                if (validModulus && validPrivateExponent)
                {
                    privateKey[0] = d;
                    privateKey[1] = n;

                    // System.Console.WriteLine("THE DECRYPTED MESSAGE:");
                    // System.Console.WriteLine(NumberToText(Decrypt(TextToNumber(encryptedMessage), privateKey)));

                    TxTFileManager.Create("THE DECRYPTED MESSAGE:\n" + NumberToText(Decrypt(encryptedMessage, privateKey)));

                    Reset();
                    Console.ReadLine();
                } 
                else
                {
                    Reset();
                    throw new ArgumentException("Either Private Exponent or Modulus is invalid");
                }
            }
            catch (DivideByZeroException)
            {
                if(privateKey[0] == 0) {throw new DivideByZeroException("The private exponent's place in the private key is zero !!!");}
                else if (privateKey[1] == 0) {throw new DivideByZeroException("The modulus' place in the private key is zero !!!");}
                else if (n == 0) {throw new DivideByZeroException("The modulus is zero !!!");}
                else if (d == 0) {throw new DivideByZeroException("The private exponent is zero !!!");}
                else {throw new DivideByZeroException("The compiler says their are a division by zero but all the variables are NOT zero!");}
            }
        }

        internal static void GenerateKeys()
        {
            Console.Clear();

            System.Console.Write("What is the desired size(in bits) of the prime used in the encryption? (powers of 2 only): ");
            int size;
            bool validSize = Int32.TryParse(Console.ReadLine(), out size);
            System.Console.Write("How many time would you like to test the generated number for primality?: ");
            int accuracy;
            bool validAccuracy = Int32.TryParse(Console.ReadLine(), out accuracy);
            
            try
            {
                if (validSize && validAccuracy)
                {
                    p = PrimesAndExponents.Prime(size, accuracy);
                    q = PrimesAndExponents.Prime(size, accuracy);
                    n = p * q;
                    eulerTotientFunction = (p - 1) * (q - 1);
                    e = PrimesAndExponents.PublicExp(size / 4, accuracy, eulerTotientFunction);
                    d = PrimesAndExponents.ModInverse(e, eulerTotientFunction);

                    if (!((e * d) % eulerTotientFunction == 1)) {throw new ArgumentException("d is not e's multiplicative inverse mod phi!\nTRY AGAIN!!");}
                
                    publicKey[0] = e;
                    publicKey[1] = n;

                    privateKey[0] = d;
                    privateKey[1] = n;

                    System.Console.WriteLine("YOUR KEYS:");
                    System.Console.WriteLine($"PUBLIC KEY:\nPublic Exponent:\n{publicKey[0]}\n\nModulus:\n{publicKey[1]}\n\n");
                    System.Console.WriteLine($"PRIVATE KEY:\nPrivate Exponent:\n{privateKey[0]}\n\nModulus:\n{privateKey[1]}\n\n");

                    Reset();
                    Console.ReadLine();
                }
                else
                {
                    System.Console.WriteLine("INVALID INPUT!\nTRY AGAIN!!\n");
                    return;
                }
            }
            catch (DivideByZeroException)
            {
                if(p == 0) {throw new DivideByZeroException("The first prime is zero (somehow) !!!");}
                else if (q == 0) {throw new DivideByZeroException("The second prime is zero (somehow) !!!");}
                else if (n == 0) {throw new DivideByZeroException("The modulus is zero !!!");}
                else if (e == 0) {throw new DivideByZeroException("The public exponent is zero !!!");}
                else if (d == 0) {throw new DivideByZeroException("The private exponent is zero !!!");}
                else if (eulerTotientFunction == 0) {throw new DivideByZeroException("The phi function is zero !!!");}
                else if (publicKey[0] == 0) {throw new DivideByZeroException("The public exponent's place in the public key is zero !!!");}
                else if (publicKey[1] == 0) {throw new DivideByZeroException("The modulus' place in the public key is zero !!!");}
                else if (privateKey[0] == 0) {throw new DivideByZeroException("The private exponent's place in the private key is zero !!!");}
                else if (privateKey[1] == 0) {throw new DivideByZeroException("The modulus' place in the private key is zero !!!");}
                else {throw new DivideByZeroException("The compiler says their are a division by zero but all the variables are NOT zero!");}
            }
        }

        private static BigInteger Encrypt(string message, BigInteger[] key, BigInteger modulus)
        {
            try
            {
                BigInteger messageAsNumber = TextToNumber(message);
                if (modulus > messageAsNumber)
                {
                    BigInteger encryptedMessage = BigInteger.ModPow(messageAsNumber, key[0], key[1]);
                    return encryptedMessage;
                } 
                else
                {
                    throw new Exception("THE MODULUS IS NOT LARGER THAN THE MESSAGE !!!\nCHOOSE BIGGER PRIMES\n\n");
                }
                
            }
            catch (DivideByZeroException)
            {
                if (key[0] == 0) { throw new DivideByZeroException("In the Encrypt method, The key's first item is zero !!!");}
                else if (key[1] == 0) { throw new DivideByZeroException("In the Encrypt method, The key's second item is zero !!!");}
                throw;
            }
        }

        private static BigInteger Decrypt(BigInteger encryptedMessage, BigInteger[] key)
        {
            try
            {
                BigInteger result = BigInteger.ModPow(encryptedMessage, key[0], key[1]);
                return result;
            }
            catch (DivideByZeroException)
            {
                if (key[0] == 0) { throw new DivideByZeroException("In the Decrypt method, The key's first item is zero !!!");}
                else if (key[1] == 0) { throw new DivideByZeroException("In the Decrypt method, The key's second item is zero !!!");}
                throw;
            }
            
        }

        private static string NumberToText(BigInteger messageAsNumber)
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

        private static BigInteger TextToNumber(string text)
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

        private static void Reset()
        {
            p = 0;
            q = 0;
            n = 0;
            e = 0;
            d = 0;
            eulerTotientFunction = 0;

            publicKey[0] = 0;
            publicKey[1] = 0;

            privateKey[0] = 0;
            privateKey[1] = 0;
        }
    }
}