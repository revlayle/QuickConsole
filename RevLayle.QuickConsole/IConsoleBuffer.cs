using System.Drawing;

namespace RevLayle;

public interface IConsoleBuffer
{
    ConsoleBufferCell[] Cells { get; }
    int Width { get; }
    int Height { get; }
    QuickConsoleColor CurrentForegroundColor { get; set; }
    QuickConsoleColor CurrentBackgroundColor { get; set; }
    void WriteBuffer(TextWriter textWriter);
    bool IsOutOfBounds(int x, int y);
    void Text(int x, int y, string text);
    void Text(int x, int y, string text, QuickConsoleColor color);
    void Text(int x, int y, string text, QuickConsoleColor color, QuickConsoleColor background);
    void Cell(int x, int y, ConsoleBufferCell cell);
    void Rectangle(int x, int y, int width, int height, ConsoleBufferCell cell);
    void Box(int x, int y, int width, int height, ConsoleBufferCell cell);
    void Box(int x, int y, int width, int height, ConsoleBufferCell cellSides, ConsoleBufferCell cellTopBottom, ConsoleBufferCell cellCorner);
    void Line(int x, int y, int length, LineDirection direction, ConsoleBufferCell cell);
    ConsoleBufferCell GetCellAt(int x, int y);
    string GetStringAt(int x, int y, int length);
    void Draw(int x, int y, IConsoleBuffer buffer);
}