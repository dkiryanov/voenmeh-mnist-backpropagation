using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Math.Optimization.Losses;
using Accord.Neuro;
using BLL.Models;
using BLL.Services.Interfaces;
using Deedle;

namespace BLL.Services.Implementations
{
    public class NetworkValidationService : INetworkValidationService
    {
        public double Validate(ActivationNetwork network, Frame<int, string> validation, double[][] labels)
        {
            double[][] expected = new double[validation.RowKeys.Count()][];
            double[][] actual = new double[validation.RowKeys.Count()][];

            double squareLoss = 0.0;

            foreach (int key in validation.RowKeys)
            {
                ObjectSeries<string> record = validation.Rows[key];
                int expectedDigit = (int)record.Values.First();
                double[] input = record.Values.Skip(1).Take(784).Select(Convert.ToDouble).ToArray();

                PredictionInfoModel prediction = ValidateSingleFeature(network, input);

                expected[key] = new double[] { expectedDigit };
                actual[key] = new double[] { prediction.Symbol };
            }

            return new SquareLoss(expected)
            {
                Root = false,
                Mean = false
            }.Loss(actual);
        }

        public PredictionInfoModel ValidateSingleFeature(ActivationNetwork network, double[] feature)
        {
            double[] predictions = network.Compute(feature);

            return Enumerable.Range(0, 10)
                .Select(v => new PredictionInfoModel
                {
                    Symbol = v,
                    Probability = predictions[v]
                })
                .OrderByDescending(v => v.Probability)
                .First();
        }

        public IEnumerable<PredictionInfoModel> BulkValidation(ActivationNetwork network, Frame<int, string> validation)
        {
            List<PredictionInfoModel> result = new List<PredictionInfoModel>(validation.RowKeys.Count());

            foreach (int key in validation.RowKeys)
            {
                ObjectSeries<string> record = validation.Rows[key];
                int expectedDigit = (int) record.Values.First();
                double[] input = record.Values.Skip(1).Take(784).Select(Convert.ToDouble).ToArray();

                PredictionInfoModel predictionResult = ValidateSingleFeature(network, input);
                predictionResult.ExpectedDigit = expectedDigit;

                predictionResult.IsCorrect = predictionResult.Symbol == expectedDigit;

                result.Add(predictionResult);
            }

            return result;
        }
    }
}