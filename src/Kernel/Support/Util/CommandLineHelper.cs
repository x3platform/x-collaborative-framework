namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Runtime.InteropServices;
    #endregion

    /// <summary>�����и�����(ֻ֧�� Windows ����)</summary>
    public sealed class CommandLineHelper
    {
        private static IntPtr consoleHandle;

        private const int standOutputHandle = -11;

        private const byte empty = 32;

        /// <summary>����</summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Coordinate
        {
            public short X;
            public short Y;
        }

        // Structure defines the Coordinateinates of the upper-left and lower-right corners of a rectangle
        /// <summary>
        /// ����
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
        /// <summary>����̨����Ϣ</summary>
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
            //BLACK             0         ��          ���߾���
            //BLUE              1         ��          ���߾���
            //GREEN             2         ��          ���߾���
            //CYAN              3         ��          ���߾���
            //RED               4         ��          ���߾���
            //MAGENTA           5        ���         ���߾���
            //BROWN             6         ��          ���߾���
            //LIGHTGRAY         7        ����         ���߾���
            //DARKGRAY          8        ���         ֻ�����ַ�
            //LIGHTBLUE         9        ����         ֻ�����ַ�
            //LIGHTGREEN        10       ����         ֻ�����ַ�
            //LIGHTCYAN         11       ����         ֻ�����ַ�
            //LIGHTRED          12       ����         ֻ�����ַ�
            //LIGHTMAGENTA      13       �����       ֻ�����ַ�
            //YELLOW            14       ��           ֻ�����ַ�
            //WHITE             15       ��           ֻ�����ַ�
            //BLINK             128      ��˸         ֻ�����ַ�

            Black = 0,                  //��ɫ        ���߾���
            Blue = 1,                   //��ɫ        ���߾���
            Green = 2,                  //��ɫ        ���߾���
            Cyan = 3,                   //��ɫ        ���߾���
            Red = 4,                    //��ɫ        ���߾���
            DarkGray = 8,               //���       ֻ�����ַ�
            LightBlue = 9,
            LightGreen = 10,
            LightCyan = 11,
            LightRed = 12,
            /// <summary>�����</summary>
            Purple = 13,                //�����       ֻ�����ַ�
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
