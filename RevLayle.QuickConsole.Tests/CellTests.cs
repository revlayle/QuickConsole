using RevLayle.QuickConsole;
using ConsoleColor = System.ConsoleColor;

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
        c = c.WithBackground(global::RevLayle.QuickConsole.AnsiColor.Blue);
        Assert.True(c == new ConsoleBufferCell { Character = 'a', Background = global::RevLayle.QuickConsole.AnsiColor.Blue });
    }
    
    [Fact]
    public void WithForegroundColorTest()
    {
        var c = new ConsoleBufferCell { Character = 'a' };
        c = c.WithForeground(global::RevLayle.QuickConsole.AnsiColor.Blue);
        Assert.True(c == new ConsoleBufferCell { Character = 'a', Foreground = global::RevLayle.QuickConsole.AnsiColor.Blue });
    }   
    
    [Fact]
    public void WithCombinedTest()
    {
        var c = ConsoleBufferCell.FromChar('a')
            .WithForeground(global::RevLayle.QuickConsole.AnsiColor.Blue)
            .WithBackground(global::RevLayle.QuickConsole.AnsiColor.Red);
        Assert.True(c == new ConsoleBufferCell { Character = 'a', Foreground = global::RevLayle.QuickConsole.AnsiColor.Blue, Background = global::RevLayle.QuickConsole.AnsiColor.Red });
    }
    
    [Fact]
    public void WithBothDefaultColors()
    {
        var c = ConsoleBufferCell.FromChar('a')
            .OverrideDefaults(global::RevLayle.QuickConsole.AnsiColor.Blue, global::RevLayle.QuickConsole.AnsiColor.Red);
        Assert.True(c == new ConsoleBufferCell { Character = 'a', Foreground = global::RevLayle.QuickConsole.AnsiColor.Blue, Background = global::RevLayle.QuickConsole.AnsiColor.Red });
    }    
    
    [Fact]
    public void WithForegroundDefaultColors()
    {
        var c = ConsoleBufferCell.FromChar('a').WithBackground(global::RevLayle.QuickConsole.AnsiColor.Magenta)
            .OverrideDefaults(global::RevLayle.QuickConsole.AnsiColor.Blue, global::RevLayle.QuickConsole.AnsiColor.Red);
        Assert.True(c == new ConsoleBufferCell { Character = 'a', Foreground = global::RevLayle.QuickConsole.AnsiColor.Blue, Background = global::RevLayle.QuickConsole.AnsiColor.Magenta });
    }  
    
    [Fact]
    public void WithBackgroundDefaultColors()
    {
        var c = ConsoleBufferCell.FromChar('a').WithForeground(global::RevLayle.QuickConsole.AnsiColor.Magenta)
            .OverrideDefaults(global::RevLayle.QuickConsole.AnsiColor.Blue, global::RevLayle.QuickConsole.AnsiColor.Red);
        Assert.True(c == new ConsoleBufferCell { Character = 'a', Foreground = global::RevLayle.QuickConsole.AnsiColor.Magenta, Background = global::RevLayle.QuickConsole.AnsiColor.Red });
    }

    [Fact]
    public void WithCharacterTest()
    {
        var c = new ConsoleBufferCell
            { Character = 'a', Foreground = global::RevLayle.QuickConsole.AnsiColor.Blue, Background = global::RevLayle.QuickConsole.AnsiColor.Red };
        var d = c.WithCharacter('f');
        Assert.False(d == c);
        Assert.True(d == new ConsoleBufferCell { Character = 'f', Foreground = global::RevLayle.QuickConsole.AnsiColor.Blue, Background = global::RevLayle.QuickConsole.AnsiColor.Red });
    }

    [Fact]
    public void TestHash()
    {
        var c = new ConsoleBufferCell
            { Character = 'a', Foreground = global::RevLayle.QuickConsole.AnsiColor.Blue, Background = global::RevLayle.QuickConsole.AnsiColor.Red };
        var d = new ConsoleBufferCell
            { Character = 'a', Foreground = global::RevLayle.QuickConsole.AnsiColor.Blue, Background = global::RevLayle.QuickConsole.AnsiColor.Red };
        Assert.True(c.GetHashCode() == d.GetHashCode());
    }

    [Fact]
    void TestEquals()
    {
        var c = new ConsoleBufferCell
            { Character = 'a', Foreground = global::RevLayle.QuickConsole.AnsiColor.Blue, Background = global::RevLayle.QuickConsole.AnsiColor.Red };
        var d = new ConsoleBufferCell
            { Character = 'a', Foreground = global::RevLayle.QuickConsole.AnsiColor.Blue, Background = global::RevLayle.QuickConsole.AnsiColor.Red };
        Assert.True(c.Equals((object)d)); 
    }
}