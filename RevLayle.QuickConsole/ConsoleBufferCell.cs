namespace RevLayle.QuickConsole;

public struct ConsoleBufferCell : IEquatable<ConsoleBufferCell>
{
    public char Character { get; set; }
    public AnsiColor Foreground { get; set; }
    public AnsiColor Background { get; set; }
    public ConsoleBufferCell WithCharacter(char c) => this with { Character = c };
    public ConsoleBufferCell WithForeground(AnsiColor color) => this with { Foreground = color };
    public ConsoleBufferCell WithBackground(AnsiColor color) => this with { Background = color };
    public ConsoleBufferCell OverrideDefaults(AnsiColor foreground, AnsiColor background) =>
        this with
        {
            Foreground = Foreground == AnsiColor.Default ? foreground : Foreground, 
            Background = Background == AnsiColor.Default ? background : Background,
        };
    
    public static readonly ConsoleBufferCell Zero = default;
    public static ConsoleBufferCell FromChar(char c) => new() { Character = c };

    public static bool operator ==(ConsoleBufferCell left, ConsoleBufferCell right) => left.Equals(right);

    public static bool operator !=(ConsoleBufferCell left, ConsoleBufferCell right) => !(left == right);

    public bool Equals(ConsoleBufferCell other)
    {
        return Character == other.Character && Foreground == other.Foreground && Background == other.Background;
    }

    public override bool Equals(object? obj)
    {
        return obj is ConsoleBufferCell other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Character, (int)Foreground, (int)Background);
    }
}