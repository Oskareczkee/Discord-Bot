using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
namespace Core.Services.AI
{
    public class AIService : IAIService
    {
        private readonly IConfiguration _config;

        //TODO add each server conversation context?
        private Queue<string> conversationContext;
        private const int QUEUE_MAX_MESSAGES=100;

        public AIService(IConfiguration config)
        {
            _config = config;
            conversationContext = new Queue<string>(QUEUE_MAX_MESSAGES);
        }
        public async Task<string> GenerateContent(string message)
        {
            var key = _config.GetSection("AIConfig:AIKey").Value;
            //not used yet
            //var model = _config.GetSection("AIConfig:Model").Value;

            OpenAIAPI api = new OpenAIAPI(new APIAuthentication(key));

            var conversation = api.Chat.CreateConversation();
            conversation.AppendExampleChatbotOutput("You are a discord bot, you have simple commands as well as RPG system");

            //add context of previous conversations
            foreach (var mess in conversationContext)
                conversation.AppendSystemMessage(mess);

            conversation.AppendUserInput(message);

            string response = await conversation.GetResponseFromChatbotAsync().ConfigureAwait(false);

            conversationContext.Enqueue(message);
            conversationContext.Enqueue(response);
            //if context reached its limit start deleting message and response
            if(conversationContext.Count > QUEUE_MAX_MESSAGES)
            {
                conversationContext.Dequeue();
                conversationContext.Dequeue();
            }    

            return response;
        }
    }
}
