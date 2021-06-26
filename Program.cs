using System;
using static System.Console;
using System.Collections.Generic;
using csharpncurses;
using static csharpncurses.NCurses;

namespace txtEditor
{
    class Program
    {
        public int argc;
        static  void Main(string[] args)
        {
            var prg = new Program();
            var init = new Init();
            var ed = new Editor();
            string fn = "";

           WriteLine("Welcome to the shitty txt editor!");

           if(prg.argc > 1)
           {
               fn = args[1];
               ed = new Editor(fn);
           }
           else
           {
               ed = new Editor();
           }

            init.curses_init();

            while(ed.getMode() != 'x')
            {
                ed.updateStatus();
                ed.printStatusLine();
                ed.PrintBuff();
                ConsoleKeyInfo input = ReadKey();
                ed.handleInput(input);
            }

            Refresh();
            EndWin();
        }
    }
}
