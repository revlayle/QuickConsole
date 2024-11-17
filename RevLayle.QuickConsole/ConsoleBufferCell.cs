namespace RevLayle.QuickConsole;

/// <summary>
/// Value type describing the character of a console buffer cell, along with its foreground and background colors.
/// </summary>
public struct ConsoleBufferCell : IEquatable<ConsoleBufferCell>
{
    /// <summary>
    /// The character for this cell.  Defaults to <c>'\0'</c>
    /// </summary>
    public char Character { get; set; }

    /// <summary>
    /// Foreground color of the cell. Defaults to <see cref="AnsiColor"/>.<see cref="AnsiColor.Default"/>.
    /// </summary>
    public AnsiColor Foreground { get; set; }

    /// <summary>
    /// Background color of the cell. Defaults to <see cref="AnsiColor"/>.<see cref="AnsiColor.Default"/>.
    /// </summary>
    public AnsiColor Background { get; set; }

    /// <summary>
    /// Generate a new cell with a different character, but the same foreground and background as the current cell.
    /// </summary>
    /// <param name="c">New character of the returned cell.</param>
    /// <returns>New ConsoleBufferCell.</returns>
    public ConsoleBufferCell WithCharacter(char c) => this with { Character = c };

    /// <summary>
    /// Generate a new cell with a different foreground color, but the same character and background as the current cell.
    /// </summary>
    /// <param name="color">New foreground of the returned cell.</param>
    /// <returns>New ConsoleBufferCell.</returns>
    public ConsoleBufferCell WithForeground(AnsiColor color) => this with { Foreground = color };

    /// <summary>
    /// Generate a new cell with a different background color, but the same character and foreground as the current cell.
    /// </summary>
    /// <param name="color">New background of the returned cell.</param>
    /// <returns>New ConsoleBufferCell.</returns> 
    public ConsoleBufferCell WithBackground(AnsiColor color) => this with { Background = color };

    /// <summary>
    /// Generate a new cell using the provided foreground color if this foreground is <see cref="AnsiColor"/>.<see cref="AnsiColor.Default"/>
    /// and the provided background if this background is <see cref="AnsiColor"/>.<see cref="AnsiColor.Default"/>.
    /// </summary>
    /// <param name="foreground">Foreground color to override <see cref="AnsiColor"/>.<see cref="AnsiColor.Default"/> with.</param>
    /// <param name="background">Background color to override <see cref="AnsiColor"/>.<see cref="AnsiColor.Default"/> with.</param>
    /// <returns>New ConsoleBufferCell.</returns>
    public ConsoleBufferCell OverrideDefaults(AnsiColor foreground, AnsiColor background) =>
        this with
        {
            Foreground = Foreground == AnsiColor.Default ? foreground : Foreground,
            Background = Background == AnsiColor.Default ? background : Background,
        };

    /// <summary>
    /// A ConsoleBuffer cell where Character = <c>\0</c>, Foreground = <see cref="AnsiColor"/>.<see cref="AnsiColor.Default"/>,
    /// and Background = <see cref="AnsiColor"/>.<see cref="AnsiColor.Default"/>.
    /// </summary>
    public static readonly ConsoleBufferCell Zero = default;

    /// <summary>
    /// Creates a new ConsoleBufferCell with the provided character, but Foreground and Background are AnsiColor.Default.
    /// </summary>
    /// <param name="c">Character to set in the new cell</param>
    /// <returns>New ConsoleBufferCell.</returns>
    public static ConsoleBufferCell FromChar(char c) => new() { Character = c };

    /// <summary>
    /// Operator to check for equality of two ConsoleBufferCell values
    /// </summary>
    /// <param name="left">A console buffer cell value</param>
    /// <param name="right">A console buffer cell value</param>
    /// <returns>True, if equal. False, if not equal.</returns>
    public static bool operator ==(ConsoleBufferCell left, ConsoleBufferCell right) => left.Equals(right);

    /// <summary>
    /// Operator to check for inequality of two ConsoleBufferCell values
    /// </summary>
    /// <param name="left">A console buffer cell value</param>
    /// <param name="right">A console buffer cell value</param>
    /// <returns>True, if not equal. False, if equal.</returns>
    public static bool operator !=(ConsoleBufferCell left, ConsoleBufferCell right) => !(left == right);

    /// <summary>
    /// Actual quality test of this cell value against a different cell value.  Cells are equal if Color, Foreground
    /// and Background all equal.
    /// </summary>
    /// <param name="other">Other console value to compare to this value.</param>
    /// <returns>True, if equal. False, if not equal.</returns>
    public bool Equals(ConsoleBufferCell other)
    {
        return Character == other.Character && Foreground == other.Foreground && Background == other.Background;
    }

    /// <summary>
    /// Actual quality test of this cell value against a different cell value.  Cells are equal if Color, Foreground
    /// and Background all equal.
    /// </summary>
    /// <param name="obj">Other console value to compare to this value.</param>
    /// <returns>True, if equal. False, if not equal.</returns>
    public override bool Equals(object? obj)
    {
        return obj is ConsoleBufferCell other && Equals(other);
    }

    /// <summary>
    /// Generate a hash code of this cell value.
    /// </summary>
    /// <returns>Integer hash code</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Character, (int)Foreground, (int)Background);
    }
}