using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Slider1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            Form1 form = new Form1();
            Screen[] screens = Screen.AllScreens;
            if (screens.Length != 1)
            {
                System.Drawing.Rectangle bounds = screens[1].Bounds;
                form.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                form.StartPosition = FormStartPosition.Manual;
            }
            Application.Run(form);
        }
    }
}
