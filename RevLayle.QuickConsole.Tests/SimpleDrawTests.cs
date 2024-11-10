namespace RevLayle.QuickConsole.Tests;

public class SimpleDrawTests
{
    private IConsoleBuffer GetSmallBuffer() => new ConsoleBuffer(5, 5);

    private static bool IsZeroCell(ConsoleBufferCell cell) =>
        cell.Character == 0 && cell.Background == 0 && cell.Foreground == 0;

    private Func<ConsoleBufferCell, bool> IsCellComparer(ConsoleBufferCell sourceCell) =>
        x => sourceCell.Character == x.Character && sourceCell.Background == x.Background;

    [Fact]
    public void TestEmptyBuffer()
    {
        var buffer = GetSmallBuffer();
        Assert.True(buffer.Cells.All(IsZeroCell));
    }

    [Fact]
    public void TestFullRectangle()
    {
        var buffer = GetSmallBuffer();
        var cell = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        buffer.Rectangle(0, 0, 5, 5, cell);

        Assert.True(buffer.Cells.All(IsCellComparer(cell)));
    }

    [Fact]
    public void TestPartialRectangle()
    {
        var buffer = GetSmallBuffer();
        var zero = ConsoleBufferCell.Zero;
        var filled = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
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
    public void TestSimpleBox()
    {
        var buffer = GetSmallBuffer();
        var zero = ConsoleBufferCell.Zero;
        var box = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
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
    public void TestDetailedBox()
    {
        var buffer = GetSmallBuffer();
        var zero = ConsoleBufferCell.Zero;
        var corner = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        var topBottom = new ConsoleBufferCell
            { Character = 'y', Foreground = QuickConsoleColor.Magenta, Background = QuickConsoleColor.Yellow };
        var sides = new ConsoleBufferCell
            { Character = 'z', Foreground = QuickConsoleColor.Green, Background = QuickConsoleColor.Cyan };
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
        var buffer = GetSmallBuffer();
        var zero = ConsoleBufferCell.Zero;
        var filled = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
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
    public void TestVLine()
    {
        var buffer = GetSmallBuffer();
        var zero = ConsoleBufferCell.Zero;
        var filled = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
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
    public void TestCrossedLines()
    {
        var buffer = GetSmallBuffer();
        var zero = ConsoleBufferCell.Zero;
        var line1 = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        var line2 = new ConsoleBufferCell
            { Character = 'y', Foreground = QuickConsoleColor.Yellow, Background = QuickConsoleColor.Magenta };
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
        var buffer = GetSmallBuffer();
        const string text = "what";
        var textCells = text.Select(x => ConsoleBufferCell.FromChar(x)
            .OverrideDefaults(QuickConsoleColor.Red, QuickConsoleColor.Blue)).ToArray();
        var zero = ConsoleBufferCell.Zero;
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
            textCells[0], textCells[1], textCells[2], textCells[3], zero,
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
        };
        
        buffer.CurrentForegroundColor = QuickConsoleColor.Red;
        buffer.CurrentBackgroundColor = QuickConsoleColor.Blue;
        buffer.Text(0, 2, text);

        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestCells()
    {
        var buffer = GetSmallBuffer();
        var zero = ConsoleBufferCell.Zero;
        var x = new ConsoleBufferCell { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        var y = new ConsoleBufferCell { Character = 'y', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Yellow };
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
        var buffer = GetSmallBuffer();
        var zero = ConsoleBufferCell.Zero;
        var x = new ConsoleBufferCell { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        var y = new ConsoleBufferCell { Character = 'y', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Yellow };

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
        var buffer = GetSmallBuffer();
        const string text = "what";
        
        buffer.CurrentForegroundColor = QuickConsoleColor.Red;
        buffer.CurrentBackgroundColor = QuickConsoleColor.Blue;
        buffer.Text(0, 2, text);

        Assert.True(buffer.GetStringAt(0, 2, 4) == text);
        Assert.True(buffer.GetStringAt(0, 2, 5) == text);
        Assert.True(buffer.GetStringAt(2, 2, 3) == text[2..]);
        Assert.True(buffer.GetStringAt(0, 0, 3) == string.Empty);
    }
}