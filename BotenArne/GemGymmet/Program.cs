using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace GemGymmet
{
    class Program
    {

        private static string _appPath => "";
        private static string _trainDataPath => Path.Combine(_appPath, "Data\\ClippyTrainingData.tsv");
        private static string _testDataPath => Path.Combine(_appPath, "Data\\ClippyTrainingData.tsv"); //don't do this
        private static string _modelPath => Path.Combine(_appPath, "Models", "model.zip");

        private static MLContext _mlContext;

        private static PredictionEngine<SubtitleEntity, ArneActionPrediction> _predEngine;
       
        private static ITransformer _trainedModel;
        static IDataView _trainingDataView;


        static void Main(string[] args)
        {
            Console.WriteLine("Boten Arnes Gem-gym");

            _mlContext = new MLContext(seed: 0);
            _trainingDataView = _mlContext.Data.LoadFromTextFile<SubtitleEntity>(_trainDataPath, hasHeader: true);

            var pipeline = ProcessData();

            var trainingPipeline = BuildAndTrainModel(_trainingDataView, pipeline);

            
            Evaluate(_trainingDataView.Schema);

        }

        public static IEstimator<ITransformer> BuildAndTrainModel(IDataView trainingDataView, IEstimator<ITransformer> pipeline)
        {
            var trainingPipeline = pipeline.Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            _trainedModel = trainingPipeline.Fit(trainingDataView);

            _predEngine = _mlContext.Model.CreatePredictionEngine<SubtitleEntity, ArneActionPrediction>(_trainedModel);

            SubtitleEntity issue = new SubtitleEntity()
            {
                Action= "Congratulate",
                Subtitle= "Tycker jag i alla fall men"
            };

            var prediction = _predEngine.Predict(issue);

            Console.WriteLine($"=============== Single Prediction just-trained-model - Result: {prediction.Action} ===============");

            return trainingPipeline;
        }

        public static void Evaluate(DataViewSchema trainingDataViewSchema)
        {
            var testDataView = _mlContext.Data.LoadFromTextFile<SubtitleEntity>(_testDataPath, hasHeader: true);

            var testMetrics = _mlContext.MulticlassClassification.Evaluate(_trainedModel.Transform(testDataView));

            Console.WriteLine($"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for Multi-class Classification model - Test Data     ");
            Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"*       MicroAccuracy:    {testMetrics.MicroAccuracy:0.###}");
            Console.WriteLine($"*       MacroAccuracy:    {testMetrics.MacroAccuracy:0.###}");
            Console.WriteLine($"*       LogLoss:          {testMetrics.LogLoss:#.###}");
            Console.WriteLine($"*       LogLossReduction: {testMetrics.LogLossReduction:#.###}");
            Console.WriteLine($"*************************************************************************************************************");


            SaveModelAsFile(_mlContext, trainingDataViewSchema, _trainedModel);
        }

        private static void SaveModelAsFile(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            mlContext.Model.Save(model, trainingDataViewSchema, _modelPath);
        }

        public static IEstimator<ITransformer> ProcessData()
        {
            var pipeline =
                _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Action", outputColumnName: "Label")
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Subtitle", outputColumnName: "SubtitleFeaturized"))
                .Append(_mlContext.Transforms.Concatenate("Features", "SubtitleFeaturized"))
                .AppendCacheCheckpoint(_mlContext);

            return pipeline;
        }


    }


    internal class ArneActionPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Action;
    }

    internal class SubtitleEntity
    {
        [LoadColumn(0)]
        public string Action { get; set; }
        [LoadColumn(1)]
        public string Subtitle { get; set; }
    }
}
