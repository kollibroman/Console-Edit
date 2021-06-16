using System;
using csharpncurses;
using System.IO;
using System.Diagnostics;
using static csharpncurses.NCurses;

namespace txtEditor
{
    public class Editor : Screen
    {
        private int x, y;
        private char mode;
        private string status, filename;
        private Buffer buff;

        public Editor()
        {
            x = 0;
            y = 0;
            mode = 'n';
            status = "Normal Mode";
            filename = "untititlied";

            buff = new Buffer();
            buff.appendLine("");
        }

        public Editor(string fn)
        {
            x = 0;
            y = 0;
            mode = 'n';
            status = "Normal Mode";
            filename = "untititlied";

            buff = new Buffer();

            try
            {
                using (var fs = new FileStream(filename, FileMode.Open))
                {
                    // Open the text file using a stream reader.
                    using (var sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            // Read the stream to a string, and append it to the buffer.
                            buff.appendLine(sr.ReadLine());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("The file {0} could not be read:", filename);
                Debug.WriteLine(e.Message);
                buff.appendLine("");
            }
        }

        private void MoveUp()
        {
            
        }

        private void MoveDown()
        {

        }

        private  void MoveLeft()
        {

        }

        private  void MoveRight()
        {

        }

        private  void DeleteLine()
        {

        }

        private  void DeleteLine(int line)
        {

        }

        private  void SaveFile()
        {

        }

        public char getMode()
        {
            return mode;
        }

        public void handleInput(ConsoleKeyInfo input)
        {
             switch (input.Key)
            {
                case ConsoleKey.LeftArrow:
                    MoveLeft();
                    return;
                case ConsoleKey.RightArrow:
                    MoveRight();
                    return;
                case ConsoleKey.UpArrow:
                    MoveUp();
                    return;
                case ConsoleKey.DownArrow:
                    MoveDown();
                    return;
            }

            switch (mode)
            {
                case 'n':
                    switch (input.Key)
                    {
                        case ConsoleKey.X:
                            // Press 'x' to exit
                            mode = 'x';
                            break;
                        case ConsoleKey.I:
                            // Press 'i' to enter insert mode
                            mode = 'i';
                            break;
                        case ConsoleKey.S:
                            // Press 's' to save the current file
                            SaveFile();
                            break;
                    }
                    break;
                case 'i':
                    switch (input.Key)
                    {
                        case ConsoleKey.Escape:
                            // The Escape/Alt key
                            mode = 'n';
                            break;
                        case ConsoleKey.Backspace:
                            // The Backspace key
                            if (x == 0 && y > 0)
                            {
                                x = buff.lines[y - 1].Length;
                                // Bring the line down
                                buff.lines[y - 1] += buff.lines[y];
                                // Delete the current line
                                DeleteLine();
                                MoveUp();
                            }
                            else
                            {
                                try
                                {
                                    // Removes a character
                                    buff.lines[y] = buff.lines[y].Remove(--x, 1);
                                }
                                catch (ArgumentOutOfRangeException e)
                                {
                                    Debug.WriteLine(e.Message);
                                    // Attempting to delete from beginning of empty line
                                    Console.Beep();
                                    x++;
                                }
                            }
                            break;
                        case ConsoleKey.Delete:
                            // The Delete key
                            if (x == buff.lines[y].Length && y != buff.lines.Count - 1)
                            {
                                // Bring the line down
                                buff.lines[y] += buff.lines[y + 1];
                                // Delete the line
                                DeleteLine(y + 1);
                            }
                            else
                            {
                                try
                                {
                                    buff.lines[y] = buff.lines[y].Remove(x, 1);
                                }
                                catch (ArgumentOutOfRangeException e)
                                {
                                    Debug.WriteLine(e.Message);
                                    // Attempting to delete from end of line
                                    Console.Beep();
                                }
                            }
                            break;
                        case ConsoleKey.Enter:
                            // The Enter key
                            // Bring the rest of the line down
                            if (x < buff.lines[y].Length)
                            {
                                // Put the rest of the line on a new line
                                buff.insertLine(buff.lines[y].Substring(x, buff.lines[y].Length - x), y + 1);
                                // Remove that part of the line
                                buff.lines[y] = buff.lines[y].Remove(x, buff.lines[y].Length - x);
                            }
                            else
                            {
                                buff.insertLine("", y + 1);
                            }
                            x = 0;
                            MoveDown();
                            break;
                        case ConsoleKey.Tab:
                            // The Tab key
                            buff.lines[y] = buff.lines[y].Insert(x, "    ");
                            x += 4;
                            break;
                        default:
                            // Any other character
                            buff.lines[y] = buff.lines[y].Insert(x, input.KeyChar.ToString());
                            x++;
                            break;
                    }
                    break;
            }
        }

        public void PrintBuff()
        {

        }

        public void printStatusLine()
        {

        }

        public void updateStatus()
        {
            switch(mode)
            {
            case 'n':
                // Normal mode
                status = "Normal Mode";
                break;
            case 'i':
                // Insert mode
                status = "Insert Mode";
                break;
            case 'x':
                // Exiting
                status = "Exiting";
                break;
            }
            status += "\tCOL" + x.ToString() + "\tROw" + y.ToString();
        }
    }
}