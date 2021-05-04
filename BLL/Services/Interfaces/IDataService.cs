using Accord.Neuro;
using Deedle;

namespace BLL.Services.Interfaces
{
    public interface IDataService
    {
        Frame<int, string> GetValidationData(int? maxRows = null);

        Frame<int, string> GetTrainingData(int? maxRows = null);

        void SaveNetworkState(ActivationNetwork network);
    }
}