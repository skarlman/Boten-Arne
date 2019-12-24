using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.ML;

namespace GemGymmet
{
    public class ArneActionService
    {
        private static MLContext _mlContext;
        private PredictionEngine<SubtitleEntity, ArneActionPrediction> _predEngine;

        private static string _modelPath => Path.Combine("Models", "model.zip");


        public ArneActionService()
        {
            _mlContext = new MLContext(seed: 0);

            ITransformer loadedModel = _mlContext.Model.Load(_modelPath, out var modelInputSchema);
            _predEngine = _mlContext.Model.CreatePredictionEngine<SubtitleEntity, ArneActionPrediction>(loadedModel);
        }

        public string PredictAction(string text)
        {
            var subtitleEntity = new SubtitleEntity(){Subtitle = text};

            var prediction = _predEngine.Predict(subtitleEntity);
            return prediction.Action;
        }
    }
}
