using RevLayle.QuickConsole;
using ConsoleColor = System.ConsoleColor;

namespace RevLayle.QuickConsole.Tests;

public class SimpleDrawTests
{
    private IConsoleBuffer GetBuffer(int w = 5, int h = 5) => new ConsoleBuffer(w, h);

    private static bool IsZeroCell(ConsoleBufferCell cell) =>
        cell.Character == 0 && cell.Background == 0 && cell.Foreground == 0;

    private Func<ConsoleBufferCell, bool> IsCellComparer(ConsoleBufferCell sourceCell) =>
        x => sourceCell.Character == x.Character && sourceCell.Background == x.Background;

    [Fact]
    public void TestEmptyBuffer()
    {
        var buffer = GetBuffer();
        Assert.True(buffer.Cells.All(IsZeroCell));
    }

    [Fact]
    public void TestFullRectangle()
    {
        var buffer = GetBuffer();
        var cell = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        buffer.Rectangle(0, 0, 5, 5, cell);

        Assert.True(buffer.Cells.All(IsCellComparer(cell)));
    }

    [Fact]
    public void TestPartialRectangle()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var filled = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, filled, filled, filled, zero,
            zero, filled, filled, filled, zero,
            zero, filled, filled, filled, zero,
            zero, filled, filled, filled, zero,
        };
        buffer.Rectangle(1, 1, 3, 4, filled);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestPartialRectangleTooWide()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var filled = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, filled, filled, filled, filled,
            zero, filled, filled, filled, filled,
            zero, filled, filled, filled, filled,
            zero, filled, filled, filled, filled,
        };
        buffer.Rectangle(1, 1, 6, 4, filled);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestSimpleBox()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var box = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, box, box, box, zero,
            zero, box, zero, box, zero,
            zero, box, zero, box, zero,
            zero, box, box, box, zero,
        };
        buffer.Box(1, 1, 3, 4, box);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
        
    [Fact]
    public void TestSimpleBoxTooWide()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var box = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, box, box, box, box,
            zero, box, zero, zero, zero,
            zero, box, zero, zero, zero,
            zero, box, box, box, box,
        };
        buffer.Box(1, 1, 6, 4, box);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestSimpleBoxTooTall()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var box = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, box, box, box, zero,
            zero, box, zero, box, zero,
            zero, box, zero, box, zero,
            zero, box, zero, box, zero,
        };
        buffer.Box(1, 1, 3, 8, box);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
        
    [Fact]
    public void TestSimpleBoxTooWideAndTall()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var box = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, box, box, box, box,
            zero, box, zero, zero, zero,
            zero, box, zero, zero, zero,
            zero, box, zero, zero, zero,
        };
        buffer.Box(1, 1, 10, 10, box);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestDetailedBox()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var corner = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var topBottom = new ConsoleBufferCell
            { Character = 'y', Foreground = global::RevLayle.QuickConsole.AnsiColor.Magenta, Background = global::RevLayle.QuickConsole.AnsiColor.Yellow };
        var sides = new ConsoleBufferCell
            { Character = 'z', Foreground = global::RevLayle.QuickConsole.AnsiColor.Green, Background = global::RevLayle.QuickConsole.AnsiColor.Cyan };
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, corner, topBottom, corner, zero,
            zero, sides, zero, sides, zero,
            zero, sides, zero, sides, zero,
            zero, corner, topBottom, corner, zero,
        };
        buffer.Box(1, 1, 3, 4, sides, topBottom, corner);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestHLine()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var filled = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
            zero, filled, filled, filled, filled,
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
        };
        buffer.Line(1, 2, 4, LineDirection.Horizontal, filled);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestHLineTooWide()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var filled = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
            zero, filled, filled, filled, filled,
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
        };
        buffer.Line(1, 2, 8, LineDirection.Horizontal, filled);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestVLine()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var filled = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var cellsToMatch = new[]
        {
            zero, filled, zero, zero, zero,
            zero, filled, zero, zero, zero,
            zero, filled, zero, zero, zero,
            zero, filled, zero, zero, zero,
            zero, filled, zero, zero, zero,
        };
        buffer.Line(1, 0, 5, LineDirection.Vertical, filled);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestVLineTooLong()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var filled = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var cellsToMatch = new[]
        {
            zero, filled, zero, zero, zero,
            zero, filled, zero, zero, zero,
            zero, filled, zero, zero, zero,
            zero, filled, zero, zero, zero,
            zero, filled, zero, zero, zero,
        };
        buffer.Line(1, 0, 8, LineDirection.Vertical, filled);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestCrossedLines()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var line1 = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var line2 = new ConsoleBufferCell
            { Character = 'y', Foreground = global::RevLayle.QuickConsole.AnsiColor.Yellow, Background = global::RevLayle.QuickConsole.AnsiColor.Magenta };
        var cellsToMatch = new[]
        {
            zero, line1, zero, zero, zero,
            zero, line1, zero, zero, zero,
            zero, line1, zero, zero, zero,
            line2, line2, line2, line2, line2,
            zero, line1, zero, zero, zero,
        };
        buffer.Line(1, 0, 5, LineDirection.Vertical, line1);
        buffer.Line(0, 3, 5, LineDirection.Horizontal, line2);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestText()
    {
        var buffer = GetBuffer();
        const string text = "what";
        var textCells = text.Select(x => ConsoleBufferCell.FromChar(x)
            .OverrideDefaults(global::RevLayle.QuickConsole.AnsiColor.Red, global::RevLayle.QuickConsole.AnsiColor.Blue)).ToArray();
        var zero = ConsoleBufferCell.Zero;
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
            textCells[0], textCells[1], textCells[2], textCells[3], zero,
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
        };
        
        buffer.CurrentForegroundColor = global::RevLayle.QuickConsole.AnsiColor.Red;
        buffer.CurrentBackgroundColor = global::RevLayle.QuickConsole.AnsiColor.Blue;
        buffer.Text(0, 2, text);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
    
    [Fact]
    public void TestTextWithForegroundColor()
    {
        var buffer = GetBuffer();
        const string text = "what";
        var textCells = text.Select(x => ConsoleBufferCell.FromChar(x)
            .OverrideDefaults(global::RevLayle.QuickConsole.AnsiColor.Cyan, global::RevLayle.QuickConsole.AnsiColor.Blue)).ToArray();
        var zero = ConsoleBufferCell.Zero;
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
            textCells[0], textCells[1], textCells[2], textCells[3], zero,
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
        };
        
        buffer.CurrentForegroundColor = global::RevLayle.QuickConsole.AnsiColor.Red;
        buffer.CurrentBackgroundColor = global::RevLayle.QuickConsole.AnsiColor.Blue;
        buffer.Text(0, 2, text, global::RevLayle.QuickConsole.AnsiColor.Cyan);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestCells()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var x = new ConsoleBufferCell { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var y = new ConsoleBufferCell { Character = 'y', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Yellow };
        var cellsToMatch = new[]
        {
            zero, zero, x, zero, zero,
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, y,
            zero, zero, zero, zero, zero,
        };

        buffer.Cell(2, 0, x);
        buffer.Cell(4, 3, y);
        
        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestGetCells()
    {
        var buffer = GetBuffer();
        var zero = ConsoleBufferCell.Zero;
        var x = new ConsoleBufferCell { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var y = new ConsoleBufferCell { Character = 'y', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Yellow };

        buffer.Cell(2, 0, x);
        buffer.Cell(4, 3, y);
        
        Assert.True(buffer.GetCellAt(2, 0) == x);
        Assert.True(buffer.GetCellAt(2, 0) != y);
        Assert.True(buffer.GetCellAt(2, 0) != zero);
        Assert.True(buffer.GetCellAt(4, 3) != x);
        Assert.True(buffer.GetCellAt(4, 3) == y);
        Assert.True(buffer.GetCellAt(4, 3) != zero);
        Assert.True(buffer.GetCellAt(4, 4) != x);
        Assert.True(buffer.GetCellAt(4, 4) != y);
        Assert.True(buffer.GetCellAt(4, 4) == zero);
    }
    
    [Fact]
    public void TestGetText()
    {
        var buffer = GetBuffer();
        const string text = "what";
        
        buffer.CurrentForegroundColor = global::RevLayle.QuickConsole.AnsiColor.Red;
        buffer.CurrentBackgroundColor = global::RevLayle.QuickConsole.AnsiColor.Blue;
        buffer.Text(0, 2, text);

        Assert.True(buffer.GetStringAt(0, 2, 4) == text);
        Assert.True(buffer.GetStringAt(0, 2, 5) == text);
        Assert.True(buffer.GetStringAt(2, 2, 3) == text[2..]);
        Assert.True(buffer.GetStringAt(0, 0, 3) == string.Empty);
    }

    [Fact]
    public void DrawBufferTest()
    {
        var buffer = GetBuffer();
        var smallerBuffer = GetBuffer(3, 3);
        var zero = ConsoleBufferCell.Zero;
        var rbx = new ConsoleBufferCell { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        smallerBuffer.Rectangle(0, 0, 3, 3, rbx);
        buffer.Draw(1, 1, smallerBuffer);
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, rbx, rbx, rbx, zero,
            zero, rbx, rbx, rbx, zero,
            zero, rbx, rbx, rbx, zero,
            zero, zero, zero, zero, zero,
        };
        
        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
}