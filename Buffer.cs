using System.Collections.Generic;
using csharpncurses;
using static csharpncurses.NCurses;

namespace txtEditor
{
    public unsafe class Buffer
    {
       public  List<string> lines;
        public Buffer()
        {
            lines = new List<string>();
        }
        public void insertLine(string line, int numln)
        {
           line = remTabs(line);
           lines.Insert(numln, line);
        }

        public void appendLine(string line)
        {
          line = remTabs(line);
          lines.Add(line);
        }

        public void removeLine(int line)
        {
            lines.RemoveAt(line);
        }

        public string remTabs(string line)
        {
            int tab = line.IndexOf('\t');
            if (tab == -1)
            {
                return line;
            }
            else
            {
                return remTabs(line.Replace("\t", "    "));
            }
        }
    }
}