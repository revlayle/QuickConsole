namespace RevLayle.QuickConsole;


/// <summary>
/// Represents the eight basic console of an ANSI-based console, plus a "default" value that can be overridden
/// </summary>
public enum AnsiColor
{
    /// <summary>
    /// AnsiColor.Default is a non-color value.  ConsoleBuffer will replace cells with this value with the buffer's
    /// current foreground or background color respectively.
    /// </summary>
    Default = 0,
    Black = 1,
    Red = 2, 
    Green = 3,
    Yellow = 4,
    Blue = 5,
    Magenta = 6,
    Cyan = 7,
    White = 8,
}