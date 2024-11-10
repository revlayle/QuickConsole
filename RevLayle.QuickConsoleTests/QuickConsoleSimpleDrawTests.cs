namespace RevLayle.QuickConsoleTests;

public class QuickConsoleSimpleDrawTests
{
    private void SetQuickConsoleBuffer(int w, int h) => QuickConsole.BufferSize(w, h);
    private IConsoleBuffer GetBuffer(int w = 5, int h = 5) => new ConsoleBuffer(w, h);

    private static bool IsZeroCell(ConsoleBufferCell cell) =>
        cell.Character == 0 && cell.Background == 0 && cell.Foreground == 0;

    private Func<ConsoleBufferCell, bool> IsCellComparer(ConsoleBufferCell sourceCell) =>
        x => sourceCell.Character == x.Character && sourceCell.Background == x.Background;

    [Fact]
    public void TestEmptyBuffer()
    {
        SetQuickConsoleBuffer(5, 5);
        Assert.True(QuickConsole.GetBuffer().Cells.All(IsZeroCell));
    }

    [Fact]
    public void TestFullRectangle()
    {
        SetQuickConsoleBuffer(5, 5);
        var cell = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        QuickConsole.Rectangle(0, 0, 5, 5, cell);

        Assert.True(QuickConsole.GetBuffer().Cells.All(IsCellComparer(cell)));
    }

    [Fact]
    public void TestPartialRectangle()
    {
        SetQuickConsoleBuffer(5, 5);
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
        QuickConsole.Rectangle(1, 1, 3, 4, filled);

        Assert.True(QuickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestSimpleBox()
    {
        SetQuickConsoleBuffer(5, 5);
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
        QuickConsole.Box(1, 1, 3, 4, box);

        Assert.True(QuickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestDetailedBox()
    {
        SetQuickConsoleBuffer(5, 5);
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
        QuickConsole.Box(1, 1, 3, 4, sides, topBottom, corner);

        Assert.True(QuickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestHLine()
    {
        SetQuickConsoleBuffer(5, 5);
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
        QuickConsole.Line(1, 2, 4, LineDirection.Horizontal, filled);

        Assert.True(QuickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestVLine()
    {
        SetQuickConsoleBuffer(5, 5);
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
        QuickConsole.Line(1, 0, 5, LineDirection.Vertical, filled);

        Assert.True(QuickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestCrossedLines()
    {
        SetQuickConsoleBuffer(5, 5);
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
        QuickConsole.Line(1, 0, 5, LineDirection.Vertical, line1);
        QuickConsole.Line(0, 3, 5, LineDirection.Horizontal, line2);

        Assert.True(QuickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestText()
    {
        SetQuickConsoleBuffer(5, 5);
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
        
        QuickConsole.CurrentForegroundColor = QuickConsoleColor.Red;
        QuickConsole.CurrentBackgroundColor = QuickConsoleColor.Blue;
        QuickConsole.Text(0, 2, text);

        Assert.True(QuickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }
    
    
    [Fact]
    public void TestTextWithForegroundColor()
    {
        SetQuickConsoleBuffer(5, 5);
        const string text = "what";
        var textCells = text.Select(x => ConsoleBufferCell.FromChar(x)
            .OverrideDefaults(QuickConsoleColor.Cyan, QuickConsoleColor.Blue)).ToArray();
        var zero = ConsoleBufferCell.Zero;
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
            textCells[0], textCells[1], textCells[2], textCells[3], zero,
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
        };
        
        QuickConsole.CurrentForegroundColor = QuickConsoleColor.Red;
        QuickConsole.CurrentBackgroundColor = QuickConsoleColor.Blue;
        QuickConsole.Text(0, 2, text, QuickConsoleColor.Cyan);

        Assert.True(QuickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestCells()
    {
        SetQuickConsoleBuffer(5, 5);
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

        QuickConsole.Cell(2, 0, x);
        QuickConsole.Cell(4, 3, y);
        
        Assert.True(QuickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestGetCells()
    {
        SetQuickConsoleBuffer(5, 5);
        var zero = ConsoleBufferCell.Zero;
        var x = new ConsoleBufferCell { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        var y = new ConsoleBufferCell { Character = 'y', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Yellow };

        QuickConsole.Cell(2, 0, x);
        QuickConsole.Cell(4, 3, y);
        
        Assert.True(QuickConsole.GetCellAt(2, 0) == x);
        Assert.True(QuickConsole.GetCellAt(2, 0) != y);
        Assert.True(QuickConsole.GetCellAt(2, 0) != zero);
        Assert.True(QuickConsole.GetCellAt(4, 3) != x);
        Assert.True(QuickConsole.GetCellAt(4, 3) == y);
        Assert.True(QuickConsole.GetCellAt(4, 3) != zero);
        Assert.True(QuickConsole.GetCellAt(4, 4) != x);
        Assert.True(QuickConsole.GetCellAt(4, 4) != y);
        Assert.True(QuickConsole.GetCellAt(4, 4) == zero);
    }
    
    [Fact]
    public void TestGetText()
    {
        SetQuickConsoleBuffer(5, 5);
        const string text = "what";
        
        QuickConsole.CurrentForegroundColor = QuickConsoleColor.Red;
        QuickConsole.CurrentBackgroundColor = QuickConsoleColor.Blue;
        QuickConsole.Text(0, 2, text);

        Assert.True(QuickConsole.GetStringAt(0, 2, 4) == text);
        Assert.True(QuickConsole.GetStringAt(0, 2, 5) == text);
        Assert.True(QuickConsole.GetStringAt(2, 2, 3) == text[2..]);
        Assert.True(QuickConsole.GetStringAt(0, 0, 3) == string.Empty);
    }

    [Fact]
    public void DrawBufferTest()
    {
        SetQuickConsoleBuffer(5, 5);
        var smallerBuffer = GetBuffer(3, 3);
        var zero = ConsoleBufferCell.Zero;
        var rbx = new ConsoleBufferCell { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        smallerBuffer.Rectangle(0, 0, 3, 3, rbx);
        QuickConsole.Draw(1, 1, smallerBuffer);
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, rbx, rbx, rbx, zero,
            zero, rbx, rbx, rbx, zero,
            zero, rbx, rbx, rbx, zero,
            zero, zero, zero, zero, zero,
        };
        
        Assert.True(QuickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }
}