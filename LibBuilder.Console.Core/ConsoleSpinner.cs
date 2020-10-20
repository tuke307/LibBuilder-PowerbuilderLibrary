using System;
using System.Threading;

namespace LibBuilder.Console.Core
{
    public class ConsoleSpinner : IDisposable
    {
        private const string Sequence = @"/-\|";
        private readonly int delay;
        private readonly int left;
        private readonly Thread thread;
        private readonly int top;
        private bool active;
        private int counter = 0;

        public ConsoleSpinner(int left, int top, int delay = 100)
        {
            this.left = left;
            this.top = top;
            this.delay = delay;
            thread = new Thread(Spin);
        }

        public void Dispose()
        {
            Stop();
        }

        public void Start()
        {
            active = true;
            if (!thread.IsAlive)
                thread.Start();
        }

        public void Stop()
        {
            active = false;
            Draw(' ');
            //System.Console.ForegroundColor = ConsoleColor.White;
        }

        private void Draw(char c)
        {
            System.Console.SetCursorPosition(left, top);
            //System.Console.ForegroundColor = ConsoleColor.DarkGreen;
            System.Console.Write(c);
        }

        private void Spin()
        {
            while (active)
            {
                Turn();
                Thread.Sleep(delay);
            }
        }

        private void Turn()
        {
            Draw(Sequence[++counter % Sequence.Length]);
        }
    }
}