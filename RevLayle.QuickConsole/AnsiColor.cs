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
    /// <summary>
    /// Black
    /// </summary>
    Black = 1,
    /// <summary>
    /// Red
    /// </summary>
    Red = 2,
    /// <summary>
    /// Green
    /// </summary>
    Green = 3,
    /// <summary>
    /// Yellow
    /// </summary>
    Yellow = 4,
    /// <summary>
    /// Blue
    /// </summary>
    Blue = 5,
    /// <summary>
    /// Magenta
    /// </summary>
    Magenta = 6,
    /// <summary>
    /// Cyan
    /// </summary>
    Cyan = 7,
    /// <summary>
    /// White
    /// </summary>
    White = 8,
}