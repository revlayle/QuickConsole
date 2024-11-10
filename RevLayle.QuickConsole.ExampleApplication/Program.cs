using RevLayle.QuickConsole;

var quickConsole = new InteractiveConsole(new DotNetSystemConsole(), 60, 20);
var vstart = 2;
var vend = 18;

var currv = vstart;

var cardBuffer = new ConsoleBuffer(100, 5)
{
    CurrentForegroundColor = AnsiColor.Black,
    CurrentBackgroundColor = AnsiColor.White
};
cardBuffer.Box(0, 0, 100, 5, ConsoleBufferCell.FromChar('#'));

var coloredSpace = new ConsoleBufferCell { Character = ' ', Foreground = AnsiColor.Yellow, Background = AnsiColor.Blue };
while (true)
{
    quickConsole.Rectangle(0, 0, 60, 20, coloredSpace);
    quickConsole.Draw(2, currv, cardBuffer);
    quickConsole.Update();
    Thread.Sleep(200);
    currv++;
    if (currv >= vend) currv = vstart;
    if (quickConsole.KeyAvailable && quickConsole.ReadKey().Key == ConsoleKey.Enter) break;
}
