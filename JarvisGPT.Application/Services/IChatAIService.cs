namespace JarvisGPT.Application;

public interface IChatAIService
{
    public Task<string> AskCli(string text);
    public Task<string> AskPseudo(string text);
    public Task ResetPseudo();
}
