namespace RevLayle.QuickConsole.Tests;

public class TransformTests
{
    private IConsoleBuffer GetBuffer(int w = 3, int h = 3) => new ConsoleBuffer(w, h);

    private IConsoleBuffer GetInteractiveBuffer(int w = 3, int h = 3) =>
        new InteractiveConsole(new MockSystemConsole(), w, h);
    private void PutBuffer(IConsoleBuffer buffer, ConsoleBufferCell[] cells)
    {
        for (var i = 0; i < cells.Length; i++)
            if (i < buffer.Cells.Length) buffer.Cells[i] = cells[i];
    }

    [Fact]
    public void ScrollTest()
    {
        var buffer = GetBuffer();
        var rbx = new ConsoleBufferCell { Character = 'x', Foreground = AnsiColor.Red, Background = AnsiColor.Green };
        var wcy = new ConsoleBufferCell { Character = 'y', Foreground = AnsiColor.White, Background = AnsiColor.Yellow };
        var zero = ConsoleBufferCell.Zero;
        PutBuffer(buffer, [
            zero, rbx, wcy,
            wcy, zero, rbx,
            rbx, wcy, zero,
        ]);
        buffer.Scroll(1, 2);
        var cellsToMatch = new[]
        {
            rbx, wcy, zero,
            zero, rbx, wcy,
            wcy, zero, rbx,
        };
        
        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void InteractiveScrollTest()
    {
        var buffer = GetInteractiveBuffer();
        var rbx = new ConsoleBufferCell { Character = 'x', Foreground = AnsiColor.Red, Background = AnsiColor.Green };
        var wcy = new ConsoleBufferCell { Character = 'y', Foreground = AnsiColor.White, Background = AnsiColor.Yellow };
        var zero = ConsoleBufferCell.Zero;
        PutBuffer(buffer, [
            zero, rbx, wcy,
            wcy, zero, rbx,
            rbx, wcy, zero,
        ]);
        buffer.Scroll(1, 2);
        var cellsToMatch = new[]
        {
            rbx, wcy, zero,
            zero, rbx, wcy,
            wcy, zero, rbx,
        };
        
        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void FlipTest()
    {
        var buffer = GetBuffer();
        var rbx = new ConsoleBufferCell { Character = 'x', Foreground = AnsiColor.Red, Background = AnsiColor.Green };
        var wcy = new ConsoleBufferCell { Character = 'y', Foreground = AnsiColor.White, Background = AnsiColor.Yellow };
        var zero = ConsoleBufferCell.Zero;
        PutBuffer(buffer, [
            zero, rbx, wcy,
            wcy, zero, rbx,
            rbx, wcy, zero,
        ]);
        buffer.Flip(true, true);
        var cellsToMatch = new[]
        {
            zero, wcy, rbx,
            rbx, zero, wcy,
            wcy, rbx, zero,
        };
        
        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }     
    
    [Fact]
    public void InteractiveFlipTest()
    {
        var buffer = GetInteractiveBuffer();
        var rbx = new ConsoleBufferCell { Character = 'x', Foreground = AnsiColor.Red, Background = AnsiColor.Green };
        var wcy = new ConsoleBufferCell { Character = 'y', Foreground = AnsiColor.White, Background = AnsiColor.Yellow };
        var zero = ConsoleBufferCell.Zero;
        PutBuffer(buffer, [
            zero, rbx, wcy,
            wcy, zero, rbx,
            rbx, wcy, zero,
        ]);
        buffer.Flip(true, true);
        var cellsToMatch = new[]
        {
            zero, wcy, rbx,
            rbx, zero, wcy,
            wcy, rbx, zero,
        };
        
        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }    
    
    [Fact]
    public void RotateTestClockWise()
    {
        var buffer = GetBuffer();
        var rbx = new ConsoleBufferCell { Character = 'x', Foreground = AnsiColor.Red, Background = AnsiColor.Green };
        var wcy = new ConsoleBufferCell { Character = 'y', Foreground = AnsiColor.White, Background = AnsiColor.Yellow };
        var zero = ConsoleBufferCell.Zero;
        PutBuffer(buffer, [
            zero, rbx, wcy,
            wcy, zero, rbx,
            rbx, wcy, zero,
        ]);
        buffer.Rotate(0, 0, 3, true);
        var cellsToMatch = new[]
        {
            rbx, wcy, zero,
            wcy, zero, rbx,
            zero, rbx, wcy,
        };
        
        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }   
    
        
    [Fact]
    public void InteractiveRotateTestClockWise()
    {
        var buffer = GetInteractiveBuffer();
        var rbx = new ConsoleBufferCell { Character = 'x', Foreground = AnsiColor.Red, Background = AnsiColor.Green };
        var wcy = new ConsoleBufferCell { Character = 'y', Foreground = AnsiColor.White, Background = AnsiColor.Yellow };
        var zero = ConsoleBufferCell.Zero;
        PutBuffer(buffer, [
            zero, rbx, wcy,
            wcy, zero, rbx,
            rbx, wcy, zero,
        ]);
        buffer.Rotate(0, 0, 3, true);
        var cellsToMatch = new[]
        {
            rbx, wcy, zero,
            wcy, zero, rbx,
            zero, rbx, wcy,
        };
        
        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }   
    
    [Fact]
    public void RotateTestCounterClockWise()
    {
        var buffer = GetBuffer();
        var rbx = new ConsoleBufferCell { Character = 'x', Foreground = AnsiColor.Red, Background = AnsiColor.Green };
        var wcy = new ConsoleBufferCell { Character = 'y', Foreground = AnsiColor.White, Background = AnsiColor.Yellow };
        var zero = ConsoleBufferCell.Zero;
        PutBuffer(buffer, [
            zero, rbx, wcy,
            wcy, zero, rbx,
            rbx, wcy, zero,
        ]);
        buffer.Rotate(0, 0, 3, false);
        var cellsToMatch = new[]
        {
            wcy, rbx, zero,
            rbx, zero, wcy,
            zero, wcy, rbx,
        };
        
        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }    
    
    [Fact]
    public void InteractiveRotateTestCounterClockWise()
    {
        var buffer = GetInteractiveBuffer();
        var rbx = new ConsoleBufferCell { Character = 'x', Foreground = AnsiColor.Red, Background = AnsiColor.Green };
        var wcy = new ConsoleBufferCell { Character = 'y', Foreground = AnsiColor.White, Background = AnsiColor.Yellow };
        var zero = ConsoleBufferCell.Zero;
        PutBuffer(buffer, [
            zero, rbx, wcy,
            wcy, zero, rbx,
            rbx, wcy, zero,
        ]);
        buffer.Rotate(0, 0, 3, false);
        var cellsToMatch = new[]
        {
            wcy, rbx, zero,
            rbx, zero, wcy,
            zero, wcy, rbx,
        };
        
        Assert.True(buffer.Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void CopyTest()
    {
        var buffer = GetBuffer(5, 5);
        buffer.CurrentForegroundColor = AnsiColor.Blue;
        buffer.CurrentBackgroundColor = AnsiColor.Red;
        ConsoleBufferCell FromChar(char c) => ConsoleBufferCell.FromChar(c).OverrideDefaults(AnsiColor.Blue, AnsiColor.Red);
        buffer.Text(0, 1, "Hello");
        buffer.Text(2, 2, "wat");
        var zero = ConsoleBufferCell.Zero;
        var copy = buffer.Copy(1, 1, 3, 3);
        var cellsToMatch = new[]
        {
            FromChar('e'), FromChar('l'), FromChar('l'),
            zero, FromChar('w'), FromChar('a'),
            zero, zero, zero,
        };
        Assert.True(copy.Cells.SequenceEqual(cellsToMatch));
    }
    
    
    [Fact]
    public void InteractiveCopyTest()
    {
        var buffer = GetInteractiveBuffer(5, 5);
        buffer.CurrentForegroundColor = AnsiColor.Blue;
        buffer.CurrentBackgroundColor = AnsiColor.Red;
        ConsoleBufferCell FromChar(char c) => ConsoleBufferCell.FromChar(c).OverrideDefaults(AnsiColor.Blue, AnsiColor.Red);
        buffer.Text(0, 1, "Hello");
        buffer.Text(2, 2, "wat");
        var zero = ConsoleBufferCell.Zero;
        var copy = buffer.Copy(1, 1, 3, 3);
        var cellsToMatch = new[]
        {
            FromChar('e'), FromChar('l'), FromChar('l'),
            zero, FromChar('w'), FromChar('a'),
            zero, zero, zero,
        };
        Assert.True(copy.Cells.SequenceEqual(cellsToMatch));
    }
}