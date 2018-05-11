using Microsoft.ML.Runtime.Api;

namespace api
{
    public class IrisPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabels;
    }
}