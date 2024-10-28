using RevLayle;

QuickConsole.BufferSize(60, 20);
//var hstart = 2;
//var hend = 58;
var vstart = 2;
var vend = 18;

//var currh = hstart;
var currv = vstart;

while (true)
{
    QuickConsole.Box(0, 0, 60, 20, '|', '-', '+', QuickConsoleColor.Yellow, QuickConsoleColor.Blue);
    QuickConsole.WriteBuffer();
    var s =QuickConsole.ReadText(5, 5, 10);
    QuickConsole.Line(2, currv, 56, LineDirection.Horizontal, '*', QuickConsoleColor.Cyan, QuickConsoleColor.Black);
    QuickConsole.Text(2, 0, s);
    QuickConsole.WriteBuffer();
    Thread.Sleep(250);
    QuickConsole.Box(0, 0, 60, 20, '|', '-', 'X', QuickConsoleColor.Yellow, QuickConsoleColor.Blue);
    QuickConsole.Line(2, currv, 56, LineDirection.Horizontal, '*', QuickConsoleColor.Cyan, QuickConsoleColor.Black);
    QuickConsole.Text(2, 1, s);
    QuickConsole.WriteBuffer();
    Thread.Sleep(250);
    currv++;
    if (currv >= vend) currv = vstart;
}

//System.RevLayle.QuickConsole.ReadLine();