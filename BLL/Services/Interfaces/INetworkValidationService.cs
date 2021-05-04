using System.Collections.Generic;
using Accord.Neuro;
using BLL.Models;
using Deedle;

namespace BLL.Services.Interfaces
{
    public interface INetworkValidationService
    {
        double Validate(ActivationNetwork network, Frame<int, string> validation, double[][] labels);

        IEnumerable<PredictionInfoModel> BulkValidation(ActivationNetwork network, Frame<int, string> validation);

        PredictionInfoModel ValidateSingleFeature(ActivationNetwork network, double[] feature);
    }
}