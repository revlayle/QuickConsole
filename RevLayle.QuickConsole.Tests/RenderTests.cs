using System.Text;
using RevLayle.QuickConsole;
using ConsoleColor = System.ConsoleColor;

namespace RevLayle.QuickConsole.Tests;

public class RenderTests
{
    private IConsoleBuffer GetSmallBuffer() => new ConsoleBuffer(5, 5);

    private void PutBuffer(IConsoleBuffer buffer, ConsoleBufferCell[] cells)
    {
        for (var i = 0; i < cells.Length; i++)
            if (i < buffer.Cells.Length) buffer.Cells[i] = cells[i];
    }

    [Fact]
    public void RenderSimpleToStream()
    {
        var zero = ConsoleBufferCell.Zero;
        var rbx = new ConsoleBufferCell
            { Character = 'x', Foreground = global::RevLayle.QuickConsole.AnsiColor.Red, Background = global::RevLayle.QuickConsole.AnsiColor.Blue };
        var buffer = GetSmallBuffer();
        buffer.CurrentForegroundColor = global::RevLayle.QuickConsole.AnsiColor.White;
        buffer.CurrentBackgroundColor = global::RevLayle.QuickConsole.AnsiColor.Black;
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