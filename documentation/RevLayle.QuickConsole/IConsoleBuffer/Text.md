# IConsoleBuffer.Text method (1 of 3)

Write text to the buffer at location X and Y

```csharp
public void Text(int x, int y, string text)
```

| parameter | description |
| --- | --- |
| x | X position to draw at |
| y | Y position to draw at |
| text | Text to draw |

## See Also

* interface [IConsoleBuffer](../IConsoleBuffer.md)
* namespace [RevLayle.QuickConsole](../../RevLayle.QuickConsole.md)

---

# IConsoleBuffer.Text method (2 of 3)

Write text to the buffer at location X and Y, with a given foreground color

```csharp
public void Text(int x, int y, string text, AnsiColor color)
```

| parameter | description |
| --- | --- |
| x | X position to draw at |
| y | Y position to draw at |
| text | Text to draw |
| color | Foreground color to draw on the cells |

## See Also

* enum [AnsiColor](../AnsiColor.md)
* interface [IConsoleBuffer](../IConsoleBuffer.md)
* namespace [RevLayle.QuickConsole](../../RevLayle.QuickConsole.md)

---

# IConsoleBuffer.Text method (3 of 3)

Write text to the buffer at location X and Y, with a given foreground and background color

```csharp
public void Text(int x, int y, string text, AnsiColor color, AnsiColor background)
```

| parameter | description |
| --- | --- |
| x | X position to draw at |
| y | Y position to draw at |
| text | Text to draw |
| color | Foreground color to draw on the cells |
| background | Background color to draw on the cells |

## See Also

* enum [AnsiColor](../AnsiColor.md)
* interface [IConsoleBuffer](../IConsoleBuffer.md)
* namespace [RevLayle.QuickConsole](../../RevLayle.QuickConsole.md)

<!-- DO NOT EDIT: generated by xmldocmd for RevLayle.QuickConsole.dll -->
