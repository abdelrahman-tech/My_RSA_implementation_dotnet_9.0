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
    }
}
