using System.Text.RegularExpressions;
using Logger;
using Serilog;
using Service.Interface;

namespace Service;

public class WordIndexerHelper: IIndexer
{
    
    /**
     * This method 1½indexes the words in a given text
     */
    public Dictionary<string, int> Index(string text)
    {
        using var activity = Monitoring.ActivitySource.StartActivity();
        var index = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase); // Allows the dictionary to be case-insensitive so if there is a word like "this" and then later "This" then it still gets counted as the same word
            try
            {
                string[] words = Regex.Split(text, @"\W+"); // @"\W+" matches non word characters

                // Goes through each word in words and adds them to the index and if it is in the index the count goes up by one
                foreach (string word in words)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        if (index.ContainsKey(word))
                        {
                            index[word]++;
                        }
                        else
                        {
                            index.Add(word, 1);
                        }
                    }
                }
                return index;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("error while indexing words");
            }
        return null;
    }
}