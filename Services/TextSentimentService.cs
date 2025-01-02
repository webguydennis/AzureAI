using Azure;
using Azure.AI.TextAnalytics;
using System;

namespace AzureAI.Services {
    public class TextSentimentService : ITextSentimentService {
        
        private readonly TextAnalyticsClient _client;
        
        public TextSentimentService(string endpoint, string apiKey)
        {
            var credentials = new AzureKeyCredential(apiKey);
            _client = new TextAnalyticsClient(new Uri(endpoint), credentials);
        }

        public string DetectSentiment(string text)
        {
            try
            {
                var response = _client.AnalyzeSentiment(text);
                return $"The sentiment of the text is: {response.Value.Sentiment}";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}