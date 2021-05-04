namespace BLL.Models
{
    public class PredictionInfoModel
    {
        public int Symbol { get; set; }

        public double Probability { get; set; }

        public bool? IsCorrect { get; set; }

        public int ExpectedDigit { get; set; }
    }
}