using RevLayle.QuickConsole;

var quickConsole = new InteractiveConsole(new DotNetSystemConsole(), 60, 20);
var cardBuffer = new ConsoleBuffer(100, 5)
{
    CurrentForegroundColor = AnsiColor.Black,
    CurrentBackgroundColor = AnsiColor.White
};
cardBuffer.Box(0, 0, 100, 5, ConsoleBufferCell.FromChar('#'));
cardBuffer.Text(1, 1, "hi");
var coloredSpace = new ConsoleBufferCell { Character = ' ', Foreground = AnsiColor.Yellow, Background = AnsiColor.Blue };
quickConsole.Rectangle(0, 0, 60, 20, coloredSpace);
quickConsole.Draw(2, 2, cardBuffer);

while (true)
{
    quickConsole.Rotate(2, 2, 5, false);
    //quickConsole.Scroll(ScrollDirection.Down, 1, true);
    quickConsole.Update();
    Thread.Sleep(200);
}
