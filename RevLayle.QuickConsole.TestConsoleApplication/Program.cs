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
cardBuffer.Box(0, 0, 100, 5, '#');

while (true)
{
    QuickConsole.Rectangle(0, 0, 60, 20, ' ', QuickConsoleColor.Yellow, QuickConsoleColor.Blue);
    QuickConsole.Draw(2, currv, cardBuffer, true);
    QuickConsole.WriteBuffer();
    Thread.Sleep(200);
    currv++;
    if (currv >= vend) currv = vstart;
    if (QuickConsole.KeyAvailable && QuickConsole.ReadKey().Key == ConsoleKey.Enter) break;
}
