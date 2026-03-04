using System.Text.RegularExpressions;

namespace LouvorApp.api.Utils
{
    public static class ChordTransposer
    {
        // O array circular (Módulo 12) com a escala cromática
        private static readonly string[] Scale = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        
        // Escala alternativa para lidar com bemóis (b)
        private static readonly string[] FlatScale = { "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B" };

        public static string Transpose(string chord, int shiftAmount)
        {
            if (string.IsNullOrWhiteSpace(chord)) return chord;

            if (chord.Contains('/'))
            {
                var parts = chord.Split('/');
                var transposedMain = TransposeSingleChord(parts[0], shiftAmount);
                var transposedBass = TransposeSingleChord(parts[1], shiftAmount);
                
                // Junta novamente após calcular os dois lados
                return $"{transposedMain}/{transposedBass}"; 
            }

            return TransposeSingleChord(chord, shiftAmount);
        }

        private static string TransposeSingleChord(string chord, int shiftAmount)
        {
            var match = Regex.Match(chord, @"^([A-G][#b]?)(.*)$");
            
            if (!match.Success) return chord; // Se não for uma cifra válida, devolve intacta

            string rootNote = match.Groups[1].Value;
            string suffix = Regex.Replace(match.Groups[2].Value, @"[^a-zA-Z0-9\(\)\+\-]", "");

            // Define qual escala usar baseada no tom atual
            var scaleToUse = rootNote.Contains('b') ? FlatScale : Scale;
            int currentIndex = Array.IndexOf(scaleToUse, rootNote);
            
            // Fallback caso a cifra tenha um bemol mas estejamos buscando no sustenido
            if (currentIndex == -1) 
                currentIndex = Array.IndexOf(rootNote.Contains('b') ? Scale : FlatScale, rootNote);

            if (currentIndex == -1) return chord;

            // A Mágica Matemática da Transposição (Lida com números negativos e positivos)
            int newIndex = (currentIndex + shiftAmount) % 12;
            if (newIndex < 0) newIndex += 12;

            return $"{scaleToUse[newIndex]}{suffix}";
        }
    }
}