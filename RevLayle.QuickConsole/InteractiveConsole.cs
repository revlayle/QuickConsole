namespace RevLayle.QuickConsole;

/// <summary>
/// An IConsoleBuffer compatible class that can read data from an instance of ISystemConsole.
/// </summary>
public class InteractiveConsole : IConsoleBuffer
{
    /// <summary>
    /// Creates an <see cref="InteractiveConsole"/> instance (with the provided width and height) that uses DotNetSystemConsole
    /// implementation of <see cref="ISystemConsole"/>.  This allows rendering to happen to an actual terminal.
    /// </summary>
    /// <param name="width">Width of buffer, does not have to be the same as the actual width of the console.</param>
    /// <param name="height">Height of buffer, does not have to be the same as the actual height of the console.</param>
    /// <returns>A new <see cref="InteractiveConsole"/></returns>
    public static InteractiveConsole FromSystemConsole(int width = 80, int height = 25) =>
        new(new DotNetSystemConsole(), width, height);
    
    private readonly IConsoleBuffer _buffer;
    private readonly ISystemConsole _systemConsole;

    /// <summary>
    /// Creates an <see cref="InteractiveConsole"/> instance (with the provided width and height) that uses a provided implementation of
    /// <see cref="ISystemConsole"/>.  This allows rendering to happen to an actual terminal.
    /// </summary>
    /// <param name="systemConsole">An instance implementing <see cref="ISystemConsole"/>.</param>
    /// <param name="width">Width of buffer, does not have to be the same as the actual width of the console.</param>
    /// <param name="height">Height of buffer, does not have to be the same as the actual height of the console.</param>
    /// <returns>A new <see cref="InteractiveConsole"/></returns>
    public InteractiveConsole(ISystemConsole systemConsole, int width = 80, int height = 25)
    {
        _systemConsole = systemConsole;
        _systemConsole.CursorVisible = false;
        _buffer = new ConsoleBuffer(width, height);
    }

    /// <summary>
    /// Renders the current state of the <see cref="InteractiveConsole"/>'s buffer to it's
    /// <see cref="ISystemConsole"/>'s <see cref="ISystemConsole.Out"/> property.
    /// </summary>
    public void Update() => _buffer.WriteBuffer(_systemConsole.Out);

    /// <summary>
    /// Get the value of the <see cref="InteractiveConsole"/>'s buffer to it's
    /// <see cref="ISystemConsole"/>'s <see cref="ISystemConsole.KeyAvailable"/> property.
    /// </summary>
    public bool KeyAvailable => _systemConsole.KeyAvailable;

    /// <summary>
    /// Calls the <see cref="InteractiveConsole"/>'s buffer to it's
    /// <see cref="ISystemConsole"/>'s <see cref="ISystemConsole.ReadKey()"/> method.
    /// </summary>
    /// <returns>A ConsoleKeyInfo value of the key read</returns>
    public ConsoleKeyInfo ReadKey() => _systemConsole.ReadKey();

    // IConsoleBuffer implementations
    /// <summary>
    /// Gets the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Height"/>
    /// </summary>
    public int Height => _buffer.Height;

    /// <summary>
    /// Gets the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Width"/>
    /// </summary>
    public int Width => _buffer.Width;

    /// <summary>
    /// Gets the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Cells"/>
    /// </summary>
    public ConsoleBufferCell[] Cells => _buffer.Cells;

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.IsOutOfBounds(int, int)"/>
    /// </summary>
    public bool IsOutOfBounds(int x, int y) => _buffer.IsOutOfBounds(x, y);
    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.IsOutOfBounds(int, int, int, int)"/>
    /// </summary>
    public bool IsOutOfBounds(int x, int y, int width, int height) => _buffer.IsOutOfBounds(x, y, width, height);
    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.IsFullyInBounds(int, int, int, int)"/>
    /// </summary>
    public bool IsFullyInBounds(int x, int y, int width, int height) => _buffer.IsFullyInBounds(x, y, width, height);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.WriteBuffer(TextWriter)"/>
    /// </summary>
    public void WriteBuffer(TextWriter textWriter) => _buffer.WriteBuffer(textWriter);

