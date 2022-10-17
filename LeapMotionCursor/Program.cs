using System;
using Leap;
using LeapMotionCursor;

Controller cntrl = new();
LeapListener listener = new();

cntrl.AddListener(listener);

Console.ReadLine();

cntrl.RemoveListener(listener);
cntrl.Dispose();
