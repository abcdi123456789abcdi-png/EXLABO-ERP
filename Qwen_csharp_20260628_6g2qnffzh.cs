using Azure.AI.OpenAI;
using EXLABO.Core.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace EXLABO.Core.Services
{
    public class AIVisionService
    {
        private readonly string _apiKey;
        private readonly string _model;

        public AIVisionService(IConfiguration config)
        {
            _apiKey = config["OpenAI:ApiKey"] ?? string.Empty;
            _model = config["OpenAI:Model"] ?? "gpt-4o";
        }

        public async Task<List<ExtractedItem>> AnalyzeOrderImageAsync(byte[] imageBytes, string imageFormat = "image/jpeg")
        {
            var client = new OpenAIClient(_apiKey);
            var chatClient = client.GetChatClient(_model);

            string systemPrompt = @"You are an expert in laboratory inventory management. 
Extract ALL materials and quantities from this order image.
Return ONLY JSON array: [{""materialName"": ""CEA"", ""quantity"": 1}]";

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(systemPrompt),
                new UserChatMessage("Analyze this order", 
                    ChatMessageContentPart.CreateImagePart(
                        BinaryData.FromBytes(imageBytes, $"image/{imageFormat}")))
            };

            var response = await chatClient.CompleteChatAsync(messages);
            string responseText = response.Value.Content[0].Text;

            responseText = responseText.Trim();
            if (responseText.StartsWith("```json")) responseText = responseText[7..];
            if (responseText.StartsWith("```")) responseText = responseText[3..];
            if (responseText.EndsWith("```")) responseText = responseText[..^3];

            return JsonSerializer.Deserialize<List<ExtractedItem>>(responseText,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<ExtractedItem>();
        }
    }
}