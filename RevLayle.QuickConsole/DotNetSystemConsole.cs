using System.Diagnostics.CodeAnalysis;

namespace RevLayle.QuickConsole;

/// <summary>
/// Implementation of ISystemConsole that uses System.Console
/// </summary>
public class DotNetSystemConsole : ISystemConsole
{
    /// <summary>
    /// This gets/sets System.Console.CursorVisible
    /// </summary>
    public bool CursorVisible
    {
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")] 
        get => System.Console.CursorVisible;
        set => System.Console.CursorVisible = value;
    }
    /// <summary>
    /// This gets System.Console.KeyAvailable
    /// </summary>
    public bool KeyAvailable => System.Console.KeyAvailable;
    /// <summary>
    /// This calls System.Console.ReadKey(true)
    /// </summary>
    /// <returns>Returns the System.ConsoleKeyInfo value from the method call</returns>
    public ConsoleKeyInfo ReadKey() => System.Console.ReadKey(true);
    /// <summary>
    /// This gets System.Console.Out
    /// </summary>
    public TextWriter Out => System.Console.Out;
    /// <summary>
    /// This calls System.Console.SetCursorPosition(left, top)
    /// </summary>
    /// <param name="left">"left" parameter passed to methods</param>
    /// <param name="top">"top" parameter passed to method</param>
    public void SetCursorPosition(int left, int top) => System.Console.SetCursorPosition(left, top);
}