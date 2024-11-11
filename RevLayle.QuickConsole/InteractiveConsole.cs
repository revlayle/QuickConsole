namespace RevLayle.QuickConsole;

public class InteractiveConsole : IConsoleBuffer
{
    public static InteractiveConsole FromSystemConsole(int width = 80, int height = 25) => 
        new(new DotNetSystemConsole(), width, height);
    
    private readonly ISystemConsole _systemConsole;
    public InteractiveConsole(ISystemConsole systemConsole, int width = 80, int height = 25)
    { 
        _systemConsole = systemConsole;
        _systemConsole.CursorVisible = false;
        _buffer = new ConsoleBuffer(width, height);
    }

    private IConsoleBuffer _buffer;
    
    public void Update() => _buffer.WriteBuffer(_systemConsole.Out);
    public bool KeyAvailable => _systemConsole.KeyAvailable;
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
}