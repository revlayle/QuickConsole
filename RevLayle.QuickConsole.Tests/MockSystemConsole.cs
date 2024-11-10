using System.Text;
using RevLayle.QuickConsole;

namespace RevLayle.QuickConsole.Tests;

public class MockSystemConsole : ISystemConsole
{
    private class MockTextWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
    
    private ConsoleKeyInfo[] _keyBuffer = [];
    private int _keyIdx = 0;

    public ConsoleKeyInfo[] KeyBuffer
    {
        get => _keyBuffer;
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

    public TextWriter Out { get; init; } = new MockTextWriter();

    public void SetCursorPosition(int left, int top)
    {
        // do nothing
    }
}