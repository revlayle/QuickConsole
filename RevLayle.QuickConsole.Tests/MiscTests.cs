using NuGet.Frameworks;
using RevLayle.QuickConsole;

namespace RevLayle.QuickConsole.Tests;

public class MiscTests
{
    [Fact]
    public void TestHeight()
    {
        var buffer = new ConsoleBuffer(5, 10);
        Assert.True(buffer.Height == 10);
    }
    
    [Fact]
    public void TestWidth()
    {
        var buffer = new ConsoleBuffer(5, 10);
        Assert.True(buffer.Width == 5);
    }
}