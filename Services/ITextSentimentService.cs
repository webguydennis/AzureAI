namespace AzureAI.Services {
    public interface ITextSentimentService {
        public string DetectSentiment(string text);
    }
}