# InteractiveConsole class

An IConsoleBuffer compatible class that can read data from an instance of ISystemConsole.

```csharp
public class InteractiveConsole : IConsoleBuffer
```

## Public Members

| name | description |
| --- | --- |
| [InteractiveConsole](InteractiveConsole/InteractiveConsole.md)(…) | Creates an [`InteractiveConsole`](./InteractiveConsole.md) instance (with the provided width and height) that uses a provided implementation of [`ISystemConsole`](./ISystemConsole.md). This allows rendering to happen to an actual terminal. |
| static [FromSystemConsole](InteractiveConsole/FromSystemConsole.md)(…) | Creates an [`InteractiveConsole`](./InteractiveConsole.md) instance (with the provided width and height) that uses DotNetSystemConsole implementation of [`ISystemConsole`](./ISystemConsole.md). This allows rendering to happen to an actual terminal. |
| [Cells](InteractiveConsole/Cells.md) { get; } | Gets the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Cells`](./IConsoleBuffer/Cells.md) |
| [CurrentBackgroundColor](InteractiveConsole/CurrentBackgroundColor.md) { get; set; } | Gets the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`CurrentBackgroundColor`](./IConsoleBuffer/CurrentBackgroundColor.md) |
| [CurrentForegroundColor](InteractiveConsole/CurrentForegroundColor.md) { get; set; } | Gets the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`CurrentForegroundColor`](./IConsoleBuffer/CurrentForegroundColor.md) |
| [Height](InteractiveConsole/Height.md) { get; } | Gets the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Height`](./IConsoleBuffer/Height.md) |
| [KeyAvailable](InteractiveConsole/KeyAvailable.md) { get; } | Get the value of the [`InteractiveConsole`](./InteractiveConsole.md)'s buffer to it's [`ISystemConsole`](./ISystemConsole.md)'s [`KeyAvailable`](./ISystemConsole/KeyAvailable.md) property. |
| [Width](InteractiveConsole/Width.md) { get; } | Gets the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Width`](./IConsoleBuffer/Width.md) |
| [Box](InteractiveConsole/Box.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Box`](./IConsoleBuffer/Box.md) (2 methods) |
| [Cell](InteractiveConsole/Cell.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Cell`](./IConsoleBuffer/Cell.md) |
| [Copy](InteractiveConsole/Copy.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Copy`](./IConsoleBuffer/Copy.md) |
| [Draw](InteractiveConsole/Draw.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Draw`](./IConsoleBuffer/Draw.md) |
| [Flip](InteractiveConsole/Flip.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Flip`](./IConsoleBuffer/Flip.md) |
| [GetCellAt](InteractiveConsole/GetCellAt.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`GetCellAt`](./IConsoleBuffer/GetCellAt.md) |
| [GetStringAt](InteractiveConsole/GetStringAt.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`GetStringAt`](./IConsoleBuffer/GetStringAt.md) |
| [IsFullyInBounds](InteractiveConsole/IsFullyInBounds.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`IsFullyInBounds`](./IConsoleBuffer/IsFullyInBounds.md) |
| [IsOutOfBounds](InteractiveConsole/IsOutOfBounds.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`IsOutOfBounds`](./IConsoleBuffer/IsOutOfBounds.md) (2 methods) |
| [Line](InteractiveConsole/Line.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Line`](./IConsoleBuffer/Line.md) |
| [ReadKey](InteractiveConsole/ReadKey.md)() | Calls the [`InteractiveConsole`](./InteractiveConsole.md)'s buffer to it's [`ISystemConsole`](./ISystemConsole.md)'s [`ReadKey`](./ISystemConsole/ReadKey.md) method. |
| [ReadText](InteractiveConsole/ReadText.md)(…) | Reads text interactively from the console represented by the internal [`ISystemConsole`](./ISystemConsole.md) implementation. The result is kept in the console buffer and as a return value. Backspace key is recognized to delete characters input. The enter key read from the input signals the end of input and the string is returned. |
| [Rectangle](InteractiveConsole/Rectangle.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Rectangle`](./IConsoleBuffer/Rectangle.md) |
| [Rotate](InteractiveConsole/Rotate.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Rotate`](./IConsoleBuffer/Rotate.md) |
| [Scroll](InteractiveConsole/Scroll.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Scroll`](./IConsoleBuffer/Scroll.md) |
| [Text](InteractiveConsole/Text.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`Text`](./IConsoleBuffer/Text.md) (3 methods) |
| [Update](InteractiveConsole/Update.md)() | Renders the current state of the [`InteractiveConsole`](./InteractiveConsole.md)'s buffer to it's [`ISystemConsole`](./ISystemConsole.md)'s [`Out`](./ISystemConsole/Out.md) property. |
| [WriteBuffer](InteractiveConsole/WriteBuffer.md)(…) | Calls the internal implementation of the [`InteractiveConsole`](./InteractiveConsole.md)'s [`IConsoleBuffer`](./IConsoleBuffer.md).[`WriteBuffer`](./IConsoleBuffer/WriteBuffer.md) |

## See Also

* interface [IConsoleBuffer](./IConsoleBuffer.md)
* namespace [RevLayle.QuickConsole](../RevLayle.QuickConsole.md)

<!-- DO NOT EDIT: generated by xmldocmd for RevLayle.QuickConsole.dll -->
