using System.Text;

namespace RevLayle;

public static class QuickConsole
{
    static QuickConsole()
    {
        System.Console.CursorVisible = false;
        _buffer = new ConsoleBuffer(80, 25);
    }

    private static IConsoleBuffer _buffer;

    public static void BufferSize(int width, int height, bool keepOriginalBuffer = false)
    {
        _buffer = new ConsoleBuffer(width, height);
    }

    public static void WriteBuffer()
    {
        var stream = Console.OpenStandardOutput();
        _buffer.WriteBuffer(stream);
    }

    public static bool KeyAvailable() => System.Console.KeyAvailable;
    public static ConsoleKeyInfo ReadKey() => System.Console.ReadKey(true);

    public static QuickConsoleColor CurrentForegroundColor
    {
        get => _buffer.CurrentForegroundColor;
        set => _buffer.CurrentForegroundColor = value;
    }
    public static QuickConsoleColor CurrentBackgroundColor
    {
        get => _buffer.CurrentBackgroundColor;
        set => _buffer.CurrentBackgroundColor = value;
    }
    public static string ReadText(int x, int y, int maxLength)
    {
        if (_buffer.IsOutOfBounds(x, y)) return string.Empty;
        var actualLength = 0;
        var rowOffset = y * _buffer.Width;
        Console.CursorVisible = true;

        ConsoleKeyInfo info;
        Console.SetCursorPosition(x, y);
        while ((info = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (info.Key == ConsoleKey.Backspace)
            {
                if (actualLength > 0)
                {
                    actualLength--;
                    _buffer.Char(x + actualLength, y, (char)0);
                }
                else continue;
            }
            else if (actualLength >= maxLength)
                continue;
            else if (char.IsControl(info.KeyChar))
                continue;
            else
            {
                _buffer.Char(x + actualLength, y, info.KeyChar);
                actualLength++;
            }

            Console.CursorVisible = false;
            WriteBuffer();
            Console.CursorVisible = true;
            Console.SetCursorPosition(x + actualLength, y);
        }

        Console.CursorVisible = false;
        return GetStringAt(x, y, actualLength);
    }

    public static void Text(int x, int y, string text) => _buffer.Text(x, y, text);
    public static void Text(int x, int y, string text, QuickConsoleColor color) => _buffer.Text(x, y, text, color);

    public static void Text(int x, int y, string text, QuickConsoleColor color, QuickConsoleColor background) =>
        _buffer.Text(x, y, text, color, background);

    public static void Char(int x, int y, char c) => _buffer.Char(x, y, c);
    public static void Char(int x, int y, char c, QuickConsoleColor color) => _buffer.Char(x, y, c, color);

    public static void Char(int x, int y, char c, QuickConsoleColor color, QuickConsoleColor background) =>
        _buffer.Char(x, y, c, color, background);

    public static void Rectangle(int x, int y, int width, int height, char c) =>
        _buffer.Rectangle(x, y, width, height, c);

    public static void Rectangle(int x, int y, int width, int height, char c, QuickConsoleColor color) =>
        _buffer.Rectangle(x, y, width, height, c, color);

    public static void Rectangle(int x, int y, int width, int height, char c, QuickConsoleColor color,
        QuickConsoleColor background) => _buffer.Rectangle(x, y, width, height, c, color, background);

    public static void Box(int x, int y, int width, int height, char c) => _buffer.Box(x, y, width, height, c);

    public static void Box(int x, int y, int width, int height, char c, QuickConsoleColor color) =>
        _buffer.Box(x, y, width, height, c, color);

    public static void Box(int x, int y, int width, int height, char c, QuickConsoleColor color,
        QuickConsoleColor background) => _buffer.Box(x, y, width, height, c, color, background);

    public static void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner) =>
        _buffer.Box(x, y, width, height, cSides, cTopBottom, cCorner);

    public static void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner,
        QuickConsoleColor color) => _buffer.Box(x, y, width, height, cSides, cTopBottom, cCorner, color);

    public static void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner,
        QuickConsoleColor color,
        QuickConsoleColor background) =>
        _buffer.Box(x, y, width, height, cSides, cTopBottom, cCorner, color, background);

    public static void Line(int x, int y, int length, LineDirection direction, char c) =>
        _buffer.Line(x, y, length, direction, c);

    public static void Line(int x, int y, int length, LineDirection direction, char c, QuickConsoleColor color) =>
        _buffer.Line(x, y, length, direction, c, color);

    public static void Line(int x, int y, int length, LineDirection direction, char c, QuickConsoleColor color,
        QuickConsoleColor background)
        => _buffer.Line(x, y, length, direction, c, color, background);

    public static char GetCharAt(int x, int y) => _buffer.GetCharAt(x, y);
    public static string GetStringAt(int x, int y, int length) => _buffer.GetStringAt(x, y, length);
    public static QuickConsoleColor GetColorAt(int x, int y) => _buffer.GetColorAt(x, y);
    public static QuickConsoleColor GetBackgroundColorAt(int x, int y) => _buffer.GetBackgroundColorAt(x, y);

    public static void Draw(int x, int y, IConsoleBuffer buffer, bool zeroCharIsTransparent)
        => _buffer.Draw(x, y, buffer, zeroCharIsTransparent);
}