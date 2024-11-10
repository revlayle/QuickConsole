namespace RevLayle;

public interface ISystemConsole
{
    bool CursorVisible { get; set; }
    bool KeyAvailable { get; }
    ConsoleKeyInfo ReadKey();
    TextWriter Out { get; }
    void SetCursorPosition(int left, int top);
}