namespace SlotMachineUI.Helpers;

public static class SymbolHelper
{
    /// <summary>
    /// Converts a spin result string to emoji representation
    /// </summary>
    /// <param name="spinResult">Comma-separated symbol names</param>
    /// <returns>Space-separated emojis</returns>
    public static string GetSymbolEmojis(string spinResult)
    {
        if (string.IsNullOrEmpty(spinResult))
            return "❓❓❓";

        var symbols = spinResult.Split(',');
        var emojis = new List<string>();
        
        foreach (var symbol in symbols)
        {
            emojis.Add(GetSymbolEmoji(symbol.Trim()));
        }
        
        return string.Join(" ", emojis);
    }

    /// <summary>
    /// Converts a single symbol name to its emoji representation
    /// </summary>
    /// <param name="symbolName">The symbol name to convert</param>
    /// <returns>The corresponding emoji</returns>
    public static string GetSymbolEmoji(string symbolName)
    {
        return symbolName switch
        {
            "Nine" => "9️⃣",
            "Ten" => "🔟",
            "Jack" => "👨‍⚖️",
            "Queen" => "👸",
            "King" => "👑",
            "Ace" => "🅰️",
            "Bonus" => "🎁",
            "Jackpot" => "💰",
            _ => "❓"
        };
    }
} 