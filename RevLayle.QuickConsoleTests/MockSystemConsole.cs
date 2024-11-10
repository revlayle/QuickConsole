namespace RevLayle.QuickConsoleTests;

public class MockSystemConsole() : ISystemConsole
{
    private ConsoleKeyInfo[] _keyBuffer = [];
    private int _keyIdx = 0;

    public ConsoleKeyInfo[] KeyBuffer
    {
        get => KeyBuffer;
        set
        {
            _keyBuffer = value;
            _keyIdx = 0;
        }
    }

    public bool CursorVisible { get; set; }
    public bool KeyAvailable => _keyIdx < _keyBuffer.Length;

    public ConsoleKeyInfo ReadKey()
    {
        if (KeyAvailable)
            return _keyBuffer[_keyIdx++];
        throw new IndexOutOfRangeException("No more keys");
    }

    public TextWriter Out { get; set; }

    public void SetCursorPosition(int left, int top)
    {
        // do nothing
    }
}