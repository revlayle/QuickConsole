namespace RevLayle.QuickConsole;

/// <summary>
/// An IConsoleBuffer compatible class that can read data from an instance of ISystemConsole.
/// </summary>
public class InteractiveConsole : IConsoleBuffer
{
    /// <summary>
    /// Creates an <see cref="InteractiveConsole">InteractiveConsole</see> instance (with the provided width and height) that uses DotNetSystemConsole
    /// implementation of <see cref="ISystemConsole">ISystemConsole</see>.  This allows rendering to happen to an actual terminal.
    /// </summary>
    /// <param name="width">Width of buffer, does not have to be the same as the actual width of the console.</param>
    /// <param name="height">Height of buffer, does not have to be the same as the actual height of the console.</param>
    /// <returns>A new <see cref="InteractiveConsole">InteractiveConsole</see></returns>
    public static InteractiveConsole FromSystemConsole(int width = 80, int height = 25) => 
        new(new DotNetSystemConsole(), width, height);
    
    private readonly ISystemConsole _systemConsole;
    
    /// <summary>
    /// Creates an <see cref="InteractiveConsole">InteractiveConsole</see> instance (with the provided width and height) that uses a provided implementation of
    /// <see cref="ISystemConsole">ISystemConsole</see>.  This allows rendering to happen to an actual terminal.
    /// </summary>
    /// <param name="width">Width of buffer, does not have to be the same as the actual width of the console.</param>
    /// <param name="height">Height of buffer, does not have to be the same as the actual height of the console.</param>
    /// <returns>A new <see cref="InteractiveConsole">InteractiveConsole</see></returns>
    public InteractiveConsole(ISystemConsole systemConsole, int width = 80, int height = 25)
    { 
        _systemConsole = systemConsole;
        _systemConsole.CursorVisible = false;
        _buffer = new ConsoleBuffer(width, height);
    }

    private IConsoleBuffer _buffer;
    
    /// <summary>
    /// Renders the current state of the <see cref="InteractiveConsole">InteractiveConsole</see>'s buffer to it's
    /// <see cref="ISystemConsole">ISystemConsole</see>'s <see cref="ISystemConsole.Out">Out</see> property.
    /// </summary>
    public void Update() => _buffer.WriteBuffer(_systemConsole.Out);
    /// <summary>
    /// Get the value of the <see cref="InteractiveConsole">InteractiveConsole</see>'s buffer to it's
    /// <see cref="ISystemConsole">ISystemConsole</see>'s <see cref="ISystemConsole.KeyAvailable">KeyAvailable</see> property.
    /// </summary>
    public bool KeyAvailable => _systemConsole.KeyAvailable;
    /// <summary>
    /// Calls the <see cref="InteractiveConsole">InteractiveConsole</see>'s buffer to it's
    /// <see cref="ISystemConsole">ISystemConsole</see>'s <see cref="ISystemConsole.ReadKey">ReadKey</see> method.
    /// </summary>
    /// <returns>A ConsoleKeyInfo value of the key read</returns>
    public ConsoleKeyInfo ReadKey() => _systemConsole.ReadKey();

    // IConsoleBuffer implementations
    public int Height => _buffer.Height;
    public int Width => _buffer.Width;
    public ConsoleBufferCell[] Cells => _buffer.Cells;
    public bool IsOutOfBounds(int x, int y) => _buffer.IsOutOfBounds(x, y);

    public void WriteBuffer(TextWriter textWriter) => _buffer.WriteBuffer(textWriter);

    public AnsiColor CurrentForegroundColor
    {
        get => _buffer.CurrentForegroundColor;
        set => _buffer.CurrentForegroundColor = value;
    }
    public AnsiColor CurrentBackgroundColor
    {
        get => _buffer.CurrentBackgroundColor;
        set => _buffer.CurrentBackgroundColor = value;
    }
    /// <summary>
    /// Reads text interactively from the console represented by the internal <see cref="ISystemConsole">ISystemConsole</see> implementation.
    /// The result is kept in the console buffer and as a return value.  Backspace key is recognized to delete
    /// characters input. The enter key read from the input signals the end of input and the string is returned.
    /// </summary>
    /// <param name="x">X position in the terminal and console buffer to output text being read from the input</param>
    /// <param name="y">Y position in the terminal and console buffer to output text being read from the input</param>
    /// <param name="maxLength">Maximum length of string to read. Once the cursor of the console reaches this length
    /// or the width of the console buffer, no more keys are read for the string.</param>
    /// <returns>The string read (and output to the buffer) until enter is pressed.</returns>
    public string ReadText(int x, int y, int maxLength)
    {
        if (_buffer.IsOutOfBounds(x, y)) return string.Empty;
        var actualLength = 0;
        var actualMaxLength = Math.Min(_buffer.Width - x, maxLength);
        _systemConsole.CursorVisible = true;

        ConsoleKeyInfo info;
        _systemConsole.SetCursorPosition(x, y);
        while ((info = _systemConsole.ReadKey()).Key != ConsoleKey.Enter)
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
            else if (actualLength >= actualMaxLength)
                continue;
            else if (char.IsControl(info.KeyChar))
                continue;
            else
            {
                _buffer.Cell(x + actualLength, y, ConsoleBufferCell.FromChar(info.KeyChar));
                actualLength++;
            }

            _systemConsole.CursorVisible = false;
            Update();
            _systemConsole.CursorVisible = true;
            _systemConsole.SetCursorPosition(x + actualLength, y);
        }

        _systemConsole.CursorVisible = false;
        return GetStringAt(x, y, actualLength);
    }

    public void Text(int x, int y, string text) => _buffer.Text(x, y, text);
    public void Text(int x, int y, string text, AnsiColor color) => _buffer.Text(x, y, text, color);
    public void Text(int x, int y, string text, AnsiColor color, AnsiColor background) =>
        _buffer.Text(x, y, text, color, background);
    public void Cell(int x, int y, ConsoleBufferCell cell) => _buffer.Cell(x, y, cell);
    public void Rectangle(int x, int y, int width, int height, ConsoleBufferCell cell) =>
        _buffer.Rectangle(x, y, width, height, cell);
    public void Box(int x, int y, int width, int height, ConsoleBufferCell cell) => _buffer.Box(x, y, width, height, cell);
    public void Box(int x, int y, int width, int height, ConsoleBufferCell cellSides, ConsoleBufferCell cellTopBottom,
        ConsoleBufferCell cellCorners) =>
        _buffer.Box(x, y, width, height, cellSides, cellTopBottom, cellCorners);

    public void Line(int x, int y, int length, LineDirection direction, ConsoleBufferCell cell)
        => _buffer.Line(x, y, length, direction, cell);
    public ConsoleBufferCell GetCellAt(int x, int y) => _buffer.GetCellAt(x, y);
    public string GetStringAt(int x, int y, int length) => _buffer.GetStringAt(x, y, length);
    public void Draw(int x, int y, IConsoleBuffer buffer)
        => _buffer.Draw(x, y, buffer);
    public void Scroll(int xd, int yd) => _buffer.Scroll(xd, yd);
    public void Flip(bool horizontal, bool vertical) => _buffer.Flip(horizontal, vertical);
    public void Rotate(int x, int y, int width, bool clockWise) => _buffer.Rotate(x, y, width, clockWise);
    public IConsoleBuffer Copy(int x, int y, int width, int height) => _buffer.Copy(x, y, width, height);
}