namespace RevLayle;

public class ConsoleBufferData(int length) : IConsoleBufferData
{
    public char[] Chars { get; } = new char[length]; 
    public QuickConsoleColor[] ForegroundColors { get; } = new QuickConsoleColor[length];
    public QuickConsoleColor[] BackgroundColors { get; } = new QuickConsoleColor[length];
    public int Length => length;
}