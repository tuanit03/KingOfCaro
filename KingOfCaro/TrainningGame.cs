using Microsoft.ML.Data;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Microsoft.ML.Trainers.LightGbm;
using System.Drawing.Drawing2D;

namespace KingOfCaro
{
    public class TrainningGame
    {
        private ChessBoard chessBoard;
        public TrainningGame(ChessBoard chessBoard)
        {
            this.chessBoard = chessBoard;
        }

        // Định nghĩa class TrainingData để lưu trữ dữ liệu đào tạo
        public class TrainingData
        {
            [VectorType(46)] // 15x15 bàn cờ + 2 tọa độ nước đi
            public float[] Features;

            public bool Label;
        }

        // Định nghĩa class ResultPrediction để lưu trữ kết quả dự đoán
        public class ResultPrediction
        {
            [ColumnName("PredictedLabel")]
            public bool Prediction;

            [ColumnName("Probability")]
            public float[] Probability;
        }

        public class CaroModel
        {
            // Tạo mô hình ML.NET
            private static readonly MLContext mlContext = new MLContext();

            // Chọn mô hình DecisionTree (Cây quyết định) cho trò chơi Caro
            private static IEstimator<ITransformer> CreateModel()
            {
                var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
                    .Append(mlContext.Transforms.Concatenate("Features", nameof(TrainingData.Features)))
                    .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                    .Append(mlContext.Transforms.Conversion.MapKeyToValue("Label"));

                var trainer = mlContext.BinaryClassification.Trainers.LightGbm();
                var model = pipeline.Append(trainer);

                return model;
            }


            // Huấn luyện mô hình với dữ liệu đào tạo
            public static ITransformer TrainModel(List<TrainingData> trainingDataList)
            {
                // Chuyển dữ liệu sang IDataView
                var dataView = mlContext.Data.LoadFromEnumerable(trainingDataList);

                // Tạo pipeline
                var pipeline = CreateModel();

                // Huấn luyện mô hình
                var model = pipeline.Fit(dataView);

                return model;
            }

            // Dự đoán kết quả
            public static float Predict(ITransformer model, float[] features)
            {
                var predictionEngine = mlContext.Model.CreatePredictionEngine<TrainingData, ResultPrediction>(model);
                var data = new TrainingData { Features = features };
                var prediction = predictionEngine.Predict(data);
                return prediction.Probability[1]; // Return the probability of winning (class 1).
            }


            // Lưu mô hình vào tập tin
            public static void SaveModel(ITransformer model, string filePath)
            {
                mlContext.Model.Save(model, null, filePath);
            }

            // Tải mô hình từ tập tin
            public static ITransformer LoadModel(string filePath)
            {
                return mlContext.Model.Load(filePath, out _);
            }


            public static Point GetBestMove(ITransformer model, List<List<Button>> Matrix)
            {
                float bestPrediction = float.MinValue;
                Point bestMove = new Point(-1, -1);

                for (int i = 0; i < Matrix.Count; i++)
                {
                    for (int j = 0; j < Matrix[i].Count; j++)
                    {
                        if (Matrix[i][j].BackgroundImage == null)
                        {
                            // Tạo mảng đặc trưng cho nước đi (i, j)
                            float[] features = ConvertBoardAndMoveToFloatArray(Matrix, new List<Point> { new Point(i, j) });

                            // Dự đoán xác suất thắng của nước đi (i, j)
                            var prediction = Predict(model, features);

                            if (prediction > bestPrediction)
                            {
                                bestPrediction = prediction;
                                bestMove = new Point(i, j);
                            }
                        }
                    }
                }

                return bestMove;
            }

        }


        List<TrainingData> trainingDataList = new List<TrainingData>();
        public void RecordTrainingData(List<List<Button>> Matrix, List<Point> machineMove, bool isMachineWin)
        {
            // Chuyển đổi bàn cờ và nước đi của máy thành mảng float
            float[] features = ConvertBoardAndMoveToFloatArray(Matrix, machineMove);

            // Thêm dữ liệu vào list dữ liệu đào tạo
            trainingDataList.Add(new TrainingData
            {
                Features = features,
                Label = isMachineWin
            });
        }

        // Hàm để ghi lại các bước đi của mỗi trận đấu giữa hai máy chơi và lưu lại dưới dạng dữ liệu huấn luyện
        public static float[] ConvertBoardAndMoveToFloatArray(List<List<Button>> Matrix, List<Point> moves)
        {
            // Mã hóa bàn cờ thành mảng float
            float[] boardFeatures = new float[Matrix.Count * Matrix[0].Count];
            for (int i = 0; i < Matrix.Count; i++)
            {
                for (int j = 0; j < Matrix[i].Count; j++)
                {
                    float currentPlayer;
                    if (Matrix[i][j].BackgroundImage == null)
                    {
                        currentPlayer = -1.0f;
                    }
                    else
                    {
                        currentPlayer = (int)Matrix[i][j].Tag;
                    }
                    boardFeatures[i * Matrix[i].Count + j] = currentPlayer;
                }
            }

            // Mã hóa các nước đi thành mảng float
            float[] moveFeatures = new float[moves.Count * 2];
            for (int i = 0; i < moves.Count; i++)
            {
                moveFeatures[i * 2] = moves[i].X;
                moveFeatures[i * 2 + 1] = moves[i].Y;
            }

            // Kết hợp mảng bàn cờ và mảng nước đi thành mảng features hoàn chỉnh
            float[] features = new float[boardFeatures.Length + moveFeatures.Length];
            Array.Copy(boardFeatures, features, boardFeatures.Length);
            Array.Copy(moveFeatures, 0, features, boardFeatures.Length, moveFeatures.Length);

            return features;
        }

    }
}



