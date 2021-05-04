using Accord.IO;
using Accord.Neuro;
using BLL.Services.Interfaces;
using Deedle;

namespace BLL.Services.Implementations
{
    public class DataService : IDataService
    {
        private const string ValidationDataFileName = "mnist_test.csv";
        private const string TrainigDataFileName = "mnist_train.csv";
        private const string NetworkStateFileName = "trained_network.state";

        private readonly IFileService _fileService;

        public DataService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public Frame<int, string> GetValidationData(int? maxRows = null)
        {
            return _fileService.GetDataFromFile(ValidationDataFileName, maxRows);
        }

        public Frame<int, string> GetTrainingData(int? maxRows = null)
        {
            return _fileService.GetDataFromFile(TrainigDataFileName, maxRows);
        }

        public void SaveNetworkState(ActivationNetwork network)
        {
            string fileName = _fileService.CreateFilePath(NetworkStateFileName);
            Serializer.Save(network, fileName);
        }
    }
}