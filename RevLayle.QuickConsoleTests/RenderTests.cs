using System.Text;

namespace RevLayle.QuickConsoleTests;

public class RenderTests
{
    private IConsoleBuffer GetSmallBuffer() => new ConsoleBuffer(5, 5);

    private void PutBuffer(IConsoleBuffer buffer, ConsoleBufferCell[] cells)
    {
        for (var i = 0; i < cells.Length; i++)
            if (i < buffer.Cells.Length) buffer.Cells[i] = cells[i];
    }

    private static bool IsZeroCell(ConsoleBufferCell cell) =>
        cell.Character == 0 && cell.Background == 0 && cell.Foreground == 0;

    private Func<ConsoleBufferCell, bool> IsCellComparer(ConsoleBufferCell sourceCell) =>
        x => sourceCell.Character == x.Character && sourceCell.Background == x.Background;
    [Fact]
    public void RenderSimpleToStream()
    {
        var zero = ConsoleBufferCell.Zero;
        var rbx = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        var buffer = GetSmallBuffer();
        buffer.CurrentForegroundColor = QuickConsoleColor.White;
        buffer.CurrentBackgroundColor = QuickConsoleColor.Black;
        PutBuffer(buffer, new []
        {
            rbx, rbx, rbx, rbx, rbx,
            rbx, rbx, rbx, rbx, rbx,
            zero, zero, zero, zero, zero,
            rbx, rbx, rbx, rbx, rbx,
            rbx, rbx, rbx, rbx, rbx,
        });
        
        using var memoryStream = new MemoryStream();
        using var textWriter = new StreamWriter(memoryStream);
        buffer.WriteBuffer(textWriter);
        var ansiString = Encoding.UTF8.GetString(memoryStream.ToArray());
        Assert.True(ansiString == "\x1b[1;1H\x1b[31m\x1b[44mxxxxx\nxxxxx\n\x1b[37m\x1b[40m     \n\x1b[31m\x1b[44mxxxxx\nxxxxx");
    }
}