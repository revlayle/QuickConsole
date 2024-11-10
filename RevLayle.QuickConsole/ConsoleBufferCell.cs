namespace RevLayle;

public struct ConsoleBufferCell : IEquatable<ConsoleBufferCell>
{
    public ConsoleBufferCell()
    {
        Character = '\0';
        Foreground = QuickConsoleColor.Default;
        Background = QuickConsoleColor.Default;
    }

    public char Character { get; set; }
    public QuickConsoleColor Foreground { get; set; }
    public QuickConsoleColor Background { get; set; }
    public ConsoleBufferCell WithCharacter(char c) => this with { Character = c };
    public ConsoleBufferCell WithForeground(QuickConsoleColor color) => this with { Foreground = color };
    public ConsoleBufferCell WithBackground(QuickConsoleColor color) => this with { Background = color };
    public ConsoleBufferCell OverrideDefaults(QuickConsoleColor foreground, QuickConsoleColor background) =>
        this with
        {
            Foreground = Foreground == QuickConsoleColor.Default ? foreground : Foreground, 
            Background = Background == QuickConsoleColor.Default ? background : Background,
        };
    
    public static readonly ConsoleBufferCell Zero = default;
    public static ConsoleBufferCell FromChar(char c) => new ConsoleBufferCell { Character = c };

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