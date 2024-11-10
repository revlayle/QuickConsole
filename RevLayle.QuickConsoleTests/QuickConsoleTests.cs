using System.Text;

namespace RevLayle.QuickConsoleTests;

public class QuickConsoleTestss
{
    private QuickConsole GetQuickConsole(int w, int h, ISystemConsole systemConsole)
    {
        var ret = new QuickConsole(systemConsole);
        ret.BufferSize(w, h);
        return ret;
    }

    private IConsoleBuffer GetBuffer(int w = 5, int h = 5) => new ConsoleBuffer(w, h);

    private static bool IsZeroCell(ConsoleBufferCell cell) =>
        cell.Character == 0 && cell.Background == 0 && cell.Foreground == 0;

    private Func<ConsoleBufferCell, bool> IsCellComparer(ConsoleBufferCell sourceCell) =>
        x => sourceCell.Character == x.Character && sourceCell.Background == x.Background;

    [Fact]
    public void TestEmptyBuffer()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        Assert.True(quickConsole.GetBuffer().Cells.All(IsZeroCell));
    }

    [Fact]
    public void TestFullRectangle()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        var cell = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        quickConsole.Rectangle(0, 0, 5, 5, cell);

        Assert.True(quickConsole.GetBuffer().Cells.All(IsCellComparer(cell)));
    }

    [Fact]
    public void TestPartialRectangle()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
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
        quickConsole.Rectangle(1, 1, 3, 4, filled);

        Assert.True(quickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestSimpleBox()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
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
        quickConsole.Box(1, 1, 3, 4, box);

        Assert.True(quickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestDetailedBox()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
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
        quickConsole.Box(1, 1, 3, 4, sides, topBottom, corner);

        Assert.True(quickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestHLine()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
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
        quickConsole.Line(1, 2, 4, LineDirection.Horizontal, filled);

        Assert.True(quickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestVLine()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
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
        quickConsole.Line(1, 0, 5, LineDirection.Vertical, filled);

        Assert.True(quickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestCrossedLines()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
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
        quickConsole.Line(1, 0, 5, LineDirection.Vertical, line1);
        quickConsole.Line(0, 3, 5, LineDirection.Horizontal, line2);

        Assert.True(quickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestText()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
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

        quickConsole.CurrentForegroundColor = QuickConsoleColor.Red;
        quickConsole.CurrentBackgroundColor = QuickConsoleColor.Blue;
        quickConsole.Text(0, 2, text);

        Assert.True(quickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }


    [Fact]
    public void TestTextWithForegroundColor()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
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

        quickConsole.CurrentForegroundColor = QuickConsoleColor.Red;
        quickConsole.CurrentBackgroundColor = QuickConsoleColor.Blue;
        quickConsole.Text(0, 2, text, QuickConsoleColor.Cyan);

        Assert.True(quickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestTextWithBothColors()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        const string text = "what";
        var textCells = text.Select(x => ConsoleBufferCell.FromChar(x)
            .OverrideDefaults(QuickConsoleColor.Cyan, QuickConsoleColor.Yellow)).ToArray();
        var zero = ConsoleBufferCell.Zero;
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
            textCells[0], textCells[1], textCells[2], textCells[3], zero,
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
        };

        quickConsole.CurrentForegroundColor = QuickConsoleColor.Red;
        quickConsole.CurrentBackgroundColor = QuickConsoleColor.Blue;
        quickConsole.Text(0, 2, text, QuickConsoleColor.Cyan, QuickConsoleColor.Yellow);

        Assert.True(quickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }
    
    [Fact]
    public void TestCells()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        var zero = ConsoleBufferCell.Zero;
        var x = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        var y = new ConsoleBufferCell
            { Character = 'y', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Yellow };
        var cellsToMatch = new[]
        {
            zero, zero, x, zero, zero,
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, zero,
            zero, zero, zero, zero, y,
            zero, zero, zero, zero, zero,
        };

        quickConsole.Cell(2, 0, x);
        quickConsole.Cell(4, 3, y);

        Assert.True(quickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    [Fact]
    public void TestGetCells()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        var zero = ConsoleBufferCell.Zero;
        var x = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        var y = new ConsoleBufferCell
            { Character = 'y', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Yellow };

        quickConsole.Cell(2, 0, x);
        quickConsole.Cell(4, 3, y);

        Assert.True(quickConsole.GetCellAt(2, 0) == x);
        Assert.True(quickConsole.GetCellAt(2, 0) != y);
        Assert.True(quickConsole.GetCellAt(2, 0) != zero);
        Assert.True(quickConsole.GetCellAt(4, 3) != x);
        Assert.True(quickConsole.GetCellAt(4, 3) == y);
        Assert.True(quickConsole.GetCellAt(4, 3) != zero);
        Assert.True(quickConsole.GetCellAt(4, 4) != x);
        Assert.True(quickConsole.GetCellAt(4, 4) != y);
        Assert.True(quickConsole.GetCellAt(4, 4) == zero);
    }

    [Fact]
    public void TestGetText()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        const string text = "what";

        quickConsole.CurrentForegroundColor = QuickConsoleColor.Red;
        quickConsole.CurrentBackgroundColor = QuickConsoleColor.Blue;
        quickConsole.Text(0, 2, text);

        Assert.True(quickConsole.GetStringAt(0, 2, 4) == text);
        Assert.True(quickConsole.GetStringAt(0, 2, 5) == text);
        Assert.True(quickConsole.GetStringAt(2, 2, 3) == text[2..]);
        Assert.True(quickConsole.GetStringAt(0, 0, 3) == string.Empty);
    }

    [Fact]
    public void DrawBufferTest()
    {
        var mockConsole = new MockSystemConsole();
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        var smallerBuffer = GetBuffer(3, 3);
        var zero = ConsoleBufferCell.Zero;
        var rbx = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        smallerBuffer.Rectangle(0, 0, 3, 3, rbx);
        quickConsole.Draw(1, 1, smallerBuffer);
        var cellsToMatch = new[]
        {
            zero, zero, zero, zero, zero,
            zero, rbx, rbx, rbx, zero,
            zero, rbx, rbx, rbx, zero,
            zero, rbx, rbx, rbx, zero,
            zero, zero, zero, zero, zero,
        };

        Assert.True(quickConsole.GetBuffer().Cells.SequenceEqual(cellsToMatch));
    }

    // Rendering tests
    private void PutBuffer(IConsoleBuffer buffer, ConsoleBufferCell[] cells)
    {
        for (var i = 0; i < cells.Length; i++)
            if (i < buffer.Cells.Length)
                buffer.Cells[i] = cells[i];
    }

    [Fact]
    public void RenderSimpleToStream()
    {
        var zero = ConsoleBufferCell.Zero;
        var rbx = new ConsoleBufferCell
            { Character = 'x', Foreground = QuickConsoleColor.Red, Background = QuickConsoleColor.Blue };
        var memoryStream = new MemoryStream();
        var textWriter = new StreamWriter(memoryStream);
        var mockConsole = new MockSystemConsole { Out = textWriter };
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        quickConsole.CurrentForegroundColor = QuickConsoleColor.White;
        quickConsole.CurrentBackgroundColor = QuickConsoleColor.Black;
        PutBuffer(quickConsole.GetBuffer(), [
            rbx, rbx, rbx, rbx, rbx,
            rbx, rbx, rbx, rbx, rbx,
            zero, zero, zero, zero, zero,
            rbx, rbx, rbx, rbx, rbx,
            rbx, rbx, rbx, rbx, rbx,
        ]);

        quickConsole.WriteBuffer();
        var ansiString = Encoding.UTF8.GetString(memoryStream.ToArray());
        Assert.True(ansiString ==
                    "\x1b[1;1H\x1b[31m\x1b[44mxxxxx\nxxxxx\n\x1b[37m\x1b[40m     \n\x1b[31m\x1b[44mxxxxx\nxxxxx");
    }

    // input tests
    [Fact]
    public void TestKeyAvailable()
    {
        var mockConsole = new MockSystemConsole
        {
            KeyBuffer = [new ConsoleKeyInfo('A', ConsoleKey.A, false, false, false)]
        };
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        Assert.True(quickConsole.KeyAvailable);
    }
    
    [Fact]
    public void TestReadKey()
    {
        var cki = new ConsoleKeyInfo('A', ConsoleKey.A, false, false, false);
        var mockConsole = new MockSystemConsole
        {
            KeyBuffer = [cki]
        };
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        var readKeyInfo = quickConsole.ReadKey();
        Assert.True(readKeyInfo == cki);
        Assert.True(readKeyInfo.Key == ConsoleKey.A);
        Assert.True(readKeyInfo.Modifiers == ConsoleModifiers.None);
        Assert.True(readKeyInfo.KeyChar == 'A');
    }
    
        
    [Fact]
    public void TestReadText()
    {
        var memoryStream = new MemoryStream();
        var textWriter = new StreamWriter(memoryStream);
        var text = "what";
        var mockConsole = new MockSystemConsole
        {
            KeyBuffer = 
            [
                new('w', ConsoleKey.W, false, false, false),
                new('h', ConsoleKey.H, false, false, false),
                new('a', ConsoleKey.A, false, false, false),
                new('t', ConsoleKey.T, false, false, false),
                new('\r', ConsoleKey.Enter, false, false, false),
            ],
            Out = textWriter,
        };
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        var readText = quickConsole.ReadText(0, 2, 5);
        Assert.True(readText == text);
        Assert.True(quickConsole.GetStringAt(0, 2, 5) == text);
    }
    
    [Fact]
    public void TestReadTextWithBackspace()
    {
        var memoryStream = new MemoryStream();
        var textWriter = new StreamWriter(memoryStream);
        var text = "what";
        var mockConsole = new MockSystemConsole
        {
            KeyBuffer = 
            [
                new('w', ConsoleKey.W, false, false, false),
                new('h', ConsoleKey.H, false, false, false),
                new('z', ConsoleKey.Z, false, false, false),
                new('\0', ConsoleKey.Backspace, false, false, false),
                new('a', ConsoleKey.A, false, false, false),
                new('t', ConsoleKey.T, false, false, false),
                new('\r', ConsoleKey.Enter, false, false, false),
            ],
            Out = textWriter,
        };
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        var readText = quickConsole.ReadText(0, 2, 5);
        Assert.True(readText == text);
        Assert.True(quickConsole.GetStringAt(0, 2, 5) == text);
    }
    
    [Fact]
    public void TestReadTextWithTooManyBackspaces()
    {
        var memoryStream = new MemoryStream();
        var textWriter = new StreamWriter(memoryStream);
        var text = "what";
        var mockConsole = new MockSystemConsole
        {
            KeyBuffer = 
            [
                new('w', ConsoleKey.W, false, false, false),
                new('\0', ConsoleKey.Backspace, false, false, false),
                new('\0', ConsoleKey.Backspace, false, false, false),
                new('\0', ConsoleKey.Backspace, false, false, false),
                new('w', ConsoleKey.W, false, false, false),
                new('h', ConsoleKey.H, false, false, false),
                new('z', ConsoleKey.Z, false, false, false),
                new('\0', ConsoleKey.Backspace, false, false, false),
                new('a', ConsoleKey.A, false, false, false),
                new('t', ConsoleKey.T, false, false, false),
                new('\r', ConsoleKey.Enter, false, false, false),
            ],
            Out = textWriter,
        };
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        var readText = quickConsole.ReadText(0, 2, 5);
        Assert.True(readText == text);
        Assert.True(quickConsole.GetStringAt(0, 2, 5) == text);
    }

    [Fact]
    public void TestReadTextWithTooManyKeys()
    {
        var memoryStream = new MemoryStream();
        var textWriter = new StreamWriter(memoryStream);
        var text = "what!";
        var mockConsole = new MockSystemConsole
        {
            KeyBuffer = 
            [
                new('w', ConsoleKey.W, false, false, false),
                new('h', ConsoleKey.H, false, false, false),
                new('a', ConsoleKey.A, false, false, false),
                new('t', ConsoleKey.T, false, false, false),
                new('!', ConsoleKey.D1, true, false, false),
                new('f', ConsoleKey.F, false, false, false),
                new('z', ConsoleKey.Z, false, false, false),
                new('\r', ConsoleKey.Enter, false, false, false),
            ],
            Out = textWriter,
        };
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        var readText = quickConsole.ReadText(0, 2, 5);
        Assert.True(readText == text);
        Assert.True(quickConsole.GetStringAt(0, 2, 5) == text);
    }   
    

    [Fact]
    public void TestReadTextTooWideForBuffer()
    {
        var memoryStream = new MemoryStream();
        var textWriter = new StreamWriter(memoryStream);
        var text = "wha";
        var mockConsole = new MockSystemConsole
        {
            KeyBuffer = 
            [
                new('w', ConsoleKey.W, false, false, false),
                new('h', ConsoleKey.H, false, false, false),
                new('a', ConsoleKey.A, false, false, false),
                new('t', ConsoleKey.T, false, false, false),
                new('\r', ConsoleKey.Enter, false, false, false),
            ],
            Out = textWriter,
        };
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        var readText = quickConsole.ReadText(2, 2, 5);
        Assert.True(readText == text);
        Assert.True(quickConsole.GetStringAt(2, 2, 5) == text);
    }   
    
    [Fact]
    public void TestReadTextIgnoreControlChars()
    {
        var memoryStream = new MemoryStream();
        var textWriter = new StreamWriter(memoryStream);
        var text = "what";
        var mockConsole = new MockSystemConsole
        {
            KeyBuffer = 
            [
                new('w', ConsoleKey.W, false, false, false),
                new('h', ConsoleKey.H, false, false, false),
                new('a', ConsoleKey.A, false, false, false),
                new('\t', ConsoleKey.Tab, false, false, false),
                new('\t', ConsoleKey.Tab, false, false, false),
                new('t', ConsoleKey.T, false, false, false),
                new('\r', ConsoleKey.Enter, false, false, false),
            ],
            Out = textWriter,
        };
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        var readText = quickConsole.ReadText(0, 2, 5);
        Assert.True(readText == text);
        Assert.True(quickConsole.GetStringAt(0, 2, 5) == text);
    }
    
    [Fact]
    public void TestReadTextOutOfBounds()
    {
        var memoryStream = new MemoryStream();
        var textWriter = new StreamWriter(memoryStream);
        var mockConsole = new MockSystemConsole
        {
            KeyBuffer = 
            [
                new('w', ConsoleKey.W, false, false, false),
                new('h', ConsoleKey.H, false, false, false),
                new('a', ConsoleKey.A, false, false, false),
                new('t', ConsoleKey.T, false, false, false),
                new('\r', ConsoleKey.Enter, false, false, false),
            ],
            Out = textWriter,
        };
        var quickConsole = GetQuickConsole(5, 5, mockConsole);
        var readText = quickConsole.ReadText(-1, 2, 5);
        Assert.True(readText == string.Empty);
    }
    
    // Misc tests
    [Fact]
    public void TestGetBackground()
    {
        var quickConsole = GetQuickConsole(5, 5, new MockSystemConsole());
        quickConsole.CurrentBackgroundColor = QuickConsoleColor.Blue;
        Assert.True(quickConsole.CurrentBackgroundColor == QuickConsoleColor.Blue);
    }
    
    [Fact]
    public void TestGetForeground()
    {
        var quickConsole = GetQuickConsole(5, 5, new MockSystemConsole());
        quickConsole.CurrentForegroundColor = QuickConsoleColor.Blue;
        Assert.True(quickConsole.CurrentForegroundColor == QuickConsoleColor.Blue);
    }
}