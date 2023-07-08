using ProgressBar = System.Windows.Forms.ProgressBar;
using Timer = System.Windows.Forms.Timer;

namespace KingOfCaro
{
    public class TimePlayer
    {
        private static ProgressBar progressBar;
        public static ProgressBar ProgressBar { get => progressBar; set => progressBar = value; }
        private static Timer setTimer;
        public static Timer SetTimer { get => setTimer; set => setTimer = value; }

        public TimePlayer(ProgressBar a, Timer b)
        {
            progressBar = a;
            setTimer = b;
        }
        public static void StartTimer()
        {
            SetTimer.Stop();
            SetTimer.Start();
            ProgressBar.Value = 0;
        }
    }
}
