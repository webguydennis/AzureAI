using AzureAI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureAI.Controllers {
    
    [ApiController]
    [Route("api/[controller]")]
    public class CognitiveController : ControllerBase {
        private readonly ITextSentimentService _textSentimentService;
        private readonly ISpeechToTextService _speechToTextService;

        public CognitiveController(
            ITextSentimentService textSentimentService,
            ISpeechToTextService speechToTextService
        )
        {
            _textSentimentService = textSentimentService;
            _speechToTextService = speechToTextService;
        }

        [HttpPost("DetectTextSentiment")]
        public IActionResult DetectSentiment([FromBody] SentimentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest("Text cannot be empty.");
            }

            var result = _textSentimentService.DetectSentiment(request.Text);
            return Ok(new { Sentiment = result });
        }

        [HttpPost("SpeechToText")]
        public async Task<IActionResult> ConvertSpeechToText(IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("Invalid audio file.");
            }

            try
            {
                var tempFilePath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(tempFilePath))
                {
                    await audioFile.CopyToAsync(stream);
                }

                var text = await _speechToTextService.ConvertSpeechToTextAsync(tempFilePath);

                System.IO.File.Delete(tempFilePath); // Clean up the temp file

                return Ok(new { Text = text });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
    public class SentimentRequest
    {
        public string? Text { get; set; }
    }

}