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
        var prompt = "Você é um assistente de geração de código toda vez" +
            "que eu te der um pseudocódigo no formato yaml, vc ira me devolver "
            + "somente e apenas somente o código resultante. me devolva apenas o código. sem explicações adicionais. meu primeiro prompt: {0}";
        return await this.client.Ask(string.Format(prompt,text), "pseudo");
    }

    public async Task ResetPseudo(){
        await this.client.ResetConversation("pseudo");
        await this.client.Ask("Você é um assistente de geração de código toda vez"+
            "que eu te der um pseudocódigo no formato yaml, vc ira me devolver "
            +"somente e apenas somente o código resultante. me devolva apenas o código. sem explicações adicionais. meu primeiro prompt: "
            +"language: csharp\n\tsample: hello-world\n", "pseudo");
    }
}
