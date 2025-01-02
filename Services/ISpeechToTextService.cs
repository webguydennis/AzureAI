namespace AzureAI.Services {
    public interface ISpeechToTextService {
        public Task<string> ConvertSpeechToTextAsync(string audioFilePath);
    }
}