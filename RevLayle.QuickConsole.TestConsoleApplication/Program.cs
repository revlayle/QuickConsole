﻿using RevLayle;

QuickConsole.BufferSize(60, 20);
//var hstart = 2;
//var hend = 58;
var vstart = 2;
var vend = 18;

//var currh = hstart;
var currv = vstart;

var cardBuffer = new ConsoleBuffer(20, 5);
cardBuffer.SetColor(QuickConsoleColor.Black);
cardBuffer.SetBackgroundColor(QuickConsoleColor.White);
cardBuffer.Box(0, 0, 20, 5, '#');

while (true)
{
    QuickConsole.Rectangle(0, 0, 60, 20, ' ', QuickConsoleColor.Yellow, QuickConsoleColor.Blue);
    //QuickConsole.WriteBuffer();
    //var s =QuickConsole.ReadText(5, 5, 10);
    QuickConsole.Draw(2, currv, cardBuffer, true);
    //QuickConsole.Text(2, 0, s);
    QuickConsole.WriteBuffer();
    Thread.Sleep(200);
    // QuickConsole.Rectangle(0, 0, 60, 20, '|', '-', 'X', QuickConsoleColor.Yellow, QuickConsoleColor.Blue);
    // QuickConsole.Draw(2, currv, cardBuffer);
    // //QuickConsole.Text(2, 1, s);
    // QuickConsole.WriteBuffer();
    // Thread.Sleep(100);
    currv++;
    if (currv >= vend - 4) currv = vstart;
}

//System.RevLayle.QuickConsole.ReadLine();