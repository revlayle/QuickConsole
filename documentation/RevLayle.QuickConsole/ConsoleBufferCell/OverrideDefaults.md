# ConsoleBufferCell.OverrideDefaults method

Generate a new cell using the provided foreground color if this foreground is [`AnsiColor`](../AnsiColor.md).Default and the provided background if this background is [`AnsiColor`](../AnsiColor.md).Default.

```csharp
public ConsoleBufferCell OverrideDefaults(AnsiColor foreground, AnsiColor background)
```

| parameter | description |
| --- | --- |
| foreground | Foreground color to override [`AnsiColor`](../AnsiColor.md).Default with. |
| background | Background color to override [`AnsiColor`](../AnsiColor.md).Default with. |

## Return Value

New ConsoleBufferCell.

## See Also

* enum [AnsiColor](../AnsiColor.md)
* struct [ConsoleBufferCell](../ConsoleBufferCell.md)
* namespace [RevLayle.QuickConsole](../../RevLayle.QuickConsole.md)

<!-- DO NOT EDIT: generated by xmldocmd for RevLayle.QuickConsole.dll -->