using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Button = System.Windows.Forms.Button;
using TextBox = System.Windows.Forms.TextBox;

namespace KingOfCaro
{
    public class ChessBoard
    {
       // private int[,] board;
        private List<List<Button>> matrix;
        public List<List<Button>> Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }
        List<Point> machineMove = new List<Point>();
        bool chec = false;
        private Panel chessPanel;
        public Panel ChessPanel
        {
            get { return chessPanel; }
            set { chessPanel = value; }
        }
        private List<Player> player;
        public List<Player> Player
        {
            get { return player; }
            set { player = value; }
        }
        private int currentPlayer;
        public int CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }
        private TextBox playerName;
        public TextBox PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }
        private PictureBox playerMark;
        public PictureBox PlayerMark
        {
            get { return playerMark; }
            set { playerMark = value; }
        }
        public static Stack<Button> SaveButton { get => saveButton; set => saveButton = value; }
        public object StartTimer { get; internal set; }
        public static Stack<Button> saveButton = new Stack<Button>();
        private int[,] arrButton = new int[16, 16];


        public static void ResetArrButton(int[,] arrButton, Stack<Button> saveButton)
        {
            while (saveButton.Count > 0)
            {
                Button b = saveButton.Pop();
                arrButton[b.Location.X / 30, b.Location.Y / 30] = 0;
            }
        }
        public ChessBoard(Panel chessPanel, TextBox playerName, PictureBox playerMark)
        {
            
            this.ChessPanel = chessPanel;
            this.PlayerName = playerName;
            this.PlayerMark = playerMark;

            this.Player = new List<Player>()
            {
                new Player("HowKteam", Image.FromFile(Application.StartupPath + "\\Resources\\zz.jpg")),
                new Player("Education", Image.FromFile(Application.StartupPath + "\\Resources\\oo.jpg"))
            };
        }
 
        public void DrawChessBoard()
        {
            Matrix = new List<List<Button>>();

            Button a = new Button()
            {
                Width = SaveValues.Chess_Width,
                Height = SaveValues.Chess_Height,
                Location = new Point(0, 0),
            };

            for (int i = 0; i < 15; i++)
            {
                Matrix.Add(new List<Button>());

                for (int j = 0; j < 15; j++)
                {
                    Button b = new Button()
                    {
                        Width = SaveValues.Chess_Width,
                        Height = SaveValues.Chess_Height,
                        Location = new Point(a.Location.X + j * a.Width, a.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                    };
                    b.Click += B_Click;
                    ChessPanel.Controls.Add(b);
                    Matrix[i].Add(b);
                    //MessageBox.Show("Tọa độ: X = " + b.Location.X + ", Y = " + b.Location.Y);
                }
                a.Location = new Point(0, a.Location.Y + a.Height);
            }
            ///Khoi tao auto
        }
        public void B_Click(object sender, EventArgs e)
        {
            HandleButtonClick(sender as Button);
        }

        private void HandleButtonClick(Button button)
        {
            if (button.BackgroundImage != null)
            {
                return;
            }
            machineMove.Add(new Point(button.Location.X / SaveValues.Chess_Width, button.Location.Y / SaveValues.Chess_Height));
            saveButton.Push(button);
            button.BackgroundImage = Player[currentPlayer].Mark;
            button.Tag = currentPlayer;
            TimePlayer.SetTimer.Start();
            TimePlayer.ProgressBar.Value = 0;

            AddButtonToArr(button.Location.X / 30, button.Location.Y / 30);
            SwapPlayer();
        }

        private void SwapPlayer()
        {
            ++currentPlayer;
            if (currentPlayer == Player.Count)
            {
                currentPlayer = 0;
            }
            PlayerName.Text = Player[currentPlayer].Name;
            PlayerMark.Image = Player[currentPlayer].Mark;
        }
        public void AddButtonToArr(int cdsX, int cdsY)
        {
            arrButton[cdsX, cdsY] = currentPlayer + 100;
            ResultChess(arrButton, cdsX, cdsY, currentPlayer + 100);
            if (chec == true)
            {
                if(currentPlayer == 0)
                {
                    TrainningGame trainningGame = new TrainningGame(this);
                    trainningGame.RecordTrainingData(Matrix, machineMove, true);
                }
                else
                {
                    TrainningGame trainningGame = new TrainningGame(this);
                    trainningGame.RecordTrainingData(Matrix, machineMove, false);
                }
            }
        }
        public void ResultChess(int[,] arrButton, int i, int j, int a)
        {
            CheckWin1(arrButton, i - 1, i + 1, j, a);
            CheckWin2(arrButton, j - 1, j + 1, i, a);
            CheckWin3(arrButton, i - 1, i + 1, j - 1, j + 1, a);
            CheckWin4(arrButton, i - 1, i + 1, j - 1, j + 1, a);
        }
        void CheckWin1(int[,] arrButton, int tmp1, int tmp2, int j, int a)
        {
            int cnt = 1;
            while (true)
            {
                if (tmp1 >= 0 && arrButton[tmp1, j] == a)
                {
                    ++cnt;
                    tmp1 -= 1;
                    continue;
                }
                if (tmp2 <= 15 && arrButton[tmp2, j] == a)
                {
                    ++cnt;
                    tmp2 += 1;
                    continue;
                }
                break;
            }
            if (cnt >= 5) {
                chec = true;
                EndGame();
            }
        }
        void CheckWin2(int[,] arrButton, int tmp3, int tmp4, int i, int a)
        {
            int cnt = 1;
            while (true)
            {
                if (tmp3 >= 0 && arrButton[i, tmp3] == a)
                {
                    ++cnt;
                    tmp3 -= 1;
                    continue;
                }
                if (tmp4 <= 15 && arrButton[i, tmp4] == a)
                {
                    ++cnt;
                    tmp4 += 1;
                    continue;
                }
                break;
            }
            if (cnt >= 5)
            {
                chec = true;
                EndGame();
            }
        }
        void CheckWin3(int[,] arrButton, int tmp1, int tmp2, int tmp3, int tmp4, int a)
        {
            int cnt = 1;
            while (true)
            {
                if (tmp2 <= 15 && tmp4 <= 15 && arrButton[tmp2, tmp4] == a)
                {
                    ++cnt;
                    tmp2 += 1;
                    tmp4 += 1;
                    continue;
                }
                if (tmp1 >= 0 && tmp3 >= 0 && arrButton[tmp1, tmp3] == a)
                {
                    ++cnt;
                    tmp1 -= 1;
                    tmp3 -= 1;
                    continue;
                }
                break;
            }
            if (cnt >= 5)
            {
                chec = true;
                EndGame();
            }
        }
        void CheckWin4(int[,] arrButton, int tmp1, int tmp2, int tmp3, int tmp4, int a)
        {
            int cnt = 1;
            while (true)
            {
                if (tmp2 <= 15 && tmp3 >= 0 && arrButton[tmp2, tmp3] == a)
                {
                    ++cnt;
                    tmp2 += 1;
                    tmp3 -= 1;
                    continue;
                }
                if (tmp1 >= 0 && tmp4 <= 15 && arrButton[tmp1, tmp4] == a)
                {
                    ++cnt;
                    tmp1 -= 1;
                    tmp4 += 1;
                    continue;
                }
                break;
            }
            if (cnt >= 5)
            {
                chec = true;
                EndGame();
            }
        }
        public void UndoButton(object sender, EventArgs e)
        {
            Button c = saveButton.Pop();
            c.BackgroundImage = null;
            arrButton[c.Location.X / 30, c.Location.Y / 30] = 0;
            SwapPlayer();
        }
        public void NewGame()
        {
            currentPlayer = 0;
            PlayerName.Text = Player[currentPlayer].Name;
            PlayerMark.Image = Player[currentPlayer].Mark;
            ChessPanel.Controls.Clear();
            ChessPanel.Enabled = true;
            ResetArrButton(arrButton, SaveButton);
            DrawChessBoard();
        }
        public void EndGame()
        {
            TimePlayer.SetTimer.Stop();
            ChessPanel.Enabled = false;
            MessageBox.Show("True ngu");
        }

        public int[,] ConvertMatrixToBoard(List<List<Button>> Matrix)
        {
            int rowCount = Matrix.Count;
            int columnCount = Matrix[0].Count;
            int[,] board = new int[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    int currentPlayer = (int)Matrix[i][j].Tag;
                    if (Matrix[i][j].BackgroundImage == null)
                    {
                        currentPlayer = -1;
                    }
                    board[i, j] = currentPlayer;
                }
            }

            return board;
        }
        private (int, Point) FindBestMove(int[,] board, int depth, int alpha, int beta, bool isMaximizingPlayer)
        {
            // Khai báo biến lưu tọa độ nước đi tốt nhất
            Point bestMove = new Point(-1, -1);

            if (depth == 0 || IsGameOver(board))
            {
                // Nếu đạt độ sâu tối đa hoặc trạng thái bàn cờ kết thúc, đánh giá điểm số
                int score = EvaluateBoard(board);
                return (score, bestMove); // Trả về điểm số và tọa độ hiện tại
            }

            if (isMaximizingPlayer)
            {
                int maxEval = int.MinValue;
                for (int i = 0; i < 15; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        if (board[i, j] == SaveValues.EmptyCell)
                        {
                            // Thử nước đi cho máy tính (chế độ Maximizing)
                            board[i, j] = SaveValues.AiPlayer;
                            (int eval, _) = FindBestMove(board, depth - 1, alpha, beta, false);
                            board[i, j] = SaveValues.EmptyCell;

                            if (eval > maxEval)
                            {
                                maxEval = eval;
                                bestMove = new Point(i, j); // Cập nhật tọa độ nước đi tốt nhất
                            }

                            alpha = Math.Max(alpha, eval);
                            if (beta <= alpha)
                            {
                                // Cắt tỉa alpha-beta
                                break;
                            }
                        }
                    }
                }
                return (maxEval, bestMove);
            }
            else
            {
                int minEval = int.MaxValue;
                for (int i = 0; i < 15; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        if (board[i, j] == SaveValues.EmptyCell)
                        {
                            // Thử nước đi cho người chơi (chế độ Minimizing)
                            board[i, j] = SaveValues.HumanPlayer;
                            (int eval, _) = FindBestMove(board, depth - 1, alpha, beta, true);
                            board[i, j] = SaveValues.EmptyCell;

                            if (eval < minEval)
                            {
                                minEval = eval;
                                bestMove = new Point(i, j); // Cập nhật tọa độ nước đi tốt nhất
                            }

                            beta = Math.Min(beta, eval);
                            if (beta <= alpha)
                            {
                                // Cắt tỉa alpha-beta
                                break;
                            }
                        }
                    }
                }
                return (minEval, bestMove);
            }
        }
        private Point GetBestMove(int[,] board, int depth, int alpha, int beta, bool isMaximizingPlayer)
        {
            (_, Point bestMove) = FindBestMove(board, depth, alpha, beta, isMaximizingPlayer);
            return bestMove;
        }

        private bool IsGameOver(int[,] board)
        {
            // Kiểm tra hàng ngang
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col <= board.GetLength(1) - 5; col++)
                {
                    int player = board[row, col];
                    if (player != 0 &&
                        player == board[row, col + 1] &&
                        player == board[row, col + 2] &&
                        player == board[row, col + 3] &&
                        player == board[row, col + 4])
                    {
                        return true; // Có người chiến thắng trong hàng ngang
                    }
                }
            }

            // Kiểm tra hàng dọc
            for (int row = 0; row <= board.GetLength(0) - 5; row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    int player = board[row, col];
                    if (player != 0 &&
                        player == board[row + 1, col] &&
                        player == board[row + 2, col] &&
                        player == board[row + 3, col] &&
                        player == board[row + 4, col])
                    {
                        return true; // Có người chiến thắng trong hàng dọc
                    }
                }
            }

            // Kiểm tra đường chéo chính (từ trái trên đến phải dưới)
            for (int row = 0; row <= board.GetLength(0) - 5; row++)
            {
                for (int col = 0; col <= board.GetLength(1) - 5; col++)
                {
                    int player = board[row, col];
                    if (player != 0 &&
                        player == board[row + 1, col + 1] &&
                        player == board[row + 2, col + 2] &&
                        player == board[row + 3, col + 3] &&
                        player == board[row + 4, col + 4])
                    {
                        return true; // Có người chiến thắng trên đường chéo chính
                    }
                }
            }

            // Kiểm tra đường chéo phụ (từ phải trên đến trái dưới)
            for (int row = 0; row <= board.GetLength(0) - 5; row++)
            {
                for (int col = 4; col < board.GetLength(1); col++)
                {
                    int player = board[row, col];
                    if (player != 0 &&
                        player == board[row + 1, col - 1] &&
                        player == board[row + 2, col - 2] &&
                        player == board[row + 3, col - 3] &&
                        player == board[row + 4, col - 4])
                    {
                        return true; // Có người chiến thắng trên đường chéo phụ
                    }
                }
            }

            // Kiểm tra xem còn ô trống trên bàn cờ hay không
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == 0)
                    {
                        return false; // Còn ô trống trên bàn cờ
                    }
                }
            }

            return true; // Không còn ô trống trên bàn cờ, trò chơi kết thúc hòa
        }

        // Hàm đánh giá điểm số cho bàn cờ
        private int EvaluateBoard(int[,] board)
        {
            int[] rows = new int[board.GetLength(0)];
            int[] cols = new int[board.GetLength(1)];
            int diag1 = 0; // Điểm đường chéo từ trái lên phải
            int diag2 = 0; // Điểm đường chéo từ phải lên trái

            // Duyệt qua các hàng và cột để tính tổng điểm cho từng hàng và cột
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    int cellValue = board[i, j];
                    rows[i] += cellValue;
                    cols[j] += cellValue;

                    // Điểm đường chéo từ trái lên phải
                    if (i == j)
                        diag1 += cellValue;

                    // Điểm đường chéo từ phải lên trái
                    if (i + j == board.GetLength(0) - 1)
                        diag2 += cellValue;
                }
            }

            // Kiểm tra nếu có ai thắng (5 ô liên tiếp cùng giá trị)
            int maxScore = int.MaxValue - 1;
            int minScore = int.MinValue + 1;
            if (rows.Contains(maxScore) || cols.Contains(maxScore) || diag1 == maxScore || diag2 == maxScore)
            {
                return maxScore; // Người chơi X thắng
            }
            else if (rows.Contains(minScore) || cols.Contains(minScore) || diag1 == minScore || diag2 == minScore)
            {
                return minScore; // Người chơi O thắng
            }

            // Nếu không có ai thắng, tính điểm số dựa trên tổng giá trị các ô
            int score = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    score += board[i, j];
                }
            }

            return score;
        }


        // Hàm trả về một nước đi ngẫu nhiên trên bàn cờ chưa được đánh
        private Point GetRandomMove(int[,] board)
        {
            Random random = new Random();

            int size = board.GetLength(0);
            List<Point> availableMoves = new List<Point>();

            // Tìm các ô trống trên bàn cờ
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] == 0)
                    {
                        availableMoves.Add(new Point(i, j));
                    }
                }
            }

            // Chọn một nước đi ngẫu nhiên từ danh sách các ô trống
            int randomIndex = random.Next(availableMoves.Count);
            return availableMoves[randomIndex];
        }
        private Button GetButtonFromMatrix(List<List<Button>> Matrix, Point coordinates)
        {
            int row = coordinates.X;
            int column = coordinates.Y;

            if (row >= 0 && row < Matrix.Count && column >= 0 && column < Matrix[row].Count)
            {
                return Matrix[row][column];
            }

            return null;
        }


        public void AutoPlay( int cnt, int gameCount)
        {
            while (cnt < gameCount)
            {
                bool swap = false; // X : 0 true
                                 //  O : 1 false
                // Khởi tạo bàn cờ
                int[,] board = new int[15, 15];
                Point xMove = GetRandomMove(board);
                // MessageBox.Show(xMove.X.ToString() + ", " + xMove.Y.ToString());
                //break;

                board[xMove.X, xMove.Y] = currentPlayer;
                // Lấy button từ ma trận các button theo tọa độ cần đánh
                Button c = GetButtonFromMatrix(Matrix, xMove);
                HandleButtonClick(c);

                Point newPoint = new Point(xMove.X + 1, xMove.Y);
                Button d = GetButtonFromMatrix(Matrix, newPoint);
                HandleButtonClick(d);
                board[newPoint.X, newPoint.Y] = currentPlayer;

                //// Lấy button từ ma trận các button theo tọa độ cần đánh
                //Button c = GetButtonFromMatrix(Matrix, xMove);
                //HandleButtonClick(c);

                // Quá trình máy tự chơi với chính nó
                while (true)
                {
                    // Máy chơi (lượt O)
                    Point oMove = GetBestMove(board, SaveValues.Depth, int.MinValue, int.MaxValue, swap);
                   // board[oMove.X/30, oMove.Y/30] = currentPlayer;
                    // Lấy button từ ma trận các button theo tọa độ cần đánh
                   MessageBox.Show(oMove.X.ToString() + ", " + oMove.Y.ToString());
                   break;
                    Button b = GetButtonFromMatrix(Matrix, oMove);
                    HandleButtonClick(b);

                    //if (IsGameOver(board) == true)
                    //{
                    //    NewGame();
                    //    break;
                    //}
                    swap = !swap;
                }
                ++cnt;
            }
        }
    }
}
