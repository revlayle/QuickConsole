namespace RevLayle.QuickConsole.Tests;

public class CellTests
{
    [Fact]
    public void FromCharTest()
    {
        var c = ConsoleBufferCell.FromChar('a');
        Assert.True(c == new ConsoleBufferCell { Character = 'a' });
    }
    
    [Fact]
    public void WithBackgroundColorTest()
    {
        var c = new ConsoleBufferCell { Character = 'a' };
        c = c.WithBackground(QuickConsoleColor.Blue);
        Assert.True(c == new ConsoleBufferCell { Character = 'a', Background = QuickConsoleColor.Blue });
    }
    
    [Fact]
    public void WithForegroundColorTest()
    {
        var c = new ConsoleBufferCell { Character = 'a' };
        c = c.WithForeground(QuickConsoleColor.Blue);
        Assert.True(c == new ConsoleBufferCell { Character = 'a', Foreground = QuickConsoleColor.Blue });
    }   
    
    [Fact]
    public void WithCombinedTest()
    {
        var c = ConsoleBufferCell.FromChar('a')
            .WithForeground(QuickConsoleColor.Blue)
            .WithBackground(QuickConsoleColor.Red);
        Assert.True(c == new ConsoleBufferCell { Character = 'a', Foreground = QuickConsoleColor.Blue, Background = QuickConsoleColor.Red });
    }
    
    [Fact]
    public void WithBothDefaultColors()
    {
        var c = ConsoleBufferCell.FromChar('a')
            .OverrideDefaults(QuickConsoleColor.Blue, QuickConsoleColor.Red);
        Assert.True(c == new ConsoleBufferCell { Character = 'a', Foreground = QuickConsoleColor.Blue, Background = QuickConsoleColor.Red });
    }    
    
    [Fact]
    public void WithForegroundDefaultColors()
    {
        var c = ConsoleBufferCell.FromChar('a').WithBackground(QuickConsoleColor.Magenta)
            .OverrideDefaults(QuickConsoleColor.Blue, QuickConsoleColor.Red);
        Assert.True(c == new ConsoleBufferCell { Character = 'a', Foreground = QuickConsoleColor.Blue, Background = QuickConsoleColor.Magenta });
    }  
    
    [Fact]
    public void WithBackgroundDefaultColors()
    {
        var c = ConsoleBufferCell.FromChar('a').WithForeground(QuickConsoleColor.Magenta)
            .OverrideDefaults(QuickConsoleColor.Blue, QuickConsoleColor.Red);
        Assert.True(c == new ConsoleBufferCell { Character = 'a', Foreground = QuickConsoleColor.Magenta, Background = QuickConsoleColor.Red });
    }

    [Fact]
    public void WithCharacterTest()
    {
        var c = new ConsoleBufferCell
            { Character = 'a', Foreground = QuickConsoleColor.Blue, Background = QuickConsoleColor.Red };
        var d = c.WithCharacter('f');
        Assert.False(d == c);
        Assert.True(d == new ConsoleBufferCell { Character = 'f', Foreground = QuickConsoleColor.Blue, Background = QuickConsoleColor.Red });
    }

    [Fact]
    public void TestHash()
    {
        var c = new ConsoleBufferCell
            { Character = 'a', Foreground = QuickConsoleColor.Blue, Background = QuickConsoleColor.Red };
        var d = new ConsoleBufferCell
            { Character = 'a', Foreground = QuickConsoleColor.Blue, Background = QuickConsoleColor.Red };
        Assert.True(c.GetHashCode() == d.GetHashCode());
    }

    [Fact]
    void TestEquals()
    {
        var c = new ConsoleBufferCell
            { Character = 'a', Foreground = QuickConsoleColor.Blue, Background = QuickConsoleColor.Red };
        var d = new ConsoleBufferCell
            { Character = 'a', Foreground = QuickConsoleColor.Blue, Background = QuickConsoleColor.Red };
        Assert.True(c.Equals((object)d)); 
    }
}