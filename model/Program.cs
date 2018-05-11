using System;
using Microsoft.ML;

namespace model
{
    class Program
    {
        static void Main(string[] args)
        {

            string dataPath = "model/data/iris.txt";

            string modelPath = "model/model.zip";

            var model = Model.Train(new LearningPipeline(),dataPath,modelPath).Result;

            // Test data for prediction
            var prediction = model.Predict(new IrisData() 
            {
                SepalLength = 3.3f,
                SepalWidth = 1.6f,
                PetalLength = 0.2f,
                PetalWidth = 5.1f
            });

            Console.WriteLine($"Predicted flower type is: {prediction.PredictedLabels}");
        }
    }
}
