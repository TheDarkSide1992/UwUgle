using Service.Interfaces;

namespace Service.Implementations;

public class CleanerString : ICleaner<string>
{
    public async Task<string> Clean(string message)
    {
        var header = new Char[] { ' ', '*', '.' };

        string input = "lala.bla";
        message = message.Split("X-").Last();
        int endHeader = message.IndexOf("\n", StringComparison.Ordinal);
        
        message = message.Remove(0, endHeader);


        //Console.WriteLine(message.Trim( header ));
        

        return message;
    }
}