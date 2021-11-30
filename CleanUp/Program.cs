using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CleanUp
{
    class Program
    {
        enum RecycleFlag : int
        {
            SHERB_NOCONFIRMATION = 0x0000001,
            SHERB_NOPROGRESSUI = 0x00000001,
            SHERB_NOSOUND = 0x00000004,
        }

        [System.Runtime.InteropServices.DllImport("Shell32.dll")]
        static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlag dwFlags);

        static void Main()
        {
            SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlag.SHERB_NOSOUND | RecycleFlag.SHERB_NOCONFIRMATION);
            Console.WriteLine("now your recycle bin is empty ;)");
            int i = 0;
            foreach (var item in new List<string>()
            {
                Path.GetTempPath(),
                "C:\\Windows\\Temp"
            })
            {
                foreach (var item2 in Directory.GetFiles(item))
                    try
                    {
                        new FileInfo(item2).Delete();
                        i++;
                    }
                    catch
                    {

                    }
                foreach (var item2 in Directory.GetDirectories(item))
                    try
                    {
                        Directory.Delete(item2, true);
                        i++;
                    }
                    catch (Exception e)
                    {
                        var l = e.GetType().Name;
                        if (!e.Message.ToLower().Contains("denied") && !e.Message.ToLower().Contains("access") && !e.Message.ToLower().Contains("Could not find a part of the path") && l != "System.UnauthorizedAccessException")
                        {
                            throw;
                        }
                    }
            }
            Console.WriteLine(i + " items deleted from temps :)");
            Console.WriteLine("do you want delete them customization?\tyes / press any key");
            if (Console.ReadLine() == "yes")
            {
                Process.Start(Path.GetTempPath());
                Process.Start("C:\\Windows\\Temp");
            }
        }

    }
}