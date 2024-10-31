using System.Text;

namespace RevLayle;

public class ConsoleBuffer : IConsoleBuffer
{
    public ConsoleBuffer(int width, int height)
    {
        BufferSize(width, height);
    }

    private IConsoleBufferData _buffer;

    private QuickConsoleColor _currentForegroundColor = QuickConsoleColor.White;
    private QuickConsoleColor _currentBackgroundColor = QuickConsoleColor.Black;

    public IConsoleBufferData Data => _buffer;
    public int Width { get; private set; }
    public int Height  { get; private set; }
    
    public void WriteBuffer(Stream stream)
    {
        var builder = new StringBuilder();
        var prevForegroundColor = -1;
        var prevBackgroundColor = -1;
        builder.Append("\x1b[1;1H");
        for (var i = 0; i < _buffer.Length; i++)
        {
            if (i > 0 && i % Width == 0)
                builder.Append('\n');
            if ((int) _buffer.ForegroundColors[i] != prevForegroundColor)
            {
                prevForegroundColor = (int) _buffer.ForegroundColors[i];
                builder.Append($"\x1b[{30 + prevForegroundColor}m");
            }
            if ((int) _buffer.BackgroundColors[i] != prevBackgroundColor)
            {
                prevBackgroundColor = (int) _buffer.BackgroundColors[i];
                builder.Append($"\x1b[{40 + prevBackgroundColor}m");
            }
            builder.Append(char.IsControl(_buffer.Chars[i]) ? ' ' : _buffer.Chars[i]);
        }
        stream.Write(Encoding.ASCII.GetBytes(builder.ToString()));
        stream.Flush();
    }
    
    public bool IsOutOfBounds(int x, int y)
    {
        return x < 0 || x >= Width || y < 0 || y >= Height;
    }
    
    public void BufferSize(int width, int height, bool keepOriginalBuffer = false)
    {
        Height = height;
        Width = width;
        _buffer = new ConsoleBufferData(width * height);
    }
    
    public void SetColor(QuickConsoleColor color)
    {
        _currentForegroundColor = color;
    }

    public void SetBackgroundColor(QuickConsoleColor color)
    {
        _currentBackgroundColor = color;
    }

    public void Text(int x, int y, string text)
    {
        Text(x, y, text, _currentForegroundColor, _currentBackgroundColor);
    }

    public void Text(int x, int y, string text, QuickConsoleColor color)
    {
        Text(x, y, text, color, _currentBackgroundColor);
    }

    public void Text(int x, int y, string text, QuickConsoleColor color, QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        var textArray = text.ToCharArray();
        Array.Copy(textArray, 0, _buffer.Chars, x + y * Width, textArray.Length);
        var colorIndexStart = x + y * Width;
        var maxLength = Math.Min(textArray.Length, Width - x);
        for (var i = 0; i < maxLength; i++)
        {
            _buffer.ForegroundColors[colorIndexStart] = _currentForegroundColor;
            _buffer.BackgroundColors[colorIndexStart++] = _currentBackgroundColor;
        }
    }

    public void Char(int x, int y, char c)
    {
        Char(x, y, c, _currentForegroundColor, _currentBackgroundColor);
    }

    public void Char(int x, int y, char c, QuickConsoleColor color)
    {
        Char(x, y, c, color, _currentBackgroundColor);
    }

    public void Char(int x, int y, char c, QuickConsoleColor color, QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        var idx = x + y * Width;
        _buffer.Chars[idx] = c;
        _buffer.ForegroundColors[idx] = color;
        _buffer.BackgroundColors[idx] = background;
    }

    public void Rectangle(int x, int y, int width, int height, char c)
    {
        Rectangle(x, y, width, height, c, _currentForegroundColor, _currentBackgroundColor);
    }

    public void Rectangle(int x, int y, int width, int height, char c, QuickConsoleColor color)
    {
        Rectangle(x, y, width, height, c, color, _currentBackgroundColor);
    }

