using System.Text;

namespace RevLayle;

public static class QuickConsole
{
    static QuickConsole()
    {
        System.Console.CursorVisible = false;
        BufferSize(80, 25);
    }
    private static int _bufferWidth;
    private static int _bufferHeight;
    private static char[] _buffer;
    private static QuickConsoleColor[] _foregroundColors;
    private static QuickConsoleColor[] _backgroundColors;

    private static QuickConsoleColor _currentForegroundColor = QuickConsoleColor.White;
    private static QuickConsoleColor _currentBackgroundColor = QuickConsoleColor.Black;

    private static bool IsOutOfBounds(int x, int y)
    {
        return x < 0 || x >= _bufferWidth || y < 0 || y >= _bufferHeight;
    }
    public static void BufferSize(int width, int height, bool keepOriginalBuffer = false)
    {
        _bufferHeight = height;
        _bufferWidth = width;
        _buffer = new char[width * height];
        _foregroundColors = new QuickConsoleColor[width * height];
        _backgroundColors = new QuickConsoleColor[width * height];
    }

    public static void WriteBuffer()
    {
        var builder = new StringBuilder();
        var prevForegroundColor = -1;
        var prevBackgroundColor = -1;
        builder.Append("\x1b[1;1H");
        for (var i = 0; i < _buffer.Length; i++)
        {
            if (i > 0 && i % _bufferWidth == 0)
                builder.Append('\n');
            if ((int) _foregroundColors[i] != prevForegroundColor)
            {
                prevForegroundColor = (int) _foregroundColors[i];
                builder.Append($"\x1b[{30 + prevForegroundColor}m");
            }
            if ((int) _backgroundColors[i] != prevBackgroundColor)
            {
                prevBackgroundColor = (int) _backgroundColors[i];
                builder.Append($"\x1b[{40 + prevBackgroundColor}m");
            }
            builder.Append(char.IsControl(_buffer[i]) ? ' ' : _buffer[i]);
        }
        using var stream = System.Console.OpenStandardOutput();
        stream.Write(Encoding.ASCII.GetBytes(builder.ToString()));
        stream.Flush();
        stream.Close();
    }
    
    public static bool KeyAvailable() => System.Console.KeyAvailable;
    public static ConsoleKeyInfo ReadKey() => System.Console.ReadKey(true);
    
    public static void SetColor(QuickConsoleColor color)
    {
        _currentForegroundColor = color;
    }

    public static void SetBackgroundColor(QuickConsoleColor color)
    {
        _currentBackgroundColor = color;
    }