    /// <summary>
    /// Gets the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.CurrentForegroundColor"/>
    /// </summary>
    public AnsiColor CurrentForegroundColor
    {
        get => _buffer.CurrentForegroundColor;
        set => _buffer.CurrentForegroundColor = value;
    }

    /// <summary>
    /// Gets the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.CurrentBackgroundColor"/>
    /// </summary>
    public AnsiColor CurrentBackgroundColor
    {
        get => _buffer.CurrentBackgroundColor;
        set => _buffer.CurrentBackgroundColor = value;
    }

    /// <summary>
    /// Reads text interactively from the console represented by the internal <see cref="ISystemConsole"/> implementation.
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

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Text(int, int, string)"/>
    /// </summary>
    public void Text(int x, int y, string text) => _buffer.Text(x, y, text);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Text(int, int, string, AnsiColor)"/>
    /// </summary>
    public void Text(int x, int y, string text, AnsiColor color) => _buffer.Text(x, y, text, color);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Text(int, int, string, AnsiColor, AnsiColor)"/>
    /// </summary>
    public void Text(int x, int y, string text, AnsiColor color, AnsiColor background) =>
        _buffer.Text(x, y, text, color, background);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Cell(int, int, ConsoleBufferCell)"/>
    /// </summary>
    public void Cell(int x, int y, ConsoleBufferCell cell) => _buffer.Cell(x, y, cell);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Rectangle(int, int, int, int, ConsoleBufferCell)"/>
    /// </summary>
    public void Rectangle(int x, int y, int width, int height, ConsoleBufferCell cell) =>
        _buffer.Rectangle(x, y, width, height, cell);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Box(int, int, int, int, ConsoleBufferCell)"/>
    /// </summary>
    public void Box(int x, int y, int width, int height, ConsoleBufferCell cell) =>
        _buffer.Box(x, y, width, height, cell);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Box(int, int, int, int, ConsoleBufferCell, ConsoleBufferCell, ConsoleBufferCell)"/>
    /// </summary>
    public void Box(int x, int y, int width, int height, ConsoleBufferCell cellSides, ConsoleBufferCell cellTopBottom,
        ConsoleBufferCell cellCorners) =>
        _buffer.Box(x, y, width, height, cellSides, cellTopBottom, cellCorners);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Line(int, int, int, LineDirection, ConsoleBufferCell)"/>
    /// </summary>
    public void Line(int x, int y, int length, LineDirection direction, ConsoleBufferCell cell)
        => _buffer.Line(x, y, length, direction, cell);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.GetCellAt(int, int)"/>
    /// </summary>
    public ConsoleBufferCell GetCellAt(int x, int y) => _buffer.GetCellAt(x, y);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.GetStringAt(int, int, int)"/>
    /// </summary>
    public string GetStringAt(int x, int y, int length) => _buffer.GetStringAt(x, y, length);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Draw(int, int, IConsoleBuffer)"/>
    /// </summary>
    public void Draw(int x, int y, IConsoleBuffer buffer)
        => _buffer.Draw(x, y, buffer);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Scroll(int, int)"/>
    /// </summary>
    public void Scroll(int xd, int yd) => _buffer.Scroll(xd, yd);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Flip(bool, bool)"/>
    /// </summary>
    public void Flip(bool horizontal, bool vertical) => _buffer.Flip(horizontal, vertical);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Rotate(int, int, int, bool)"/>
    /// </summary>
    public void Rotate(int x, int y, int width, bool clockWise) => _buffer.Rotate(x, y, width, clockWise);

    /// <summary>
    /// Calls the internal implementation of the <see cref="InteractiveConsole"/>'s <see cref="IConsoleBuffer"/>.<see cref="IConsoleBuffer.Copy(int, int, int, int)"/>
    /// </summary>
    public IConsoleBuffer Copy(int x, int y, int width, int height) => _buffer.Copy(x, y, width, height);
}