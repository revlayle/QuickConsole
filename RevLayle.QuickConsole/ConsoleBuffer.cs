using System.Text;

namespace RevLayle.QuickConsole;

public class ConsoleBuffer(int width, int height) : IConsoleBuffer
{
    //private readonly IConsoleBufferData _buffer = new ConsoleBufferData(width * height);

    public AnsiColor CurrentForegroundColor { get; set; } = AnsiColor.White;
    public AnsiColor CurrentBackgroundColor { get; set; } = AnsiColor.Black;

    public ConsoleBufferCell[] Cells { get; } = new ConsoleBufferCell[width * height];

    public int Width { get; } = width;
    public int Height  { get; } = height;

    public void WriteBuffer(TextWriter textWriter)
    {
        var currentForeground = CurrentForegroundColor == AnsiColor.Default ? AnsiColor.Black : CurrentForegroundColor;
        var currentBackground = CurrentBackgroundColor == AnsiColor.Default ? AnsiColor.Black : CurrentBackgroundColor;
        var builder = new StringBuilder();
        var prevForegroundColor = -1;
        var prevBackgroundColor = -1;
        builder.Append("\x1b[1;1H");
        for (var i = 0; i < Cells.Length; i++)
        {
            var cell = Cells[i];
            if (i > 0 && i % Width == 0)
                builder.Append('\n');
            var foreground = cell.Foreground == AnsiColor.Default ? currentForeground : cell.Foreground;
            var background = cell.Background == AnsiColor.Default ? currentBackground : cell.Background;
            if ((int) foreground != prevForegroundColor)
            {
                prevForegroundColor = (int) foreground;
                builder.Append($"\x1b[{30 + prevForegroundColor - 1}m");
            }
            if ((int) background != prevBackgroundColor)
            {
                prevBackgroundColor = (int) background;
                builder.Append($"\x1b[{40 + prevBackgroundColor - 1}m");
            }
            builder.Append(char.IsControl(cell.Character) ? ' ' : cell.Character);
        }
        textWriter.Write(builder.ToString());
        textWriter.Flush();
    }
    
    public bool IsOutOfBounds(int x, int y) => x < 0 || x >= Width || y < 0 || y >= Height;

    public void Text(int x, int y, string text) => Text(x, y, text, CurrentForegroundColor, CurrentBackgroundColor);
    public void Text(int x, int y, string text, AnsiColor color) =>
        Text(x, y, text, color, CurrentBackgroundColor);
    public void Text(int x, int y, string text, AnsiColor color, AnsiColor background)
    {
        if (IsOutOfBounds(x, y)) return;
        var textArray = text.ToCharArray();
        var idx = x + y * Width;
        var maxLength = Math.Min(textArray.Length, Width - x);
        for (var i = 0; i < maxLength; i++)
        {
            Cells[idx + i] = new ConsoleBufferCell
            {
                Character = textArray[i],
                Foreground = color,
                Background = background,
            };
        }
    }
    
    public void Cell(int x, int y, ConsoleBufferCell cell)
    {
        if (IsOutOfBounds(x, y)) return;
        Cells[x + y * Width] = cell;
    }

    public void Rectangle(int x, int y, int width, int height, ConsoleBufferCell cell)
    {
        if (IsOutOfBounds(x, y)) return;
        cell.OverrideDefaults(CurrentForegroundColor, CurrentBackgroundColor);
        var actualWidth = Math.Min(Width - x, width);
        var actualHeight = Math.Min(Height - y, height);
        var rowIdx = y * Width;
        for (var i = 0; i < actualHeight; i++)
        {
            for (var j = 0; j < actualWidth; j++)
            {
                var idx = j + x + rowIdx;
                Cells[idx] = cell;
            }
            rowIdx += Width;
        }
    }
    
    public void Box(int x, int y, int width, int height, ConsoleBufferCell cell) =>
        Box(x, y, width, height, cell, cell, cell);
    public void Box(int x, int y, int width, int height, ConsoleBufferCell cellSides, ConsoleBufferCell cellTopBottom, ConsoleBufferCell cellCorner)
    {
        if (IsOutOfBounds(x, y)) return;
        cellSides = cellSides.OverrideDefaults(CurrentForegroundColor, CurrentBackgroundColor);
        cellTopBottom = cellTopBottom.OverrideDefaults(CurrentForegroundColor, CurrentBackgroundColor);
        cellCorner = cellCorner.OverrideDefaults(CurrentForegroundColor, CurrentBackgroundColor);
        var rowIdx = y * Width;
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                var cell = (i, j) switch
                {
                    (0, 0) => cellCorner,
                    (0, var tj) when tj == width - 1 => cellCorner,
                    var (ti, tj) when tj == width - 1 && ti == height - 1 => cellCorner,
                    (var ti, 0) when ti == height - 1 => cellCorner,
                    (0, _) => cellTopBottom,
                    (_, 0) => cellSides,
                    var (ti, _) when ti == height - 1 => cellTopBottom,
                    var (_, tj) when tj == width - 1 => cellSides,
                    _ => ConsoleBufferCell.Zero,
                };
                if (cell == ConsoleBufferCell.Zero)
                    continue;
                if ((j + x) >= Width || (i + y) >= Height)
                    continue;
                var idx = j + x + rowIdx;
                Cells[idx] = cell;
            }

