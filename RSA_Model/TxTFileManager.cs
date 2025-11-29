using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

//OPEN, READ, AND DELETE TEMP TXT FILES

namespace Rsa_Model
{
    internal class TxTFileManager
    {

        private static readonly string path = Path.Combine(Path.GetTempPath(), $"readfile_{Guid.NewGuid():N}.txt");

        internal static string GetTheInputInTextEditor()
        {
            Create();
            OpenUsingNotepad();
            string input = Read();
            Delete();
            return input;
        }

        internal static void Create(string messageInTheTxt)
        {
            File.WriteAllText(path, messageInTheTxt);
            OpenUsingNotepad();
            Delete();
        }
        private static void Create()
        {
            //CREATES A .txt FILE IN THE temp FOLDER
            File.WriteAllText(path, "DELETE ALL EXSITING TEXT THEN TYPE YOUR INPUT !!!\nSAVE THE FILE AFTER FINISHING\n(NOTE: check the console for the required data)");
        }

        private static void OpenUsingNotepad()
        {
            //OPEN NOTEPAD IN WINDOWS AND WAITS UNTIL IT IS CLOSED
            using 
            (
                var p = Process.Start(new ProcessStartInfo
                {
                    FileName = "notepad.exe",
                    Arguments = "\"" + path + "\"",
                    UseShellExecute = false
                }
                )
            )
            {
                if (p == null) {throw new InvalidOperationException("FAILD TO START NOTEPAD !!!");}
                
                p.WaitForExit();
            }
        }

        private static string Read()
        {
            return File.ReadAllText(path);
        }

        private static void Delete()
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path)) { File.Delete(path); }
            }
            catch (System.Exception)
            {
                
                throw new FileNotFoundException("CAN NOT DELETE THE FILE !!!");
            }
        }
    }
}