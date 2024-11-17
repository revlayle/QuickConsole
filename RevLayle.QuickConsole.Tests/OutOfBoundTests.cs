using RevLayle.QuickConsole;
using ConsoleColor = System.ConsoleColor;

namespace RevLayle.QuickConsole.Tests;

public class OutOfBoundTests
{
    private IConsoleBuffer GetBuffer(int w = 5, int h = 5) => new ConsoleBuffer(w, h);

    private static bool IsZeroCell(ConsoleBufferCell cell) =>
        cell.Character == 0 && cell.Background == 0 && cell.Foreground == 0;

    private static readonly ConsoleBufferCell _someCell = new ConsoleBufferCell
        { Character = 'A', Background = global::RevLayle.QuickConsole.AnsiColor.Black, Foreground = global::RevLayle.QuickConsole.AnsiColor.White };

    private static readonly ConsoleBufferCell[] _defaultBuffer =
    [
        ConsoleBufferCell.Zero, ConsoleBufferCell.Zero, ConsoleBufferCell.Zero, ConsoleBufferCell.Zero,
        ConsoleBufferCell.Zero,
        ConsoleBufferCell.Zero, ConsoleBufferCell.Zero, ConsoleBufferCell.Zero, ConsoleBufferCell.Zero,
        ConsoleBufferCell.Zero,
        ConsoleBufferCell.Zero, ConsoleBufferCell.Zero, ConsoleBufferCell.Zero, ConsoleBufferCell.Zero,
        ConsoleBufferCell.Zero,
        ConsoleBufferCell.Zero, ConsoleBufferCell.Zero, ConsoleBufferCell.Zero, ConsoleBufferCell.Zero,
        ConsoleBufferCell.Zero,
        ConsoleBufferCell.Zero, ConsoleBufferCell.Zero, ConsoleBufferCell.Zero, ConsoleBufferCell.Zero,
        ConsoleBufferCell.Zero,
    ];

    [Fact]
    public void TestBox()
    {
        var buffer = GetBuffer();
        //buffer.Box(-1, -1, 5, 5, _someCell);
        buffer.Box(0, 0, 5, 0, _someCell);
        buffer.Box(0, 0, -1, 0, _someCell);
        buffer.Box(5, 5, 2, 2, _someCell);
        buffer.Box(-2, -2, 2, 2, _someCell);
        Assert.True(buffer.Cells.SequenceEqual(_defaultBuffer));
    }

    [Fact]
    public void TestCell()
    {
        var buffer = GetBuffer();
        buffer.Cell(6, 3, _someCell);
        buffer.Cell(-1, -1, _someCell);
        Assert.True(buffer.Cells.SequenceEqual(_defaultBuffer));
    }

    [Fact]
    public void TestDraw()
    {
        var buffer = GetBuffer();
        var otherBuffer = GetBuffer(2, 2);
        otherBuffer.Box(0,0,2,2, _someCell);
        //buffer.Draw(-1, -1, otherBuffer);
        buffer.Draw(0, 6, otherBuffer);
        Assert.True(buffer.Cells.SequenceEqual(_defaultBuffer));
        // to verify other buffer actually had cells set
        Assert.True(otherBuffer.Cells.All(x => x == _someCell));
    }

    [Fact]
    public void TestGetCellAt()
    {
        var buffer = GetBuffer();
        Assert.Throws<ArgumentException>(() => buffer.GetCellAt(-1, 0));
    }

    [Fact]
    public void TestGetStringAt()
    {
        var buffer = GetBuffer();
        buffer.Text(0, 0, "x");
        var text = buffer.GetStringAt(-1, 0, 3);
        Assert.True(text == "x");
        text = buffer.GetStringAt(0, 0, 0);
        Assert.True(text == string.Empty);
        text = buffer.GetStringAt(0, 0, -1);
        Assert.True(text == string.Empty);        
        text = buffer.GetStringAt(-5, 0, 5);
        Assert.True(text == string.Empty);
    }

    [Fact]
    public void TestLine()
    {
        var buffer = GetBuffer();
        buffer.Line(-1, -1, 3, LineDirection.Vertical, _someCell);
        buffer.Line(0, 6, 3, LineDirection.Horizontal, _someCell);
        buffer.Line(2, 1, -2, LineDirection.Horizontal, _someCell);
        buffer.Line(1, 1, -2, LineDirection.Vertical, _someCell);
        Assert.True(buffer.Cells.SequenceEqual(_defaultBuffer));
    }
    
    [Fact]
    public void TestRectangle()
    {
        var buffer = GetBuffer();
        //buffer.Rectangle(-1, -1, 3, 5, _someCell);
        buffer.Rectangle(0, 6, 3, 5, _someCell);
        buffer.Rectangle(0, 0, 0, 5, _someCell);
        buffer.Rectangle(0, 0, 3, -1, _someCell);
        Assert.True(buffer.Cells.SequenceEqual(_defaultBuffer));
    }
    
    [Fact]
    public void TestText()
    {
        var buffer = GetBuffer();
        buffer.Text(-1, -1, "hello");
        buffer.Text(0, 6, "hello");
        Assert.True(buffer.Cells.SequenceEqual(_defaultBuffer));
    }

    [Fact]
    public void TestRotate()
    {
        var buffer = GetBuffer();
        buffer.Rotate(-1, 0, 3, true);
        buffer.Rotate(2, 3, 4, true);       
        buffer.Rotate(2, 3, -1, true);
        Assert.True(buffer.Cells.SequenceEqual(_defaultBuffer));
    }
    
    [Fact]
    public void TestCopy()
    {
        var buffer = GetBuffer();
        Assert.Throws<ArgumentException>(() => buffer.Copy(-1, 0, 3, 3));
        Assert.Throws<ArgumentException>(() => buffer.Copy(0, 0, 0, 3));
        Assert.Throws<ArgumentException>(() => buffer.Copy(0, 0, 3, -1));
    }
}