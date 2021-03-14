using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Napalm_OS.Hardware;
using Napalm_OS.Graphics;

namespace Napalm_OS.Core
{
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            // initialize graphics driver
            Graphics2D.Initialize(GraphicsDriver.VMWareSVGA);

            // initialize mouse driver
            MousePS2.Initialize();
        }

        protected override void Run()
        {
            // clear screen
            Graphics2D.Clear(new Color(128, 0, 128));

            // draw performance information
            Graphics2D.DrawText(8, 8, "FPS: " + Graphics2D.FPS.ToString(), Color.White, FontType.Regular9x16);
            Graphics2D.DrawText(8, 28, "RES: " + Graphics2D.Width.ToString() + "x" + Graphics2D.Height.ToString() + "x" + Graphics2D.Depth.ToString(), Color.White, FontType.Regular9x16);
            Graphics2D.DrawText(8, 48, "MS:  x=" + MousePS2.X.ToString() + "   y=" + MousePS2.Y.ToString(), Color.White, FontType.Regular9x16);

            // draw mouse
            MousePS2.Display();

            // draw back buffer
            Graphics2D.Display();

            // update framerate
            Graphics2D.UpdateFramerate();
        }
    }
}