    public static string ReadText(int x, int y, int maxLength)
    {
        if (IsOutOfBounds(x, y)) return string.Empty;
        var actualLength = 0;
        var rowOffset = y * _bufferWidth;
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
                    _buffer[x + actualLength + rowOffset] = (char)0;
                }
                else continue;
            }
            else if (actualLength >= maxLength)
                continue;
            else if (char.IsControl(info.KeyChar))
                continue;
            else
            {
                _buffer[x + actualLength + rowOffset] = info.KeyChar;
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

    public static void Text(int x, int y, string text)
    {
        Text(x, y, text, _currentForegroundColor, _currentBackgroundColor);
    }

    public static void Text(int x, int y, string text, QuickConsoleColor color)
    {
        Text(x, y, text, color, _currentBackgroundColor);
    }

    public static void Text(int x, int y, string text, QuickConsoleColor color, QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        var textArray = text.ToCharArray();
        Array.Copy(textArray, 0, _buffer, x + y * _bufferWidth, textArray.Length);
        var colorIndexStart = x + y * _bufferWidth;
        var maxLength = Math.Min(textArray.Length, _bufferWidth - x);
        for (var i = 0; i < maxLength; i++)
        {
            _foregroundColors[colorIndexStart] = _currentForegroundColor;
            _backgroundColors[colorIndexStart++] = _currentBackgroundColor;
        }
    }

    public static void Char(int x, int y, char c)
    {
        Char(x, y, c, _currentForegroundColor, _currentBackgroundColor);
    }

    public static void Char(int x, int y, char c, QuickConsoleColor color)
    {
        Char(x, y, c, color, _currentBackgroundColor);
    }

    public static void Char(int x, int y, char c, QuickConsoleColor color, QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        var idx = x + y * _bufferWidth;
        _buffer[idx] = c;
        _foregroundColors[idx] = color;
        _backgroundColors[idx] = background;
    }

    public static void Box(int x, int y, int width, int height, char c)
    {
        Box(x, y, width, height, c, c, c, _currentForegroundColor, _currentBackgroundColor);
    }

    public static void Box(int x, int y, int width, int height, char c, QuickConsoleColor color)
    {
        Box(x, y, width, height, c, c, c, color, _currentBackgroundColor);
    }

    public static void Box(int x, int y, int width, int height, char c, QuickConsoleColor color, QuickConsoleColor background)
    {
        Box(x, y, width, height, c, c, c, color, background);
    }

    public static void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner)
    {
        Box(x, y, width, height, cSides, cTopBottom, cCorner, _currentForegroundColor, _currentBackgroundColor);
    }

    public static void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner, QuickConsoleColor color)
    {
        Box(x, y, width, height, cSides, cTopBottom, cCorner, color, _currentBackgroundColor);
    }

    public static void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner, QuickConsoleColor color,
        QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                var ch = (i, j) switch
                {
                    (0, 0) => cCorner,
                    (0, var tj) when tj == width - 1 => cCorner,
                    (0, _) => cTopBottom,
                    (var ti, 0) when ti == height - 1 => cCorner,
                    (_, 0) => cSides,
                    var (ti, tj) when tj == width - 1 && ti == height - 1 => cCorner,
                    var (ti, _) when ti == height - 1 => cTopBottom,
                    var (_, tj) when tj == width - 1 => cSides,
                    _ => (char)0,
                };
                if ((j + x) >= _bufferWidth || (i + y) >= _bufferHeight)
                    continue;
                var idx = (j + x) + (i + y) * _bufferWidth;
                _buffer[idx] = ch;
                _foregroundColors[idx] = color;
                _backgroundColors[idx] = background;
            }
        }
    }

    public static void Line(int x, int y, int length, LineDirection direction, char c)
    {
        Line(x, y, length, direction, c, _currentForegroundColor, _currentBackgroundColor);
    }

    public static void Line(int x, int y, int length, LineDirection direction, char c, QuickConsoleColor color)
    {
        Line(x, y, length, direction, c, color, _currentBackgroundColor);
    }

    public static void Line(int x, int y, int length, LineDirection direction, char c, QuickConsoleColor color, QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        var inc = direction == LineDirection.Horizontal ? 1 : _bufferWidth;
        var idx = x + y * _bufferWidth;
        var maxLength = Math.Min(length, direction== LineDirection.Horizontal ? _bufferWidth - x : _bufferWidth - y);
        for (var i = 0; i < maxLength; i++)
        {
            _buffer[idx] = c;
            _foregroundColors[idx] = color;
            _backgroundColors[idx] = background;
            idx += inc;
        }
    }

    public static char GetCharAt(int x, int y)
    {
        if (IsOutOfBounds(x, y)) return (char)0;
        return _buffer[x + y * _bufferWidth];
    }

    public static string GetStringAt(int x, int y, int length)
    {
        if (IsOutOfBounds(x, y)) return string.Empty;
        var idx = x + y * _bufferWidth;
        return new string(_buffer[idx..(idx + length)]);
    }

    public static QuickConsoleColor GetColorAt(int x, int y)
    {
        if (IsOutOfBounds(x, y)) return QuickConsoleColor.Black;
        return _foregroundColors[x + y * _bufferWidth];
    }

    public static QuickConsoleColor GetBackgroundColorAt(int x, int y)
    {
        if (IsOutOfBounds(x, y)) return QuickConsoleColor.Black;
        return _backgroundColors[x + y * _bufferWidth];
    }
}