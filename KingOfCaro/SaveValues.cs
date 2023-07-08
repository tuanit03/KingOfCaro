using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfCaro
{
    public class SaveValues
    {
        private static int chess_Width = 30;
        private static int chess_Height = 30;
        private static int progress_Max = 10000;
        private static int progress_Step = 100;
        private static int timer_Interval = 100;

        private static int humanPlayer = 0;
        private static int aiPlayer = 1;
        private static int emptyCell = -1;
        private static int depth = 5;
       
        private static int cnt = 0;
        private static int gameCount = 1;

        public static int Chess_Width { get => chess_Width; set => chess_Width = value; }
        public static int Chess_Height { get => chess_Height; set => chess_Height = value; }
        public static int Progress_Max { get => progress_Max; set => progress_Max = value; }
        public static int Progress_Step { get => progress_Step; set => progress_Step = value; }
        public static int Timer_Interval { get => timer_Interval; set => timer_Interval = value; }


        public static int HumanPlayer { get => humanPlayer; set => humanPlayer = value; }
        public static int AiPlayer { get => aiPlayer; set => aiPlayer = value; }
        public static int EmptyCell { get => emptyCell; set => emptyCell = value; }
        public static int Depth { get => depth; set => depth = value; }

     
        public static int Cnt { get => cnt; set => cnt = value; }
        public static int GameCount { get => gameCount; set => gameCount = value; }









    }
}
