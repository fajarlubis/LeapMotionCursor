using System;
using Leap;

namespace LeapMotionCursor
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller cntrl = new Controller();
            LeapListener listener = new LeapListener();

            cntrl.AddListener(listener);

            Console.ReadLine();

            cntrl.RemoveListener(listener);
            cntrl.Dispose();
        }
    }
}
