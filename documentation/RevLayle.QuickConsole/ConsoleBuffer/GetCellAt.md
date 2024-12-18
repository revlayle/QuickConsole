# ConsoleBuffer.GetCellAt method

Gets the ConsoleBufferCell value from the console buffer at position provided.

```csharp
public ConsoleBufferCell GetCellAt(int x, int y)
```

| parameter | description |
| --- | --- |
| x | X position of the cell to get the value for |
| y | X position of the cell to get the value for |

## Return Value

ConsoleBufferCell value of the cell

## Exceptions

| exception | condition |
| --- | --- |
| ArgumentException | X and/or Y value is out-of-bounds of the console buffer. |

## See Also

* struct [ConsoleBufferCell](../ConsoleBufferCell.md)
* class [ConsoleBuffer](../ConsoleBuffer.md)
* namespace [RevLayle.QuickConsole](../../RevLayle.QuickConsole.md)

<!-- DO NOT EDIT: generated by xmldocmd for RevLayle.QuickConsole.dll -->
