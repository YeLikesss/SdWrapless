using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using SdWrapCore.PE;
using SdWrapCore.SdWrap;
using SdWrapCore.Utils;

namespace ConsoleExecute
{
    internal class Program
    {
        static unsafe void Main(string[] args)
        {

            string exe = "D:\\Galgame Reverse\\SoftDC\\HanaganeKanadeGram_C3_DMM.exe";
            SdWrapProgram sd = new();
            sd.Load(exe);

        }
    }
}