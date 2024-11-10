using RevLayle;

QuickConsole.BufferSize(60, 20);
var vstart = 2;
var vend = 18;

var currv = vstart;

var cardBuffer = new ConsoleBuffer(100, 5)
{
    CurrentForegroundColor = QuickConsoleColor.Black,
    CurrentBackgroundColor = QuickConsoleColor.White
};
cardBuffer.Box(0, 0, 100, 5, ConsoleBufferCell.FromChar('#'));

var coloredSpace = new ConsoleBufferCell { Character = ' ', Foreground = QuickConsoleColor.Yellow, Background = QuickConsoleColor.Blue };
while (true)
{
    QuickConsole.Rectangle(0, 0, 60, 20, coloredSpace);
    QuickConsole.Draw(2, currv, cardBuffer);
    QuickConsole.WriteBuffer();
    Thread.Sleep(200);
    currv++;
    if (currv >= vend) currv = vstart;
    if (QuickConsole.KeyAvailable && QuickConsole.ReadKey().Key == ConsoleKey.Enter) break;
}
