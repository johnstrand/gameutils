namespace GameUtils.Term;
internal static class PInvoke
{
    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool SetConsoleActiveBuffer(IntPtr hConsoleHandle);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleHandle, Coord dwSize);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool SetConsoleWindowInfo(IntPtr hConsoleHandle, bool bAbsolute, ref SmallRectangle lpConsoleWindow);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool SetConsoleCursorPosition(IntPtr hConsoleHandle, Coord dwCursorPosition);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleHandle, out ConsoleScreenBufferInfo lpConsoleScreenBufferInfo);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool FillConsoleOutputCharacter(IntPtr hConsoleHandle, char cCharacter, uint nLength, Coord dwWriteCoord, out uint lpNumberOfCharsWritten);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr GetStdHandle(int nStdHandle);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool GetConsoleCursorInfo(IntPtr hConsoleHandle, out ConsoleCursorInfo lpConsoleCursorInfo);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool SetConsoleCursorInfo(IntPtr hConsoleHandle, ref ConsoleCursorInfo lpConsoleCursorInfo);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out ConsoleMode lpMode);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, ConsoleMode dwMode);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr CreateConsoleScreenBuffer(uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwFlags, IntPtr lpScreenBufferData);
}

public struct Coord
{
    public short X;
    public short Y;
}

public struct SmallRectangle
{
    public short Left;
    public short Top;
    public short Right;
    public short Bottom;
}

public struct ConsoleScreenBufferInfo
{
    public Coord Size;
    public Coord CursorPosition;
    public short Attributes;
    public SmallRectangle Window;
    public Coord MaximumWindowSize;
}

public struct ConsoleCursorInfo
{
    public uint Size;
    public bool Visible;
}

public enum ConsoleMode : uint
{
    ENABLE_PROCESSED_INPUT = 0x0001,
    ENABLE_LINE_INPUT = 0x0002,
    ENABLE_ECHO_INPUT = 0x0004,
    ENABLE_WINDOW_INPUT = 0x0008,
    ENABLE_MOUSE_INPUT = 0x0010,
    ENABLE_INSERT_MODE = 0x0020,
    ENABLE_QUICK_EDIT_MODE = 0x0040,
    ENABLE_EXTENDED_FLAGS = 0x0080,
    ENABLE_AUTO_POSITION = 0x0100,
    ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200,

    ENABLE_PROCESSED_OUTPUT = 0x0001,
    ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,
    ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004,
    DISABLE_NEWLINE_AUTO_RETURN = 0x0008,
    ENABLE_LVB_GRID_WORLDWIDE = 0x0010,
}
