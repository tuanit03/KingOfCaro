using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KingOfCaro
{
    public partial class Form2 : Form
    {
        ChessBoard chessBoard;
        private MenuForm menuForm;

        public Form2(MenuForm menuForm)
        {
            InitializeComponent();
            this.menuForm = menuForm;
            chessBoard = new ChessBoard(pnChessBoard, tbPlayerName, pbPlayer);


            //TrainningGame trainningGame = new TrainningGame();
            //trainningGame.RecordTrainingData(Matrix, machineMove, isMachineWin);


            pgTimer.Maximum = SaveValues.Progress_Max;
            pgTimer.Step = SaveValues.Progress_Step;
            timer1.Interval = SaveValues.Timer_Interval;
            pgTimer.Value = 0;
            _ = new TimePlayer(pgTimer, timer1);
            chessBoard.NewGame();

            int cnt = SaveValues.Cnt;
            int gameCount = SaveValues.GameCount;
            
            chessBoard.AutoPlay(cnt, gameCount);
        }


       







        private void timer1_Tick(object sender, EventArgs e)
        {
            TimePlayer.ProgressBar.PerformStep();
            if (TimePlayer.ProgressBar.Value >= TimePlayer.ProgressBar.Maximum)
            {
                chessBoard.EndGame();
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn chơi ván cờ mới ? ", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                chessBoard.NewGame();
                TimePlayer.StartTimer();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chessBoard.UndoButton(sender, e);
            TimePlayer.StartTimer();
            if (ChessBoard.SaveButton.Count == 0)
            {
                undoToolStripMenuItem.Enabled = false;
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2_FormClosing(sender, new FormClosingEventArgs(CloseReason.UserClosing, false));
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Con gà muốn chạy hả", "Thông báo", MessageBoxButtons.OKCancel)
               != DialogResult.OK)
                e.Cancel = true;
            this.Hide();
            menuForm.Show();
        }
    }
}
