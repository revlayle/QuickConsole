namespace RevLayle.QuickConsole.Tests;

public class SimpleDrawTests
{
    private IConsoleBuffer GetSmallBuffer() => new ConsoleBuffer(5, 5);

    private bool IsZeroCell(ConsoleBufferCell cell) => cell.Character == 0 && cell.Background == 0 && cell.Foreground == 0;
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
        var filled = new ConsoleBufferCell { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        var cellsToMatch = new ConsoleBufferCell[]
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
}