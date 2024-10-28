using System.Drawing;

namespace RevLayle;

public interface IConsoleBuffer
{
    void SetColor(QuickConsoleColor color);
    void SetBackgroundColor(QuickConsoleColor color);
    void Text(int x, int y, string text);
    void Text(int x, int y, string text, QuickConsoleColor color);
    void Text(int x, int y, string text, QuickConsoleColor color, QuickConsoleColor background);
    void Char(int x, int y, char c);
    void Char(int x, int y, char c, QuickConsoleColor color);
    void Char(int x, int y, char c, QuickConsoleColor color, QuickConsoleColor background);
    void Box(int x, int y, int width, int height, char c);
    void Box(int x, int y, int width, int height, char c, QuickConsoleColor color);
    void Box(int x, int y, int width, int height, char c, QuickConsoleColor color, QuickConsoleColor background);
    void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner);
    void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner, QuickConsoleColor color);
    void Box(int x, int y, int width, int height, char cSides, char cTopBottom, char cCorner, QuickConsoleColor color, QuickConsoleColor background);
    void Line(int x, int y, int length, LineDirection direction, char c);
    void Line(int x, int y, int length, LineDirection direction, char c, QuickConsoleColor color);
    void Line(int x, int y, int length, LineDirection direction, char c, QuickConsoleColor color, QuickConsoleColor background);
    char GetCharAt(int x, int y);
    string GetStringAt(int x, int y, int length);
    QuickConsoleColor GetColorAt(int x, int y);
    QuickConsoleColor GetBackgroundColorAt(int x, int y);
}