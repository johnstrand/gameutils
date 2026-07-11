#pragma warning disable CS0649  // Fields are populated by P/Invoke marshaling
#pragma warning disable S1104   // P/Invoke interop structs require public fields; properties are not supported by the marshaler
namespace GameUtils.Term;
internal static class PInvoke
{
    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out ConsoleMode lpMode);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, ConsoleMode dwMode);

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
    internal static extern IntPtr CreateConsoleScreenBuffer(uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwFlags, IntPtr lpScreenBufferData);
}

/// <summary>Console coordinate (column/row) used in Windows console API calls.</summary>
public struct Coord
{
    /// <summary>Column (X) coordinate.</summary>
    public short X;
    /// <summary>Row (Y) coordinate.</summary>
    public short Y;
}

/// <summary>Defines the coordinates of the upper-left and lower-right corners of a rectangle used in Windows console API calls.</summary>
public struct SmallRectangle
{
    /// <summary>Left column of the rectangle.</summary>
    public short Left;
    /// <summary>Top row of the rectangle.</summary>
    public short Top;
    /// <summary>Right column of the rectangle.</summary>
    public short Right;
    /// <summary>Bottom row of the rectangle.</summary>
    public short Bottom;
}

/// <summary>Contains information about a console screen buffer returned by <c>GetConsoleScreenBufferInfo</c>.</summary>
public struct ConsoleScreenBufferInfo
{
    /// <summary>Size of the screen buffer, in character columns and rows.</summary>
    public Coord Size;
    /// <summary>Current position of the cursor in the screen buffer.</summary>
    public Coord CursorPosition;
    /// <summary>Attributes of the characters written to a screen buffer by <c>WriteFile</c> and <c>WriteConsole</c>.</summary>
    public short Attributes;
    /// <summary>Coordinates of the upper-left and lower-right corners of the display window.</summary>
    public SmallRectangle Window;
    /// <summary>Maximum size of the console window, in character columns and rows, given the current screen buffer size and font.</summary>
    public Coord MaximumWindowSize;
}

/// <summary>Contains information about the size and visibility of the cursor for the specified console screen buffer.</summary>
public struct ConsoleCursorInfo
{
    /// <summary>Percentage of the character cell filled by the cursor (1–100).</summary>
    public uint Size;
    /// <summary>Visibility of the cursor; true if visible.</summary>
    public bool Visible;
}

/// <summary>
/// Windows console input and output mode flags used with <c>GetConsoleMode</c> / <c>SetConsoleMode</c>.
/// Input and output flags share the same numeric values but apply to different handles.
/// </summary>
[Flags]
public enum ConsoleMode : uint
{
    /// <summary>CTRL+C is processed by the system and not placed in the input buffer.</summary>
    ENABLE_PROCESSED_INPUT = 0x0001,
    /// <summary>The ReadFile or ReadConsole function returns only when a carriage return character is read.</summary>
    ENABLE_LINE_INPUT = 0x0002,
    /// <summary>Characters read by ReadFile or ReadConsole are written to the active screen buffer as they are typed.</summary>
    ENABLE_ECHO_INPUT = 0x0004,
    /// <summary>User interactions that change the size of the console screen buffer are reported in the input buffer.</summary>
    ENABLE_WINDOW_INPUT = 0x0008,
    /// <summary>Mouse events are placed in the input buffer.</summary>
    ENABLE_MOUSE_INPUT = 0x0010,
    /// <summary>When enabled, text entered in a console window will be inserted at the current cursor location.</summary>
    ENABLE_INSERT_MODE = 0x0020,
    /// <summary>This flag enables the user to use the mouse to select and edit text (Quick Edit mode).</summary>
    ENABLE_QUICK_EDIT_MODE = 0x0040,
    /// <summary>Required to enable or disable extended flags. Must be combined with ENABLE_INSERT_MODE or ENABLE_QUICK_EDIT_MODE.</summary>
    ENABLE_EXTENDED_FLAGS = 0x0080,
    /// <summary>Setting this flag directs the Virtual Terminal input sequences to be processed by the console host.</summary>
    ENABLE_AUTO_POSITION = 0x0100,
    /// <summary>Setting this flag directs the Virtual Terminal input sequences to be processed by the console host.</summary>
    ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200,

    /// <summary>Characters written by WriteFile or WriteConsole or echoed by ReadFile or ReadConsole are parsed for ASCII control sequences.</summary>
    ENABLE_PROCESSED_OUTPUT = 0x0001,
    /// <summary>The cursor moves to the beginning of the next row when it reaches the end of the current row.</summary>
    ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,
    /// <summary>When writing with WriteFile or WriteConsole, characters are parsed for VT100 and similar control character sequences.</summary>
    ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004,
    /// <summary>When writing with WriteFile or WriteConsole, the cursor will not move to the beginning of the next row after it reaches the end of a row.</summary>
    DISABLE_NEWLINE_AUTO_RETURN = 0x0008,
    /// <summary>The APIs for writing character attributes including WriteConsoleOutput and ReadConsoleOutput will treat the character attributes as if they're encoding a LVB (Latin VT Bonus) attribute.</summary>
    ENABLE_LVB_GRID_WORLDWIDE = 0x0010,
}

#pragma warning restore CS0649
#pragma warning restore S1104
