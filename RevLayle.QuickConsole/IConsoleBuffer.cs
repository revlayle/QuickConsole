namespace RevLayle.QuickConsole;

/// <summary>
/// Interface the describes the data and actions you can do to a console buffer.
/// </summary>
public interface IConsoleBuffer
{
    /// <summary>
    /// Cells in the console buffer
    /// </summary>
    ConsoleBufferCell[] Cells { get; }
    /// <summary>
    /// Width of the buffer in cells
    /// </summary>
    int Width { get; }
    /// <summary>
    /// Height of the buffer in cells
    /// </summary>
    int Height { get; }
    /// <summary>
    /// Current foreground color of buffer
    /// </summary>
    AnsiColor CurrentForegroundColor { get; set; }
    /// <summary>
    /// Current background color of buffer
    /// </summary>
    AnsiColor CurrentBackgroundColor { get; set; }
    /// <summary>
    /// Write the buffer to a provided TextWriter
    /// </summary>
    /// <param name="textWriter">TextWriter buffer will be written to.</param>
    void WriteBuffer(TextWriter textWriter);
    /// <summary>
    /// Given an X and a Y position, is that position out-of-bounds?
    /// </summary>
    /// <param name="x">X position to check</param>
    /// <param name="y">Y position to check</param>
    /// <returns>True, position is out-of-bounds.  False, positions is in-bounds.</returns>
    bool IsOutOfBounds(int x, int y);
    /// <summary>
    /// Write text to the buffer at location X and Y
    /// </summary>
    /// <param name="x">X position to draw at</param>
    /// <param name="y">Y position to draw at</param>
    /// <param name="text">Text to draw</param>
    void Text(int x, int y, string text);
    /// <summary>
    /// Write text to the buffer at location X and Y, with a given foreground color
    /// </summary>
    /// <param name="x">X position to draw at</param>
    /// <param name="y">Y position to draw at</param>
    /// <param name="text">Text to draw</param>
    /// <param name="color">Foreground color to draw on the cells</param>
    void Text(int x, int y, string text, AnsiColor color);
    /// <summary>
    /// Write text to the buffer at location X and Y, with a given foreground and background color
    /// </summary>
    /// <param name="x">X position to draw at</param>
    /// <param name="y">Y position to draw at</param>
    /// <param name="text">Text to draw</param>
    /// <param name="color">Foreground color to draw on the cells</param>
    /// <param name="background">Background color to draw on the cells</param>
    void Text(int x, int y, string text, AnsiColor color, AnsiColor background);
    /// <summary>
    /// Set a single cell in a console buffer.
    /// </summary>
    /// <param name="x">X position to set</param>
    /// <param name="y">Y position to set</param>
    /// <param name="cell">A ConsoleBufferCell value to put in the cell</param>
    void Cell(int x, int y, ConsoleBufferCell cell);
    /// <summary>
    /// Draws a filled rectangle in the console buffer. 
    /// </summary>
    /// <param name="x">X position of the top-left corner of the rectangle</param>
    /// <param name="y">Y position of the top-left corner of the rectangle</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle</param>
    /// <param name="cell">The ConsoleBufferCell value to fill the rectangle with</param>
    void Rectangle(int x, int y, int width, int height, ConsoleBufferCell cell);
    /// <summary>
    /// Draws a box in the console buffer.
    /// </summary>
    /// <param name="x">X position of the top-left corner of the box</param>
    /// <param name="y">Y position of the top-left corner of the box</param>
    /// <param name="width">Width of the box.</param>
    /// <param name="height">Height of the box</param>
    /// <param name="cell">The ConsoleBufferCell value tto draw the box frame with</param>
    void Box(int x, int y, int width, int height, ConsoleBufferCell cell);
    /// <summary>
    /// Draws a box in the console buffer.
    /// </summary>
    /// <param name="x">X position of the top-left corner of the box</param>
    /// <param name="y">Y position of the top-left corner of the box</param>
    /// <param name="width">Width of the box.</param>
    /// <param name="height">Height of the box</param>
    /// <param name="cellSides">The ConsoleBufferCell value to draw the left and right box frame sides with</param>
    /// <param name="cellTopBottom">The ConsoleBufferCell value to draw the top and bottom box frame sides with</param>
    /// <param name="cellCorner">The ConsoleBufferCell value tto draw the box corners with</param>
    void Box(int x, int y, int width, int height, ConsoleBufferCell cellSides, ConsoleBufferCell cellTopBottom, ConsoleBufferCell cellCorner);
    /// <summary>
    /// Draws a horizontal or vertical line in the console buffer.
    /// </summary>
    /// <param name="x">X position of the start (top for vertical, left for horizontal) of the line</param>
    /// <param name="y">Y position of the start (top for vertical, left for horizontal) of the line</param>
    /// <param name="length">How many cells long the line will be drawn for.  Vertical lines draw down.
    /// Horizontal lines draw right.</param>
    /// <param name="direction">A valid LineDirection to indicate if the line is horizontal or vertical</param>
    /// <param name="cell">The ConsoleBufferCell value to draw the line with</param>
    void Line(int x, int y, int length, LineDirection direction, ConsoleBufferCell cell);
    /// <summary>
    /// Gets the ConsoleBufferCell value from the console buffer at position provided.
    /// </summary>
    /// <param name="x">X position of the cell to get the value for</param>
    /// <param name="y">X position of the cell to get the value for</param>
    /// <returns>ConsoleBufferCell value of the cell</returns>
    ConsoleBufferCell GetCellAt(int x, int y);
    /// <summary>
    /// Gets a text string from the console buffer.
    /// </summary>
    /// <param name="x">X position to get text from</param>
    /// <param name="y">Y position to get text from</param>
    /// <param name="length">How many cells to read.</param>
    /// <returns>Text string read from the buffer, sans any color information.</returns>
    string GetStringAt(int x, int y, int length);
    /// <summary>
    /// Draws an external buffer onto this buffer.
    /// </summary>
    /// <param name="x">X position to draw provided buffer</param>
    /// <param name="y">Y position to draw provided buffer</param>
    /// <param name="buffer">An external object, implementing IConsoleBuffer, that will be drawn onto this buffer</param>
    void Draw(int x, int y, IConsoleBuffer buffer);
    /// <summary>
    /// Scrolls a buffer horizontally and/or vertically.
    /// </summary>
    /// <param name="xd">The X-delta to scroll horizontally. Negative values will scroll left. Positive values scroll
    /// right. An X-delta of zero will not do any horizontal scrolling.</param>
    /// <param name="yd">The Y-delta to scroll vertically. Negative values will scroll up. Positive values scroll
    /// down. A Y-delta of zero will not do any vertical scrolling.</param>
    void Scroll(int xd, int yd);
    /// <summary>
    /// Mirrors the content of a console buffer horizontally and/or vertically
    /// </summary>
    /// <param name="horizontal">If true, mirror horizontally. If false, no horizontal mirroring is performed.</param>
    /// <param name="vertical">If true, mirror vertically. If false, no vertical mirroring is performed.</param>
    void Flip(bool horizontal, bool vertical);
    /// <summary>
    /// Rotate a square section of the console buffer 90 degrees clockwise or counter-clockwise.
    /// </summary>
    /// <param name="x">X position of the top-left square area to rotate</param>
    /// <param name="y">Y position of the top-left square area to rotate</param>
    /// <param name="width">Width (and height) of area to rotate</param>
    /// <param name="clockWise">If true, rotate 90 degrees clockwise. If false, rotate 90 degrees
    /// counter-clockwise.</param>
    void Rotate(int x, int y, int width, bool clockWise);
    /// <summary>
    /// Get a copy of a rectangular area of the console buffer
    /// </summary>
    /// <param name="x">X position of the top-left rectangular area to get a copy</param>
    /// <param name="y">Y position of the top-left rectangular area to get a copy</param>
    /// <param name="width">Width of the area to copy</param>
    /// <param name="height">Height of the area to copy</param>
    /// <returns>A new instance implementing IConsoleBuffer with the copies cells and the width and height
    /// provided.</returns>
    IConsoleBuffer Copy(int x, int y, int width, int height);
}