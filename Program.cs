using System;
using static System.Console;
using System.Collections.Generic;
using csharpncurses;
using static csharpncurses.NCurses;

namespace txtEditor
{
    class Program
    {
        static void Main(int argc, string[] args)
        {
            var init = new Init();
            string fn = "";

           WriteLine("Welcome to the shitty txt editor!");

           if(argc > 1)
           {
               fn = args[1];
           }

            init.curses_init();

            Refresh();
            EndWin();
        }
    }
}
