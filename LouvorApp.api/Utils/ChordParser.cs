using System.Text.RegularExpressions;

namespace LouvorApp.api.Utils
{
    // Representa um "pedacinho" da música. Ex: Cifra = "C", Letra = "leluia"
    public class ParsedSegment
    {
        public string Chord { get; set; } = string.Empty;
        public string Lyric { get; set; } = string.Empty;
    }

    // Representa uma linha inteira contendo vários segmentos
    public class ParsedLine
    {
        public List<ParsedSegment> Segments { get; set; } = new();
    }

    public static class ChordParser
    {
        // Essa Regex captura qualquer coisa que esteja entre colchetes [ ].
        private static readonly string Pattern = @"\[(.*?)\]";

        public static List<ParsedLine> Parse(string rawText)
        {
            var lines = new List<ParsedLine>();
            if (string.IsNullOrWhiteSpace(rawText)) return lines;

            // Divide o texto gigante em linhas, respeitando as quebras (\n)
            var rawLines = rawText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var rawLine in rawLines)
            {
                var parsedLine = new ParsedLine();
                
                // O Split com Regex separa exatamente onde estão os colchetes
                var parts = Regex.Split(rawLine, Pattern);

                if (parts.Length == 1)
                {
                    parsedLine.Segments.Add(new ParsedSegment { Lyric = parts[0] });
                }
                else
                {
                    // Adiciona o texto antes da primeira cifra (se houver)
                    if (!string.IsNullOrEmpty(parts[0]))
                    {
                        parsedLine.Segments.Add(new ParsedSegment { Lyric = parts[0] });
                    }

                    // Pula de 2 em 2 (Cifra, depois Letra)
                    for (int i = 1; i < parts.Length; i += 2)
                    {
                        parsedLine.Segments.Add(new ParsedSegment
                        {
                            // Separa Cifra e Letra no objeto
                            Chord = parts[i], 
                            Lyric = i + 1 < parts.Length ? parts[i + 1] : string.Empty 
                        });
                    }
                }
                lines.Add(parsedLine);
            }
            return lines;
        }
    }
}