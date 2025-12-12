using System;
using System.Linq;
using System.Threading;

namespace ToDoList
{
    internal static class FancyUi
    {
        // Colours for rainbow numbers (cycled per menu option)
        // TODO: Maybe add more colors later? cuz fancy
        internal static readonly ConsoleColor[] RainbowColors = new[]
        {
            ConsoleColor.Red,
            ConsoleColor.Yellow,
            ConsoleColor.Green,
            ConsoleColor.Cyan,
            ConsoleColor.Blue,
            ConsoleColor.Magenta
        };

        private static int RainbowIndex = 0; // I dont like to shorten varible names even if maybe that would be better lmao

        internal static void ResetRainbow()
        {
            RainbowIndex = 0;
        }

        // Helper: print menu options with ONLY the number in rainbow colour (not the parentheses) I LOVE RAINBOWS
        internal static void WriteMenuOption(string number, string text)
        {
            Console.Write("(");

            // pick one color per option
            var color = RainbowColors[RainbowIndex % RainbowColors.Length];
            RainbowIndex++;

            Console.ForegroundColor = color;
            Console.Write(number);
            Console.ResetColor();
            Console.WriteLine($") {text}");
        }

        // Flair #2: progress bar :)
        internal static void PrintProgressBar(int todo, int done)
        {
            int total = todo + done;
            if (total <= 0)
            {
                Console.WriteLine("No tasks yet. Time to add some!\n");
                return;
            }

            const int progressBarWidth = 20;
            int doneBlocks = (int)Math.Round((double)done / total * progressBarWidth);
            string bar = new string('#', doneBlocks).PadRight(progressBarWidth, '-');
            Console.WriteLine($"Progress: [{bar}] {done}/{total}\n");
        }

        // Flair #3: ASCII banner for TODOLIST in big capital letters, rainbow coloured + Christmas snow 
        internal static void WriteBanner()
        {
            // ASCII art - took forever to align this properly lol
            string[] bannerlines =
            {
                "TTTTT  OOOOO  DDDDD   OOOOO   L      IIIII  SSSSS  TTTTT",
                "  T    O   O  D    D  O   O   L        I    S        T  ",
                "  T    O   O  D    D  O   O   L        I    SSSS     T  ",
                "  T    O   O  D    D  O   O   L        I       S     T  ",
                "  T    OOOOO  DDDDD   OOOOO   LLLLL  IIIII  SSSSS    T  "
            };

            int colorIndex = 0;
            int width = bannerlines.Max(l => l.Length); // banner width
            int left = 0;                               // start at left edge

            foreach (string line in bannerlines)
            {
                Console.SetCursorPosition(left, Console.CursorTop);

                foreach (char character in line)
                {
                    if (character == ' ')
                    {
                        Console.Write(' ');
                        continue;
                    }

                    var color = RainbowColors[colorIndex % RainbowColors.Length];
                    colorIndex++;

                    Console.ForegroundColor = color;
                    Console.Write(character);
                    Console.ResetColor();
                }

                Console.WriteLine();
            }

            Console.WriteLine();

            // little snow animation just under the banner
            int snowTop = Console.CursorTop;
            AnimateSnowLine(left, width, snowTop);
        }

        // Tiny snow / sparkle line under the banner for Christmas vibes 
        // A bit overkill but it looks cool, so whatever you...know..
        // IT'S CHRISTMAS TIME!!!!
        private static void AnimateSnowLine(int left, int width, int top)
        {
            var random = new Random();
            const int frames = 12;   // how long it animates

            for (int frame = 0; frame < frames; frame++)
            {
                Console.SetCursorPosition(left, top);

                for (int position = 0; position < width; position++)
                {
                    bool snowHere = random.NextDouble() < 0.20; // density of flakes

                    if (snowHere)
                    {
                        var color = RainbowColors[(frame + position) % RainbowColors.Length];
                        Console.ForegroundColor = color;
                        Console.Write('*'); // "snowflake" lol
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write('.'); // background dotttyys for alignment :D
                    }
                }

                Thread.Sleep(120);
            }

            // move cursor to next line so the rest of the UI prints below the snow
            Console.SetCursorPosition(0, top + 1);
            Console.WriteLine();
        }
    }
}
