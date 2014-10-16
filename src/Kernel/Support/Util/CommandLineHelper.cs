namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Runtime.InteropServices;
    #endregion

    /// <summary>命令行辅助类(只支持 Windows 环境)</summary>
    public sealed class CommandLineHelper
    {
        private static IntPtr consoleHandle;

        private const int standOutputHandle = -11;

        private const byte empty = 32;

        /// <summary>坐标</summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Coordinate
        {
            public short X;
            public short Y;
        }

        // Structure defines the Coordinateinates of the upper-left and lower-right corners of a rectangle
        /// <summary>
        /// 矩形
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Rectangle
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        // Structure containing information about the Console's screen buffer.
        /// <summary>控制台的信息</summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ConsoleScreenBufferInfo
        {
            public Coordinate Size;
            public Coordinate CursorPosition;
            public int Attributes;
            public Rectangle Window;
            public Coordinate MaximumWindowSize;
        }

        // Win32 API Function declarations.

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern int SetConsoleTextAttribute(IntPtr hConsoleOutput, int Attributes);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr WaitForSingleObject(int nStdHandle);

        [DllImport("Kernel32.dll")]
        public static extern int FillConsoleOutputCharacter(IntPtr hConsoleOutput, byte cCharacter, int nLength, Coordinate dwWriteCoordinate, IntPtr lpNumberOfCharsWritten);

        [DllImport("Kernel32.dll")]
        public static extern int GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out ConsoleScreenBufferInfo lpConsoleScreenBufferInfo);

        [DllImport("Kernel32.dll")]
        public static extern int SetConsoleCursorPosition(IntPtr hConsoleOutput, Coordinate CursorPosition);

        public enum Foreground
        {
            //BLACK             0         黑          两者均可
            //BLUE              1         兰          两者均可
            //GREEN             2         绿          两者均可
            //CYAN              3         青          两者均可
            //RED               4         红          两者均可
            //MAGENTA           5        洋红         两者均可
            //BROWN             6         棕          两者均可
            //LIGHTGRAY         7        淡灰         两者均可
            //DARKGRAY          8        深灰         只用于字符
            //LIGHTBLUE         9        淡兰         只用于字符
            //LIGHTGREEN        10       淡绿         只用于字符
            //LIGHTCYAN         11       淡青         只用于字符
            //LIGHTRED          12       淡红         只用于字符
            //LIGHTMAGENTA      13       淡洋红       只用于字符
            //YELLOW            14       黄           只用于字符
            //WHITE             15       白           只用于字符
            //BLINK             128      闪烁         只用于字符

            Black = 0,                  //黑色        两者均可
            Blue = 1,                   //蓝色        两者均可
            Green = 2,                  //绿色        两者均可
            Cyan = 3,                   //青色        两者均可
            Red = 4,                    //红色        两者均可
            DarkGray = 8,               //深灰       只用于字符
            LightBlue = 9,
            LightGreen = 10,
            LightCyan = 11,
            LightRed = 12,
            /// <summary>淡洋红</summary>
            Purple = 13,                //淡洋红       只用于字符
            Yellow = 14,
            White = 15,
        }

        public static void SetTextColor(Foreground color)
        {
            SetTextColor((int)color);
        }

        public static void SetTextColor(int color)
        {
            consoleHandle = GetStdHandle(standOutputHandle);
            SetConsoleTextAttribute(consoleHandle, color);
        }

        public static void ResetColor()
        {
            consoleHandle = GetStdHandle(standOutputHandle);
            SetConsoleTextAttribute(consoleHandle, 7);
        }

        // Subroutine used to clear the Console screen.
        public static void Clear()
        {
            IntPtr consoleHandle = IntPtr.Zero;
            IntPtr writtenChars = IntPtr.Zero;

            ConsoleScreenBufferInfo consoleInfo;
            Coordinate home;

            // Set the home position for the cursor.
            home.X = 0;
            home.Y = 0;

            // Get Handle for standard output;
            consoleHandle = GetStdHandle(standOutputHandle);

            // Get information about the standard output buffer of the Console
            GetConsoleScreenBufferInfo(consoleHandle, out consoleInfo);

            // Fill output buffer with empty characters (ASCII 32)
            FillConsoleOutputCharacter(consoleHandle, empty, consoleInfo.Size.X * consoleInfo.Size.Y, home, writtenChars);

            // Set the Console cursor back to the origin
            SetConsoleCursorPosition(consoleHandle, home);
        }
    }
}