            rowIdx += Width;
        }
    }
    
    public void Line(int x, int y, int length, LineDirection direction, ConsoleBufferCell cell)
    {
        if (IsOutOfBounds(x, y)) return;
        cell = cell.OverrideDefaults(CurrentForegroundColor, CurrentBackgroundColor);
        var inc = direction == LineDirection.Horizontal ? 1 : Width;
        var idx = x + y * Width;
        var maxLength = Math.Min(length, direction == LineDirection.Horizontal ? Width - x : Width - y);
        for (var i = 0; i < maxLength; i++)
        {
            Cells[idx] = cell;
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
        return new string(Cells[idx..(idx + length)].Select(cell => cell.Character)
            .ToArray()).Trim((char)0);
    }

    public void Draw(int x, int y, IConsoleBuffer buffer)
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
            if (bufferX < maxWidth && idx < Cells.Length && sourceCell.Character > 0)
                Cells[idx] = sourceCell;
            idx++;
        }
    }

    public void Scroll(int xd, int yd)
    {
        if (xd != 0)
        {
            var axd = Math.Abs(xd);
            var count = Height * axd;
            var tempBuffer = new ConsoleBufferCell[count];
            for (int i = 0, x = 0; i < Cells.Length; i += Width, x += axd)
                Array.Copy(Cells, i + (xd < 0 ? 0 : Width - axd), tempBuffer, x, axd);
            Array.Copy(Cells, xd < 0 ? axd : 0, Cells, xd < 0 ? 0 : axd, Cells.Length - axd);
            for (int i = 0, x = 0; i < Cells.Length; i += Width, x += axd)
                Array.Copy(tempBuffer, x, Cells, i + (xd < 0 ? Width - axd : 0), axd);
        }

        if (yd != 0)
        {
            var count = Math.Abs(Width * yd);
            var tempBuffer = new ConsoleBufferCell[count];
            Array.Copy(Cells, yd < 0 ? 0 : Cells.Length - count, tempBuffer, 0, count);
            Array.Copy(Cells, yd < 0 ? count : 0, Cells, yd < 0 ? 0 : count, Cells.Length - count);
            Array.Copy(tempBuffer, 0, Cells, yd < 0 ? Cells.Length - count : 0, count);
        }
    }

    public void Flip(bool horizontal, bool vertical)
    {
        var tempBuffer = new ConsoleBufferCell[Width];
        for (int i = 0, j = Cells.Length - Width; i < Cells.Length; i += Width, j -= Width)
        {
            if (vertical && i < j)
            {
                Array.Copy(Cells, i, tempBuffer, 0, Width);
                Array.Copy(Cells, j, Cells, i, Width);
                Array.Copy(tempBuffer, 0, Cells, j, Width);
            }
            if (horizontal)
                Array.Reverse(Cells, i, Width);
        }
    }

    public void Rotate(int x, int y, int width, bool clockWise)
    {
        if (IsOutOfBounds(x, y)) return;
        var rotateCopy = Copy(x, y, width, width);
        if (rotateCopy.Width != rotateCopy.Height) return;
        var sourceOffset = 0;
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < width; j++)
            {
                var offset = clockWise ? (width - i - 1 + x) + (j + y) * Width : x + i + (width - j - 1 + y) * Width;
                Cells[offset] = rotateCopy.Cells[sourceOffset];
                sourceOffset++;
            }
        }
    }

    public IConsoleBuffer Copy(int x, int y, int width, int height)
    {
        if (IsOutOfBounds(x, y))
            throw new ArgumentException("X and/or Y out of bounds of console buffer");

        var actualWidth = Math.Min(width, Width - x);
        var actualHeight = Math.Min(height, Height - y);

        var copy = new ConsoleBuffer(actualWidth, actualHeight);
        var sourceIdx = x + y * Width;
        var copyIdx = 0;
        for (var i = 0; i < actualHeight; i++)
        {
            Array.Copy(Cells, sourceIdx, copy.Cells, copyIdx, actualWidth);
            copyIdx += actualWidth;
            sourceIdx += Width;
        }
        return copy;
    }
}