using System.Text;

namespace RevLayle;

public static class QuickConsole
{
    static QuickConsole()
    { 
        if (Environment.UserInteractive == true)
            SetCursorVisible(false);
        _buffer = new ConsoleBuffer(80, 25);
    }

    private static IConsoleBuffer _buffer;

    private static void SetCursorVisible(bool visible)
    {
        try
        {
            Console.CursorVisible = visible;
        }
        catch (IOException ioex)
        {
            // eat exception
        }
    }

    public static void BufferSize(int width, int height)
    {
        _buffer = new ConsoleBuffer(width, height);
    }

    public static void WriteBuffer()
    {
        _buffer.WriteBuffer(Console.Out);
    }
    
    public static IConsoleBuffer GetBuffer() => _buffer;

    public static bool KeyAvailable => Console.KeyAvailable;
    public static ConsoleKeyInfo ReadKey() => Console.ReadKey(true);

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
                    _buffer.Cell(x + actualLength, y, ConsoleBufferCell.Zero);
                }
                else continue;
            }
            else if (actualLength >= maxLength)
                continue;
            else if (char.IsControl(info.KeyChar))
                continue;
            else
            {
                _buffer.Cell(x + actualLength, y, ConsoleBufferCell.FromChar(info.KeyChar));
                actualLength++;
            }

            SetCursorVisible(false);
            WriteBuffer();
            SetCursorVisible(true);
            Console.SetCursorPosition(x + actualLength, y);
        }

        Console.CursorVisible = false;
        return GetStringAt(x, y, actualLength);
    }

    public static void Text(int x, int y, string text) => _buffer.Text(x, y, text);
    public static void Text(int x, int y, string text, QuickConsoleColor color) => _buffer.Text(x, y, text, color);
    public static void Text(int x, int y, string text, QuickConsoleColor color, QuickConsoleColor background) =>
        _buffer.Text(x, y, text, color, background);
    public static void Cell(int x, int y, ConsoleBufferCell cell) => _buffer.Cell(x, y, cell);
    public static void Rectangle(int x, int y, int width, int height, ConsoleBufferCell cell) =>
        _buffer.Rectangle(x, y, width, height, cell);
    public static void Box(int x, int y, int width, int height, ConsoleBufferCell cell) => _buffer.Box(x, y, width, height, cell);
    public static void Box(int x, int y, int width, int height, ConsoleBufferCell cellSides, ConsoleBufferCell cellTopBottom,
        ConsoleBufferCell cellCorners) =>
        _buffer.Box(x, y, width, height, cellSides, cellTopBottom, cellCorners);

    public static void Line(int x, int y, int length, LineDirection direction, ConsoleBufferCell cell)
        => _buffer.Line(x, y, length, direction, cell);
    public static ConsoleBufferCell GetCellAt(int x, int y) => _buffer.GetCellAt(x, y);
    public static string GetStringAt(int x, int y, int length) => _buffer.GetStringAt(x, y, length);
    public static void Draw(int x, int y, IConsoleBuffer buffer)
        => _buffer.Draw(x, y, buffer);
}