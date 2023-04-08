using System.Text.Json;
using ChatGPT.Net;
using ChatGPT.Net.DTO;
using ChatGPT.Net.Session;
using Microsoft.Extensions.Options;

namespace JarvisGPT.Application;

public class ChatGPTService : IChatAIService
{
    private readonly ChatGptClient client;

    public ChatGPTService(IOptions<ChatGptOptions> options)
    {
        var chatGpt = new ChatGpt(new ChatGptConfig{
            UseCache = true
        });
        chatGpt.WaitForReady().GetAwaiter().GetResult();
        this.client = chatGpt.CreateClient(new ChatGptClientConfig
        {
            SessionToken = options.Value.SessionToken,
            AccountType = ChatGPT.Net.Enums.AccountType.Pro,
            
        }).GetAwaiter().GetResult();
    }

    public async Task<string> AskCli(string text)
    {
        return await this.client.Ask(text,"default");
    }

    public async Task<string> AskPseudo(string text){
        var prompt = "You are a code assistant, and your capabilites are: refactor,"
        +"simplify, unit testing generation. You will answer only with the result code, ony the code on code block"
        +" with no aditional comments or descriptions, add comments only if I asked on my following prompt, otherwise no comments. my prompt: {0}";

        string result = await this.client.Ask(string.Format(prompt,text), "pseudo");

        return result.Replace("```", "");
    }

    public async Task ResetPseudo(){
        await this.client.ResetConversation("pseudo");
    }
}
