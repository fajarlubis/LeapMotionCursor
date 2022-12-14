using System;
using Leap;

namespace LeapMotionCursor
{
    class LeapListener : Listener
    {
        public override void OnInit(Controller cntrlr)
        {
            Console.WriteLine("Initialized");
        }

        public override void OnConnect(Controller cntrlr)
        {
            Console.WriteLine("Connected");
        }

        public override void OnDisconnect(Controller cntrlr)
        {
            Console.WriteLine("Disconnected");
        }

        public override void OnExit(Controller cntrlr)
        {
            Console.WriteLine("Exited");
        }

        private long currentTime;
        private long previousTime;
        private long timeChange;

        public override void OnFrame(Controller cntrlr)
        {
            // Get the current frame.
            Frame currentFrame = cntrlr.Frame();
            cntrlr.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);

            currentTime = currentFrame.Timestamp;
            timeChange = currentTime - previousTime;

            for (int g = 0; g < currentFrame.Gestures().Count; g++)
            {
                if (currentFrame.Gestures()[g].Type == Gesture.GestureType.TYPE_KEY_TAP)
                {
                    MouseCursor.SendClick();
                    Console.WriteLine(currentFrame.Gestures()[g].Type);
                    break;
                }
            }

            if (timeChange > 10000)
            {
                if (!currentFrame.Hands.IsEmpty)
                {
                    // Get the first finger in the list of fingers
                    Finger finger = currentFrame.Fingers[0];
                    // Get the closest screen intercepting a ray projecting from the finger
                    Screen screen = cntrlr.LocatedScreens.ClosestScreenHit(finger);

                    if (screen != null && screen.IsValid)
                    {
                        // Get the velocity of the finger tip
                        var tipVelocity = (int)finger.TipVelocity.Magnitude;

                        // Use tipVelocity to reduce jitters when attempting to hold
                        // the cursor steady.
                        // Change this value if needed.
                        if (tipVelocity > 25)
                        {
                            var xScreenIntersect = screen.Intersect(finger, true).x;
                            var yScreenIntersect = screen.Intersect(finger, true).y;

                            if (xScreenIntersect.ToString() != "NaN")
                            {
                                var x = (int)(xScreenIntersect * screen.WidthPixels);
                                var y = (int)(screen.HeightPixels - (yScreenIntersect * screen.HeightPixels));

                                // Write to Console
                                Console.WriteLine("Screen intersect X: " + xScreenIntersect.ToString());
                                Console.WriteLine("Screen intersect Y: " + yScreenIntersect.ToString());
                                Console.WriteLine("Width pixels: " + screen.WidthPixels.ToString());
                                Console.WriteLine("Height pixels: " + screen.HeightPixels.ToString());

                                Console.WriteLine("\n");

                                Console.WriteLine("x: " + x.ToString());
                                Console.WriteLine("y: " + y.ToString());

                                Console.WriteLine("\n");

                                Console.WriteLine("Tip velocity: " + tipVelocity.ToString());

                                // Move the cursor
                                MouseCursor.MoveCursor(x, y);

                                Console.WriteLine("\n" + new String('=', 40) + "\n");
                            }

                        }
                    }

                }

                previousTime = currentTime;
            }
        }
    }
}
