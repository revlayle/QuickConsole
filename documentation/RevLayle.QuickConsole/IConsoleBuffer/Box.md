# IConsoleBuffer.Box method (1 of 2)

Draws a box in the console buffer.

```csharp
public void Box(int x, int y, int width, int height, ConsoleBufferCell cell)
```

| parameter | description |
| --- | --- |
| x | X position of the top-left corner of the box |
| y | Y position of the top-left corner of the box |
| width | Width of the box. |
| height | Height of the box |
| cell | The [`ConsoleBufferCell`](../ConsoleBufferCell.md) value tto draw the box frame with |

## See Also

* struct [ConsoleBufferCell](../ConsoleBufferCell.md)
* interface [IConsoleBuffer](../IConsoleBuffer.md)
* namespace [RevLayle.QuickConsole](../../RevLayle.QuickConsole.md)

---

# IConsoleBuffer.Box method (2 of 2)

Draws a box in the console buffer.

```csharp
public void Box(int x, int y, int width, int height, ConsoleBufferCell cellSides, 
    ConsoleBufferCell cellTopBottom, ConsoleBufferCell cellCorner)
```

| parameter | description |
| --- | --- |
| x | X position of the top-left corner of the box |
| y | Y position of the top-left corner of the box |
| width | Width of the box. |
| height | Height of the box |
| cellSides | The [`ConsoleBufferCell`](../ConsoleBufferCell.md) value to draw the left and right box frame sides with |
| cellTopBottom | The [`ConsoleBufferCell`](../ConsoleBufferCell.md) value to draw the top and bottom box frame sides with |
| cellCorner | The [`ConsoleBufferCell`](../ConsoleBufferCell.md) value tto draw the box corners with |

## See Also

* struct [ConsoleBufferCell](../ConsoleBufferCell.md)
* interface [IConsoleBuffer](../IConsoleBuffer.md)
* namespace [RevLayle.QuickConsole](../../RevLayle.QuickConsole.md)

<!-- DO NOT EDIT: generated by xmldocmd for RevLayle.QuickConsole.dll -->
