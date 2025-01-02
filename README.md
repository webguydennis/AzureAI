# Azure AI Demo - Cognitive Services

Azure AI is a comprehensive suite of artificial intelligence (AI) and machine learning (ML) tools and services provided by Microsoft Azure. It enables developers and businesses to build, deploy, and scale AI-driven applications in a seamless and secure manner. Azure AI empowers users to enhance their applications with capabilities like natural language understanding, computer vision, speech processing, and decision-making models.

### Prerequisites
 
We need a few nuget packages.<br />
```
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.Azure.CognitiveServices.Vision.ComputerVision
dotnet add package Azure.AI.TextAnalytics
dotnet add package Swashbuckle.AspNetCore
dotnet add package System.Net.Http.Json
dotnet add package Microsoft.CognitiveServices.Speech
```
<br />
Replace the language cognitive service endpoint and key, and Speech Service Key and Region in the appsettings.json.

```json
{
    "TextAnalytics": {
        "Endpoint": "https://<your-text-analytics-endpoint>.cognitiveservices.azure.com/",
        "ApiKey": "<your-text-analytics-key>"
    },
    "AzureSpeech": {
        "ApiKey": "YOUR_SPEECH_API_KEY",
        "Region": "YOUR_REGION"
    }
}
```

### Running Locally

Navigate to the AzureAI Project from the Terminal. See example to navigate when you in sms folder below. 
<br />
```
cd .\AzureAI\
```

Run the web api
<br />
```
dotnet run
```

Check the sentiment of a text whether the text is positive, neutral, or negative

```
curl -X 'POST' \
  'http://localhost:5000/api/Cognitive/DetectTextSentiment' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "text": "I am nervous about the stock market tomorrow."
}'
```

Get the text of a speech in a wav file. From Swagger, choose the Files/SpeechToTextDemo.wav file. From curl, try placing the SpeechToTextDemo.wav file. Run the curl command below.

```
curl -X 'POST' \
  'http://localhost:5000/api/Cognitive/SpeechToText' \
  -H 'accept: */*' \
  -H 'Content-Type: multipart/form-data' \
  -F 'audioFile=@SpeechToTextDemo.wav;type=audio/wav'
```