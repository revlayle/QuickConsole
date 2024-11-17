# DotNetSystemConsole class

Implementation of ISystemConsole that uses System.Console

```csharp
public class DotNetSystemConsole : ISystemConsole
```

## Public Members

| name | description |
| --- | --- |
| [DotNetSystemConsole](DotNetSystemConsole/DotNetSystemConsole.md)() | The default constructor. |
| [CursorVisible](DotNetSystemConsole/CursorVisible.md) { get; set; } | This gets/sets System.Console.CursorVisible |
| [KeyAvailable](DotNetSystemConsole/KeyAvailable.md) { get; } | This gets System.Console.KeyAvailable |
| [Out](DotNetSystemConsole/Out.md) { get; } | This gets System.Console.Out |
| [ReadKey](DotNetSystemConsole/ReadKey.md)() | This calls System.Console.ReadKey(true) |
| [SetCursorPosition](DotNetSystemConsole/SetCursorPosition.md)(…) | This calls System.Console.SetCursorPosition(int, int) |

## See Also

* interface [ISystemConsole](./ISystemConsole.md)
* namespace [RevLayle.QuickConsole](../RevLayle.QuickConsole.md)

<!-- DO NOT EDIT: generated by xmldocmd for RevLayle.QuickConsole.dll -->