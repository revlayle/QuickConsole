namespace RevLayle.QuickConsole;

/// <summary>
/// Interface that describes the abstraction of a console or terminal 
/// </summary>
public interface ISystemConsole
{
    /// <summary>
    /// Is the cursor visible in this console?
    /// </summary>
    bool CursorVisible { get; set; }
    /// <summary>
    /// Is there a key available to read from the console?
    /// </summary>
    bool KeyAvailable { get; }
    /// <summary>
    /// Reads a key from the console. If no key is available, it will wait for a key.
    /// </summary>
    /// <returns>A System.ConsoleKeyInfo value describing the key pressed.</returns>
    ConsoleKeyInfo ReadKey();
    /// <summary>
    /// A TextWriter that represents a standard output of the console.  IConsoleBuffer instances can write to this
    /// to render a console.  InteractiveConsole instances will use this in their provided ISystemConsole instance
    /// when InteractiveConsole.Update() is called.
    /// </summary>
    TextWriter Out { get; }
    /// <summary>
    /// Set the current curson position in the console.  Used when reading text from an InteractiveConsole instance.
    /// </summary>
    /// <param name="left">Left position in the console to place the cursor</param>
    /// <param name="top">Right position in the console to place the curson</param>
    void SetCursorPosition(int left, int top);
}