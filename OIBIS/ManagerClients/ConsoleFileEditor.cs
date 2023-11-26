using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ManagerClients
{
    public class ConsoleFileEditor
    {
        private List<string> lines;
        private int cursorTop;
        private int cursorLeft;
        private int currentLine;
        private int totalLines;
        Regex r = new Regex(@"[a-zA-Z0-9]\.[a-zA-Z]+$");

        public ConsoleFileEditor()
        {
            lines = new List<string>
            {
                string.Empty
            };
            cursorTop = 0;
            cursorLeft = 0;
            currentLine = 0;
            totalLines = 1;
        }
        public ConsoleFileEditor(string[] content)
        {
            lines = content.ToList();
            cursorTop = 0;
            cursorLeft = 0;
            currentLine = 0;
            totalLines = lines.Count();

        }

        public void Display()
        {
            Console.Clear();
            Console.CursorVisible = true;

            int MaxRows = Console.BufferHeight - 1;
            int start = currentLine - cursorTop;
            int end = start + MaxRows;
            end = end > totalLines ? totalLines : end;

            for (int i = start; i < end; i++)
            {
                Console.WriteLine(lines[i]);
            }
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }

        public void Edit()
        {
            ConsoleKeyInfo keyInfo;

            do
            {
                Display();
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentLine > 0)
                        {
                            currentLine--;
                            if (cursorTop - 1 >= 0)
                                cursorTop--;
                            cursorLeft = lines[currentLine].Length;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (currentLine < totalLines - 1)
                        {
                            currentLine++;
                            cursorLeft = lines[currentLine].Length;
                            if (cursorTop + 1 < Console.BufferHeight)
                                cursorTop++;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (cursorLeft > 0)
                        {
                            cursorLeft--;
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (cursorLeft < lines[currentLine].Length)
                        {
                            cursorLeft++;
                        }
                        break;

                    case ConsoleKey.Backspace:
                        if (cursorLeft > 0)
                        {
                            cursorLeft--;
                            lines[currentLine] = lines[currentLine].Remove(cursorLeft, 1);
                        }
                        else
                        {
                            if (lines.Count > 1)
                            {
                                lines.RemoveAt(currentLine);
                                totalLines--;
                                if (currentLine - 1 >= 0)
                                    currentLine--;
                                cursorLeft = lines[currentLine].Length;
                                if (cursorTop - 1 >= 0)
                                    cursorTop--;
                            }
                        }
                        break;

                    case ConsoleKey.Enter:
                        string newLine = lines[currentLine].Substring(cursorLeft);
                        lines[currentLine] = lines[currentLine].Substring(0, cursorLeft);
                        currentLine++;
                        lines.Insert(currentLine, newLine);
                        cursorLeft = 0;
                        totalLines++;
                        if (cursorTop + 1 < Console.BufferHeight)
                            cursorTop++;
                        break;

                    default:
                        char c = keyInfo.KeyChar;
                        lines[currentLine] = lines[currentLine].Insert(cursorLeft++, c.ToString());
                        break;
                }
            } while (keyInfo.Key != ConsoleKey.Escape);

            Console.CursorVisible = true;
            Console.Clear();
        }

        public IFile SaveToFile(string fileName)
        {
            if (!r.IsMatch(fileName))
            {
                fileName += ".txt";
            }
            MonitoredFile mf = new MonitoredFile();
            mf.Name = fileName;
            mf.Hash = string.Empty;
            StreamWriter sw = new StreamWriter(mf.File);
            sw.AutoFlush = true;
            foreach (var x in lines)
            {
                sw.Write(x.ToString());
            }
            return mf;
        }
    }
}
