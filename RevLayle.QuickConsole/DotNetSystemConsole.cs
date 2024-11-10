namespace RevLayle;

public class DotNetSystemConsole : ISystemConsole
{
    public bool CursorVisible
    {
        get => Console.CursorVisible;
        set => Console.CursorVisible = value;
    }
    public bool KeyAvailable => Console.KeyAvailable;
    public ConsoleKeyInfo ReadKey() => Console.ReadKey(true);
    public TextWriter Out => Console.Out;
    public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);
}