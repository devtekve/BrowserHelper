using System;
using System.Windows.Forms;

namespace ConsoleApp1
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.Run(new Form1());
        }
    }
}