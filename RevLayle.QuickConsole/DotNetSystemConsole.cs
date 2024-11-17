using System.Diagnostics.CodeAnalysis;

namespace RevLayle.QuickConsole;

/// <summary>
/// Implementation of ISystemConsole that uses <see cref="System.Console">System.Console</see>
/// </summary>
public class DotNetSystemConsole : ISystemConsole
{
    /// <summary>
    /// This gets/sets <see cref="System.Console.CursorVisible"/>
    /// </summary>
    public bool CursorVisible
    {
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")] 
        get => System.Console.CursorVisible;
        set => System.Console.CursorVisible = value;
    }
    /// <summary>
    /// This gets <see cref="System.Console.KeyAvailable"/>
    /// </summary>
    public bool KeyAvailable => Console.KeyAvailable;
    /// <summary>
    /// This calls <see cref="System.Console.ReadKey(bool)" /> where <c>intercept = true</c>
    /// </summary>
    /// <returns>Returns the System.ConsoleKeyInfo value from the method call</returns>
    public ConsoleKeyInfo ReadKey() => Console.ReadKey(true);
    /// <summary>
    /// This gets <see cref="System.Console.Out" />
    /// </summary>
    public TextWriter Out => Console.Out;
    /// <summary>
    /// This calls <see cref="System.Console.SetCursorPosition(int, int)"/>
    /// </summary>
    /// <param name="left">"left" parameter passed to methods</param>
    /// <param name="top">"top" parameter passed to method</param>
    public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);
}