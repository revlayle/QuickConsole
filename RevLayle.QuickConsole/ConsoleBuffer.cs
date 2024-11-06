using System.Text;

namespace RevLayle;

public class ConsoleBuffer(int width, int height) : IConsoleBuffer
{
    //private readonly IConsoleBufferData _buffer = new ConsoleBufferData(width * height);

    public QuickConsoleColor CurrentForegroundColor { get; set; } = QuickConsoleColor.White;
    public QuickConsoleColor CurrentBackgroundColor { get; set; } = QuickConsoleColor.Black;

    public ConsoleBufferCell[] Cells { get; } = new ConsoleBufferCell[width * height];

    public int Width { get; private set; } = width;
    public int Height  { get; private set; } = height;

    public void WriteBuffer(Stream stream)
    {
        var builder = new StringBuilder();
        var prevForegroundColor = -1;
        var prevBackgroundColor = -1;
        builder.Append("\x1b[1;1H");
        for (var i = 0; i < Cells.Length; i++)
        {
            var cell = Cells[i];
            if (i > 0 && i % Width == 0)
                builder.Append('\n');
            if ((int) cell.Foreground != prevForegroundColor)
            {
                prevForegroundColor = (int) cell.Foreground;
                builder.Append($"\x1b[{30 + prevForegroundColor}m");
            }
            if ((int) cell.Background != prevBackgroundColor)
            {
                prevBackgroundColor = (int) cell.Background;
                builder.Append($"\x1b[{40 + prevBackgroundColor}m");
            }
            builder.Append(char.IsControl(cell.Character) ? ' ' : cell.Character);
        }
        stream.Write(Encoding.ASCII.GetBytes(builder.ToString()));
        stream.Flush();
    }
    
    public bool IsOutOfBounds(int x, int y) => x < 0 || x >= Width || y < 0 || y >= Height;

    public void Text(int x, int y, string text) => Text(x, y, text, CurrentForegroundColor, CurrentBackgroundColor);
    public void Text(int x, int y, string text, QuickConsoleColor color) =>
        Text(x, y, text, color, CurrentBackgroundColor);
    public void Text(int x, int y, string text, QuickConsoleColor color, QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        var textArray = text.ToCharArray();
        var idx = x + y * Width;
        var maxLength = Math.Min(textArray.Length, Width - x);
        for (var i = 0; i < maxLength; i++)
        {
            var cell = new ConsoleBufferCell
            {
                Character = textArray[i],
                Foreground = color,
                Background = background,
            };
        }
    }

    public void Char(int x, int y, char c) => Char(x, y, c, CurrentForegroundColor, CurrentBackgroundColor);
    public void Char(int x, int y, char c, QuickConsoleColor color) =>
        Char(x, y, c, color, CurrentBackgroundColor);
    public void Char(int x, int y, char c, QuickConsoleColor color, QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        Cells[x + y * Width] = new ConsoleBufferCell
        {
            Character = c,
            Foreground = color,
            Background = background,
        };
    }

    public void Rectangle(int x, int y, int width, int height, char c) =>
        Rectangle(x, y, width, height, c, CurrentForegroundColor, CurrentBackgroundColor);
    public void Rectangle(int x, int y, int width, int height, char c, QuickConsoleColor color) =>
        Rectangle(x, y, width, height, c, color, CurrentBackgroundColor);
    public void Rectangle(int x, int y, int width, int height, char c, QuickConsoleColor color, QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        var rowIdx = y * Width;
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                var idx = j + x + rowIdx;
                if (idx >= Cells.Length) break;
                Cells[idx] = new ConsoleBufferCell
                {
                    Character = c,
                    Foreground = color,
                    Background = background,
                };
            }
            rowIdx += Width;
        }
    }

    public void Box(int x, int y, int width, int height, char c) =>
        Box(x, y, width, height, c, c, c, CurrentForegroundColor, CurrentBackgroundColor);
    public void Box(int x, int y, int width, int height, char c, QuickConsoleColor color) =>
        Box(x, y, width, height, c, c, c, color, CurrentBackgroundColor);
    public void Box(int x, int y, int width, int height, char c, QuickConsoleColor color, QuickConsoleColor background) =>
        Box(x, y, width, height, c, c, c, color, background);
    public void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner) =>
        Box(x, y, width, height, cSides, cTopBottom, cCorner, CurrentForegroundColor, CurrentBackgroundColor);
    public void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner, QuickConsoleColor color) =>
        Box(x, y, width, height, cSides, cTopBottom, cCorner, color, CurrentBackgroundColor);
    public void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner, QuickConsoleColor color,
        QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        var rowIdx = y * Width;
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
                var idx = j + x + rowIdx;
                Cells[idx] = new ConsoleBufferCell
                {
                    Character = ch,
                    Foreground = color,
                    Background = background,
                };
            }

            rowIdx += Width;
        }
    }

    public void Line(int x, int y, int length, LineDirection direction, char c) =>
        Line(x, y, length, direction, c, CurrentForegroundColor, CurrentBackgroundColor);
    public void Line(int x, int y, int length, LineDirection direction, char c, QuickConsoleColor color) =>
        Line(x, y, length, direction, c, color, CurrentBackgroundColor);
    public void Line(int x, int y, int length, LineDirection direction, char c, QuickConsoleColor color, QuickConsoleColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        var inc = direction == LineDirection.Horizontal ? 1 : Width;
        var idx = x + y * Width;
        var maxLength = Math.Min(length, direction== LineDirection.Horizontal ? Width - x : Width - y);
        for (var i = 0; i < maxLength; i++)
        {
            if (idx >= Cells.Length) break;
            Cells[idx] = new ConsoleBufferCell
            {
                Character = c,
                Foreground = color,
                Background = background,
            };
            idx += inc;
        }
    }

    public ConsoleBufferCell GetCellAt(int x, int y)
    {
        if (IsOutOfBounds(x, y)) throw new ArgumentException("X and/or Y out of bounds of console buffer");
        return Cells[x + y * Width];
    }

    public string GetStringAt(int x, int y, int length)
    {
        if (IsOutOfBounds(x, y)) return string.Empty;
        var idx = x + y * Width;
        return new string(Cells[idx..(idx + length)].Select(x => x.Character).ToArray());
    }

    public void Draw(int x, int y, IConsoleBuffer buffer, bool zeroCharIsTransparent)
    {
        if (IsOutOfBounds(x, y)) return;
        var idx = x + y * Width;
        var maxWidth = Math.Min(Width - x, buffer.Width);
        for (var bufferIdx = 0; bufferIdx < buffer.Cells.Length; bufferIdx++)
        {
            var bufferX = bufferIdx % buffer.Width;
            if (bufferX == 0 && bufferIdx > 0)
                idx += Width - buffer.Width;
            var sourceCell = buffer.Cells[bufferIdx];
            if (bufferX < maxWidth && idx < Cells.Length && (zeroCharIsTransparent == false || sourceCell.Character > 0))
                Cells[idx] = sourceCell;
            idx++;
        }
    }
}