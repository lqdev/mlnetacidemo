using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;

namespace api.Controllers
{
    [Route("api/[controller]")]
    public class PredictController : Controller
    {
        // POST api/predict
        [HttpPost]
        public string Post([FromBody] IrisData instance)
        {
            var model = PredictionModel.ReadAsync<IrisData,IrisPrediction>("model.zip").Result;
            var prediction = model.Predict(instance);
            return prediction.PredictedLabels;
        }
    }
}