using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Threading.Tasks;

namespace AzureAI.Services {
    public class SpeechToTextService : ISpeechToTextService {

        private readonly string _speechKey;
        private readonly string _region;

        public SpeechToTextService(string speechKey, string region)
        {
            _speechKey = speechKey ?? throw new ArgumentNullException(nameof(speechKey));
            _region = region ?? throw new ArgumentNullException(nameof(region));
        }

        public async Task<string> ConvertSpeechToTextAsync(string audioFilePath)
        {
            if (string.IsNullOrEmpty(audioFilePath))
                throw new ArgumentException("Audio file path cannot be null or empty", nameof(audioFilePath));

            var config = SpeechConfig.FromSubscription(_speechKey, _region);

            using var audioInput = AudioConfig.FromWavFileInput(audioFilePath);
            using var recognizer = new SpeechRecognizer(config, audioInput);

            var result = await recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                return result.Text;
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                throw new Exception("No speech could be recognized.");
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                throw new Exception($"Speech recognition canceled: {cancellation.Reason}. Error details: {cancellation.ErrorDetails}");
            }

            return string.Empty;
        }

    }
}