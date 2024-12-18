# AnsiColor enumeration

Represents the eight basic console of an ANSI-based console, plus a "default" value that can be overridden

```csharp
public enum AnsiColor
```

## Values

| name | value | description |
| --- | --- | --- |
| Default | `0` | AnsiColor.Default is a non-color value. ConsoleBuffer will replace cells with this value with the buffer's current foreground or background color respectively. |
| Black | `1` | Black |
| Red | `2` | Red |
| Green | `3` | Green |
| Yellow | `4` | Yellow |
| Blue | `5` | Blue |
| Magenta | `6` | Magenta |
| Cyan | `7` | Cyan |
| White | `8` | White |

## See Also

* namespace [RevLayle.QuickConsole](../RevLayle.QuickConsole.md)

<!-- DO NOT EDIT: generated by xmldocmd for RevLayle.QuickConsole.dll -->
