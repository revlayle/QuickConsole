using System.Diagnostics.CodeAnalysis;

namespace RevLayle.QuickConsole;

public class DotNetSystemConsole : ISystemConsole
{
    public bool CursorVisible
    {
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")] 
        get => System.Console.CursorVisible;
        set => System.Console.CursorVisible = value;
    }
    public bool KeyAvailable => System.Console.KeyAvailable;
    public ConsoleKeyInfo ReadKey() => System.Console.ReadKey(true);
    public TextWriter Out => System.Console.Out;
    public void SetCursorPosition(int left, int top) => System.Console.SetCursorPosition(left, top);
}