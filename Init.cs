using System;
using csharpncurses;
using static csharpncurses.NCurses;

namespace txtEditor
{
    public class Init
    {
        private readonly IntPtr _stdscr;

        public void curses_init()
        {
            InitScreen();
            NoEcho();
            Keypad(_stdscr, true);
        }
    }
}