using System.Text;

namespace RevLayle.QuickConsole;

/// <summary>
/// An in-memory buffer representing an ANSI console
/// </summary>
/// <param name="width">Width of the buffer in cells</param>
/// <param name="height">Height of the buffer in cells</param>
public class ConsoleBuffer(int width, int height) : IConsoleBuffer
{
    /// <summary>
    /// Current foreground color of the console buffer. When rendering a buffer, if a cell has no defined foreground
    /// color (i.e. <see cref="AnsiColor.Default">AnsiColor.Default</see>), then this color is rendered.
    /// </summary>
    public AnsiColor CurrentForegroundColor { get; set; } = AnsiColor.White;
    /// <summary>
    /// Current background color of the console buffer. When rendering a buffer, if a cell has no defined background
    /// color (i.e. <see cref="AnsiColor.Default">AnsiColor.Default</see>), then this color is rendered.
    /// </summary>
    public AnsiColor CurrentBackgroundColor { get; set; } = AnsiColor.Black;

    /// <summary>
    /// All the cells in this console buffer. This array should always contain Width x Height elements.
    /// </summary>
    public ConsoleBufferCell[] Cells { get; } = new ConsoleBufferCell[width * height];

    /// <summary>
    /// Width of the buffer in cells
    /// </summary>
    public int Width { get; } = width;
    /// <summary>
    /// Height of the buffer in cells
    /// </summary>
    public int Height  { get; } = height;

    /// <summary>
    /// Output the console buffer using ANSI console string escapes to a TextWriter.
    /// </summary>
    /// <param name="textWriter">A text writer instance to render the buffer to</param>
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
    
    /// <summary>
    /// Given an X and a Y position, is that location considered out-of-bounds?  Out of bounds is either X or Y is
    /// zero or less, or, X is greater than or equal to Width or Y is greater than or equal to Height.
    /// </summary>
    /// <param name="x">X position to check</param>
    /// <param name="y">Y position to check</param>
    /// <returns>True, position is out-of-bounds.  False, positions is in-bounds.</returns>
    public bool IsOutOfBounds(int x, int y) => x < 0 || x >= Width || y < 0 || y >= Height;

    /// <summary>
    /// Write text to the buffer at location x/y.  If x or y is out-of-bounds, nothing is written.  If the length
    /// of the text go beyond the width of the console buffer, the text will be truncated.  The console's
    /// CurrentForegroundColor and CurrentBackgroundColor colors are used for the cell colors of the text.
    /// </summary>
    /// <param name="x">X position to draw at</param>
    /// <param name="y">Y position to draw at</param>
    /// <param name="text">Text to draw</param>
    public void Text(int x, int y, string text) => Text(x, y, text, CurrentForegroundColor, CurrentBackgroundColor);
    /// <summary>
    /// Write text to the buffer at location x/y.  If x or y is out-of-bounds, nothing is written.  If the length
    /// of the text go beyond the width of the console buffer, the text will be truncated.  The color provided will be
    /// foreground color of the drawn cells and CurrentBackgroundColor color are used for the cell background color.
    /// </summary>
    /// <param name="x">X position to draw at</param>
    /// <param name="y">Y position to draw at</param>
    /// <param name="text">Text to draw</param>
    /// <param name="color">Foreground color to draw on the cells</param>
    public void Text(int x, int y, string text, AnsiColor color) =>
        Text(x, y, text, color, CurrentBackgroundColor);
    /// <summary>
    /// Write text to the buffer at location x/y.  If x or y is out-of-bounds, nothing is written.  If the length
    /// of the text go beyond the width of the console buffer, the text will be truncated.  The provided color and
    /// background arguments set the drawn color of the cells.
    /// </summary>
    /// <param name="x">X position to draw at</param>
    /// <param name="y">Y position to draw at</param>
    /// <param name="text">Text to draw</param>
    /// <param name="color">Foreground color to draw on the cells</param>
    /// <param name="background">Background color to draw on the cells</param>
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
    