    public void Rectangle(int x, int y, int width, int height, char c, QuickConsoleColor color, QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                var idx = (j + x) + (i + y) * Width;
                _buffer.Chars[idx] = c;
                _buffer.ForegroundColors[idx] = color;
                _buffer.BackgroundColors[idx] = background;
            }
        }
    }

    public void Box(int x, int y, int width, int height, char c)
    {
        Box(x, y, width, height, c, c, c, _currentForegroundColor, _currentBackgroundColor);
    }

    public void Box(int x, int y, int width, int height, char c, QuickConsoleColor color)
    {
        Box(x, y, width, height, c, c, c, color, _currentBackgroundColor);
    }

    public void Box(int x, int y, int width, int height, char c, QuickConsoleColor color, QuickConsoleColor background)
    {
        Box(x, y, width, height, c, c, c, color, background);
    }

    public void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner)
    {
        Box(x, y, width, height, cSides, cTopBottom, cCorner, _currentForegroundColor, _currentBackgroundColor);
    }

    public void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner, QuickConsoleColor color)
    {
        Box(x, y, width, height, cSides, cTopBottom, cCorner, color, _currentBackgroundColor);
    }

    public void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner, QuickConsoleColor color,
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
                if (ch == 0)
                    continue;
                if ((j + x) >= Width || (i + y) >= Height)
                    continue;
                var idx = (j + x) + (i + y) * Width;
                _buffer.Chars[idx] = ch;
                _buffer.ForegroundColors[idx] = color;
                _buffer.BackgroundColors[idx] = background;
            }
        }
    }

    public void Line(int x, int y, int length, LineDirection direction, char c)
    {
        Line(x, y, length, direction, c, _currentForegroundColor, _currentBackgroundColor);
    }

    public void Line(int x, int y, int length, LineDirection direction, char c, QuickConsoleColor color)
    {
        Line(x, y, length, direction, c, color, _currentBackgroundColor);
    }

    public void Line(int x, int y, int length, LineDirection direction, char c, QuickConsoleColor color, QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        var inc = direction == LineDirection.Horizontal ? 1 : Width;
        var idx = x + y * Width;
        var maxLength = Math.Min(length, direction== LineDirection.Horizontal ? Width - x : Width - y);
        for (var i = 0; i < maxLength; i++)
        {
            _buffer.Chars[idx] = c;
            _buffer.ForegroundColors[idx] = color;
            _buffer.BackgroundColors[idx] = background;
            idx += inc;
        }
    }

    public char GetCharAt(int x, int y)
    {
        if (IsOutOfBounds(x, y)) return (char)0;
        return _buffer.Chars[x + y * Width];
    }

    public string GetStringAt(int x, int y, int length)
    {
        if (IsOutOfBounds(x, y)) return string.Empty;
        var idx = x + y * Width;
        return new string(_buffer.Chars[idx..(idx + length)]);
    }

    public QuickConsoleColor GetColorAt(int x, int y)
    {
        if (IsOutOfBounds(x, y)) return QuickConsoleColor.Black;
        return _buffer.ForegroundColors[x + y * Width];
    }

    public QuickConsoleColor GetBackgroundColorAt(int x, int y)
    {
        if (IsOutOfBounds(x, y)) return QuickConsoleColor.Black;
        return _buffer.BackgroundColors[x + y * Width];
    }

    public void Draw(int x, int y, IConsoleBuffer buffer, bool zeroCharIsTransparent)
    {
        var offset = 0;
        var idx = x + y * Width;
        for (var bufferIdx = 0; bufferIdx < buffer.Data.Length; bufferIdx++)
        {
            var ch = buffer.Data.Chars[bufferIdx];
            if (zeroCharIsTransparent == false || ch > 0)
            {
                _buffer.Chars[idx] = ch;
                _buffer.ForegroundColors[idx] = buffer.Data.ForegroundColors[bufferIdx];
                _buffer.BackgroundColors[idx] = buffer.Data.BackgroundColors[bufferIdx];
            }

            idx++;
            if (bufferIdx % buffer.Width == buffer.Width - 1)
                idx += Width - buffer.Width;
        }
    }
}