using System.Drawing;

namespace RevLayle;

public interface IConsoleBuffer
{
    //IConsoleBufferData Data { get; }
    ConsoleBufferCell[] Cells { get; }
    int Width { get; }
    int Height { get; }
    QuickConsoleColor CurrentForegroundColor { get; set; }
    QuickConsoleColor CurrentBackgroundColor { get; set; }
    void WriteBuffer(Stream stream);
    bool IsOutOfBounds(int x, int y);
    void Text(int x, int y, string text);
    void Text(int x, int y, string text, QuickConsoleColor color);
    void Text(int x, int y, string text, QuickConsoleColor color, QuickConsoleColor background);
    void Char(int x, int y, char c);
    void Char(int x, int y, char c, QuickConsoleColor color);
    void Char(int x, int y, char c, QuickConsoleColor color, QuickConsoleColor background);
    void Rectangle(int x, int y, int width, int height, char c);
    void Rectangle(int x, int y, int width, int height, char c, QuickConsoleColor color);
    void Rectangle(int x, int y, int width, int height, char c, QuickConsoleColor color, QuickConsoleColor background);
    void Box(int x, int y, int width, int height, char c);
    void Box(int x, int y, int width, int height, char c, QuickConsoleColor color);
    void Box(int x, int y, int width, int height, char c, QuickConsoleColor color, QuickConsoleColor background);
    void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner);
    void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner, QuickConsoleColor color);
    void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner, QuickConsoleColor color, QuickConsoleColor background);
    void Line(int x, int y, int length, LineDirection direction, char c);
    void Line(int x, int y, int length, LineDirection direction, char c, QuickConsoleColor color);
    void Line(int x, int y, int length, LineDirection direction, char c, QuickConsoleColor color, QuickConsoleColor background);
    ConsoleBufferCell GetCellAt(int x, int y);
    string GetStringAt(int x, int y, int length);
    void Draw(int x, int y, IConsoleBuffer buffer, bool zeroCharIsTransparent);
}