namespace RevLayle.QuickConsole;

public interface IConsoleBuffer
{
    ConsoleBufferCell[] Cells { get; }
    int Width { get; }
    int Height { get; }
    AnsiColor CurrentForegroundColor { get; set; }
    AnsiColor CurrentBackgroundColor { get; set; }
    void WriteBuffer(TextWriter textWriter);
    bool IsOutOfBounds(int x, int y);
    void Text(int x, int y, string text);
    void Text(int x, int y, string text, AnsiColor color);
    void Text(int x, int y, string text, AnsiColor color, AnsiColor background);
    void Cell(int x, int y, ConsoleBufferCell cell);
    void Rectangle(int x, int y, int width, int height, ConsoleBufferCell cell);
    void Box(int x, int y, int width, int height, ConsoleBufferCell cell);
    void Box(int x, int y, int width, int height, ConsoleBufferCell cellSides, ConsoleBufferCell cellTopBottom, ConsoleBufferCell cellCorner);
    void Line(int x, int y, int length, LineDirection direction, ConsoleBufferCell cell);
    ConsoleBufferCell GetCellAt(int x, int y);
    string GetStringAt(int x, int y, int length);
    void Draw(int x, int y, IConsoleBuffer buffer);
    void Scroll(int xd, int yd);
    void Flip(bool horizontal, bool vertical);
    void Rotate(int x, int y, int width, bool clockWise);
    IConsoleBuffer Copy(int x, int y, int width, int height);
}