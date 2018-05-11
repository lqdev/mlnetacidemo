using Microsoft.ML.Runtime.Api;

namespace model
{
    public class IrisPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabels;
    }
}