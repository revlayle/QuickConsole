namespace RevLayle;

public interface IConsoleBufferData
{
    char[] Chars { get; }
    QuickConsoleColor[] ForegroundColors { get; }
    QuickConsoleColor[] BackgroundColors { get; }
    int Length { get; }
}