    /// <summary>
    /// Set a single cell in a console buffer.
    /// </summary>
    /// <param name="x">X position to set</param>
    /// <param name="y">Y position to set</param>
    /// <param name="cell">A ConsoleBufferCell value to put in the cell</param>
    public void Cell(int x, int y, ConsoleBufferCell cell)
    {
        if (IsOutOfBounds(x, y)) return;
        Cells[x + y * Width] = cell;
    }

    /// <summary>
    /// Draws a filled rectangle in the console buffer.  If the width and/or height go outside the boundary of the
    /// console buffer, the rectangle is simply truncated at the corresponding edges.
    /// </summary>
    /// <param name="x">X position of the top-left corner of the rectangle</param>
    /// <param name="y">Y position of the top-left corner of the rectangle</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle</param>
    /// <param name="cell">The ConsoleBufferCell value to fill the rectangle with</param>
    public void Rectangle(int x, int y, int width, int height, ConsoleBufferCell cell)
    {
        if (width <= 0 || height <= 0) return;
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
    
    /// <summary>
    /// Draws a box in the console buffer.  Only the frame is drawn, none of the cells inside the box are affected.
    /// If the width and/or height go outside the boundary of the console buffer, those edges are not drawn.
    /// </summary>
    /// <param name="x">X position of the top-left corner of the box</param>
    /// <param name="y">Y position of the top-left corner of the box</param>
    /// <param name="width">Width of the box.</param>
    /// <param name="height">Height of the box</param>
    /// <param name="cell">The ConsoleBufferCell value tto draw the box frame with</param>
    public void Box(int x, int y, int width, int height, ConsoleBufferCell cell) =>
        Box(x, y, width, height, cell, cell, cell);
    /// <summary>
    /// Draws a box in the console buffer.  Only the frame is drawn, none of the cells inside the box are affected.
    /// If the width and/or height go outside the boundary of the console buffer, those edges are not drawn.
    /// </summary>
    /// <param name="x">X position of the top-left corner of the box</param>
    /// <param name="y">Y position of the top-left corner of the box</param>
    /// <param name="width">Width of the box.</param>
    /// <param name="height">Height of the box</param>
    /// <param name="cellSides">The ConsoleBufferCell value to draw the left and right box frame sides with</param>
    /// <param name="cellTopBottom">The ConsoleBufferCell value to draw the top and bottom box frame sides with</param>
    /// <param name="cellCorner">The ConsoleBufferCell value tto draw the box corners with</param>
    public void Box(int x, int y, int width, int height, ConsoleBufferCell cellSides, ConsoleBufferCell cellTopBottom, ConsoleBufferCell cellCorner)
    {
        if (width <= 0 || height <= 0) return;
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
    
    /// <summary>
    /// Draws a horizontal or vertical line in the console buffer.  If the length of the line goes outside the boundary
    /// of the console buffer, the line is truncated.
    /// </summary>
    /// <param name="x">X position of the start (top for vertical, left for horizontal) of the line</param>
    /// <param name="y">Y position of the start (top for vertical, left for horizontal) of the line</param>
    /// <param name="length">How many cells long the line will be drawn for.  Vertical lines draw down.
    /// Horizontal lines draw right.</param>
    /// <param name="direction">A valid LineDirection to indicate if the line is horizontal or vertical</param>
    /// <param name="cell">The ConsoleBufferCell value to draw the line with</param>
    public void Line(int x, int y, int length, LineDirection direction, ConsoleBufferCell cell)
    {
        if (length <=0) return;
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

    /// <summary>
    /// Gets the ConsoleBufferCell value from the console buffer at position provided.
    /// </summary>
    /// <param name="x">X position of the cell to get the value for</param>
    /// <param name="y">X position of the cell to get the value for</param>
    /// <returns>ConsoleBufferCell value of the cell</returns>
    /// <exception cref="ArgumentException">X and/or Y value is out-of-bounds of the console buffer.</exception>
    public ConsoleBufferCell GetCellAt(int x, int y)
    {
        if (IsOutOfBounds(x, y)) throw new ArgumentException("X and/or Y out of bounds of console buffer");
        return Cells[x + y * Width];
    }

    /// <summary>
    /// Gets a text string from the console buffer.
    /// </summary>
    /// <param name="x">X position to get text from</param>
    /// <param name="y">Y position to get text from</param>
    /// <param name="length">How many cells to read.  If length go outside the boundary of the console buffer, read
    /// length is truncated.</param>
    /// <returns>Text string read from the buffer, sans any color information. Out-of-bounds coordinates return an
    /// empty string.  0 or negative length values return an empty string.  Returned strings with <c>\0</c> character values
    /// are trimmed from the string.</returns>
    public string GetStringAt(int x, int y, int length)
    {
        if (length <= 0) return string.Empty;
        if (IsOutOfBounds(x, y)) return string.Empty;
        var idx = x + y * Width;
        return new string(Cells[idx..(idx + length)].Select(cell => cell.Character)
            .ToArray()).Trim((char)0);
    }

    /// <summary>
    /// Draws an external buffer onto this buffer.  Any cells in the provided buffer that have a <c>\0</c> character value
    /// are considered "transparent" and will not draw onto this buffer.  If the width/height are beyond the boundary
    /// of this console buffer, the provided buffer is drawn truncated.
    /// </summary>
    /// <param name="x">X position to draw provided buffer</param>
    /// <param name="y">Y position to draw provided buffer</param>
    /// <param name="buffer">An external object, implementing IConsoleBuffer, that will be drawn onto this buffer</param>
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

    /// <summary>
    /// Scrolls a buffer horizontally and/or vertically.
    /// </summary>
    /// <param name="xd">The X-delta to scroll horizontally. Negative values will scroll left. Positive values scroll
    /// right. An X-delta of zero will not do any horizontal scrolling.</param>
    /// <param name="yd">The Y-delta to scroll vertically. Negative values will scroll up. Positive values scroll
    /// down. A Y-delta of zero will not do any vertical scrolling.</param>
    public void Scroll(int xd, int yd)
    {
        xd %= Width;
        yd %= Height;
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

    /// <summary>
    /// Mirrors the content of a console buffer horizontally and/or vertically
    /// </summary>
    /// <param name="horizontal">If true, mirror horizontally. If false, no horizontal mirroring is performed.</param>
    /// <param name="vertical">If true, mirror vertically. If false, no vertical mirroring is performed.</param>
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

    /// <summary>
    /// Rotate a square section of the console buffer 90 degrees clockwise or counter-clockwise.  If the square area
    /// is truncated because of boundaries and is no longer square, no rotation is done.
    /// </summary>
    /// <param name="x">X position of the top-left square area to rotate</param>
    /// <param name="y">Y position of the top-left square area to rotate</param>
    /// <param name="width">Width (and height) of area to rotate</param>
    /// <param name="clockWise">If true, rotate 90 degrees clockwise. If false, rotate 90 degrees
    /// counter-clockwise.</param>
    public void Rotate(int x, int y, int width, bool clockWise)
    {
        if (width <= 0) return;
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

    /// <summary>
    /// Get a copy of a rectangular area of the console buffer
    /// </summary>
    /// <param name="x">X position of the top-left rectangular area to get a copy</param>
    /// <param name="y">Y position of the top-left rectangular area to get a copy</param>
    /// <param name="width">Width of the area to copy</param>
    /// <param name="height">Height of the area to copy</param>
    /// <returns>A new instance implementing <see cref="IConsoleBuffer">IConsoleBuffer</see> with the copies cells and the width and height
    /// provided. Note, if the width and/or height go beyond the console buffer's boundaries, you will get a smaller
    /// buffer copy.</returns>
    /// <exception cref="ArgumentException">X and/or Y are out-of-bounds, or width and height are not both greater than
    /// zero.</exception>
    public IConsoleBuffer Copy(int x, int y, int width, int height)
    {
        if (width <= 0 || height <= 0) 
            throw new ArgumentException("Width and height must be greater than 0");
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