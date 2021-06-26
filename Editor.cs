using System;
using csharpncurses;
using System.IO;
using System.Diagnostics;
using static csharpncurses.NCurses;
using static System.Console;

namespace txtEditor  //here works
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
            if(y-1 >= 0) y--;
            if(x >= buff.lines[y].Length)
            {
                x = buff.lines[y].Length;
            }
            Move(x, y);
        }

        private void MoveDown()
        {
            if(y + 1 < Console.WindowHeight && y + 1 < buff.lines.Count)
            {
                y++;
            }
            if(x >= buff.lines[y].Length)
            {
                x = buff.lines[y].Length;
            }
            Move(x, y);
        }

        private  void MoveLeft()
        {
            if(x-1 >= 0)
            {
                x--;
                Move(x, y);
            }
        }

        private  void MoveRight()
        {
            if(x+1 < Console.WindowWidth && x+1 <= buff.lines[y].Length)
            {
                x++;
                Move(x, y);
            }
        }

        private  void DeleteLine()
        {
            buff.removeLine(y);
        }

        private  void DeleteLine(int line)
        {
            buff.removeLine(line);
        }

        private  void SaveFile()
        {
            if(filename == " ")
            {
                filename = "untitiled.txt";
            }

             try
            {
                using (var fs = new FileStream(filename, FileMode.Create))
                {
                    // Open the text file using a stream reader.
                    using (var sw = new StreamWriter(fs))
                    {
                        for (var i = 0; i < buff.lines.Count; i++)
                        {
                            sw.WriteLine(buff.lines[i]);
                        }
                        status = "Saved to file!";
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("The file {0} could not be read:", filename);
                Debug.WriteLine(e.Message);
                status = "Error: Cannot open file for writing!";
            }
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
            for(int i = 0; i < Console.WindowHeight; i++)
            {
                if(i >= buff.lines.Count)
                {
                    Move(i, 0);
                    ClearToEndOfLine();
                }
                else 
                {
                    Write(buff.lines[i], i, 0);
                }
                ClearToEndOfLine();
            }
            Move(x, y);
        }

        public void printStatusLine()
        {
            AttributeOn((uint)NCursesAttribute.REVERSE);
            Write(status, WindowHeight - 1, 0);
            ClearToEndOfLine();
            AttributeOff((uint)NCursesAttribute.REVERSE);
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