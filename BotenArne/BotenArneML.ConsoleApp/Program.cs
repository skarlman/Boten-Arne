// This file was auto-generated by ML.NET Model Builder. 

using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using BotenArneML.Model;

namespace BotenArneML.ConsoleApp
{
    class Program
    {
        //Dataset to use for predictions 
        private const string DATA_FILEPATH = @"C:\code\streaming\lunchmed.net\repositories\Boten-Arne\BotenArne\GemGymmet\Data\ClippyTrainingData.tsv";

        static void Main(string[] args)
        {

            var predEngine = ConsumeModel.GetPredictionEngine();

            TestOnOtherSRT(predEngine);


            Console.WriteLine("Slut.");
            return;

            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = CreateSingleDataSample(DATA_FILEPATH);

            // Make a single prediction on the sample data and print results
            ModelOutput predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual Action with predicted Action from sample data...\n\n");
            Console.WriteLine($"Subtitle: {sampleData.Subtitle}");
            Console.WriteLine($"\n\nActual Action: {sampleData.Action} \nPredicted Action value {predictionResult.Prediction} \nPredicted Action scores: [{String.Join(",", predictionResult.Score)}]\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }

        public static void TestOnOtherSRT(PredictionEngine<ModelInput, ModelOutput> predictionEngine)
        {
            var allLines = File.ReadAllLines("Data\\zQmDgww8-XY.srt");

            var subtitles = allLines.Where((l, i) => (i + 2) % 4 == 0).Distinct().ToArray();

            foreach (var s in subtitles)
            {
                var prediction = predictionEngine.Predict(new ModelInput() { Subtitle = s });
                if (prediction.Prediction != "CheckingSomething") // || prediction.Score.Max() > 0.95f)
                {
                    Console.WriteLine($"({prediction.Score.Max():P}) {prediction.Prediction} <== {s}");
                }
            }
        }


        // Change this code to create your own sample data
        #region CreateSingleDataSample
        // Method to load single row of dataset to try a single prediction
        private static ModelInput CreateSingleDataSample(string dataFilePath)
        {
            // Create MLContext
            MLContext mlContext = new MLContext();

            // Load dataset
            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                            path: dataFilePath,
                                            hasHeader: true,
                                            separatorChar: '\t',
                                            allowQuoting: true,
                                            allowSparse: false);

            // Use first line of dataset as model input
            // You can replace this with new test data (hardcoded or from end-user application)
            ModelInput sampleForPrediction = mlContext.Data.CreateEnumerable<ModelInput>(dataView, false)
                                                                        .First();
            return sampleForPrediction;
        }
        #endregion
    }
}
