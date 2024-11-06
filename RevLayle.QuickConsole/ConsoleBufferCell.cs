namespace RevLayle;

public struct ConsoleBufferCell
{
    public char Character { get; set; }
    public QuickConsoleColor Foreground { get; set; }
    public QuickConsoleColor Background { get; set; }
    
    public ConsoleBufferCell WithCharacter(char c) => this with { Character = c };
    public ConsoleBufferCell WithForeground(QuickConsoleColor color) => this with { Foreground = color };
    public ConsoleBufferCell WithBackground(QuickConsoleColor color) => this with { Background = color };
}