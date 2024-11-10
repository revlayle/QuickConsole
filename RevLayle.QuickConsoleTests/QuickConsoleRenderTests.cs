using System.Diagnostics;
using System.Text;

namespace RevLayle.QuickConsoleTests;

public class QuickConsoleRenderTests
{
    private void PrepareConsole(TextWriter textWriter)
    {
        QuickConsole.BufferSize(5, 5);
        Console.SetOut(textWriter); 
    }

    private void PutBuffer(IConsoleBuffer buffer, ConsoleBufferCell[] cells)
    {
        for (var i = 0; i < cells.Length; i++)
            if (i < buffer.Cells.Length)
                buffer.Cells[i] = cells[i];
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
        var memoryStream = new MemoryStream();
        var textWriter = new StreamWriter(memoryStream);
        PrepareConsole(textWriter);
        QuickConsole.CurrentForegroundColor = QuickConsoleColor.White;
        QuickConsole.CurrentBackgroundColor = QuickConsoleColor.Black;
        PutBuffer(QuickConsole.GetBuffer(), [
            rbx, rbx, rbx, rbx, rbx,
            rbx, rbx, rbx, rbx, rbx,
            zero, zero, zero, zero, zero,
            rbx, rbx, rbx, rbx, rbx,
            rbx, rbx, rbx, rbx, rbx,
        ]);
        
        QuickConsole.WriteBuffer();
        var ansiString = Encoding.UTF8.GetString(memoryStream.ToArray());
        Assert.True(ansiString == "\x1b[1;1H\x1b[31m\x1b[44mxxxxx\nxxxxx\n\x1b[37m\x1b[40m     \n\x1b[31m\x1b[44mxxxxx\nxxxxx");
    }
}