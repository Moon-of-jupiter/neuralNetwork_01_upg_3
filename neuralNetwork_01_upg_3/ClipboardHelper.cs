using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3
{
    public static class ClipboardHelper
    {

        public static Thread CreateClipboardThread(ThreadStart ts)
        {
            var thread = new Thread(ts);
            thread.SetApartmentState(ApartmentState.STA);
            return thread;
        }

        public static void AddTextToClipboard(string text)
        {
            ThreadStart ts = new ThreadStart(AddToClipboard);
            CreateClipboardThread(ts).Start();
        }

        private static void AddToClipboard()
        {
            System.Windows.Forms.Clipboard.SetText("hello world");
        }
    

    }
}
