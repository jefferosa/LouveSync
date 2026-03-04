using Xunit;
using LouvorApp.api.Utils;
using System.Linq;

namespace LouvorApp.Tests
{
    public class ChordParserTests
    {
        [Fact]
        public void Should_Parse_ChordPro_Inside_Words()
        {
            // O texto que o usuário salva no banco
            string input = "A[C]leluia, [G]Amém"; 
            
            var result = ChordParser.Parse(input);

            // Verifica se gerou apenas 1 linha
            Assert.Single(result); 
            var line = result.First();
            
            // Verifica se cortou em 3 pedaços: "A", "[C]leluia, ", "[G]Amém"
            Assert.Equal(3, line.Segments.Count);
            
            // Pedaço 1
            Assert.Equal("", line.Segments[0].Chord);
            Assert.Equal("A", line.Segments[0].Lyric);
            
            // Pedaço 2
            Assert.Equal("C", line.Segments[1].Chord);
            Assert.Equal("leluia, ", line.Segments[1].Lyric);
            
            // Pedaço 3
            Assert.Equal("G", line.Segments[2].Chord);
            Assert.Equal("Amém", line.Segments[2].Lyric);
        }

        [Fact]
        public void Should_Respect_Empty_Spaces()
        {
            // [Item 3 do Checklist]: Testando se os espaços não somem
            string input = "[C]   Espaços   [D]   ";
            
            var result = ChordParser.Parse(input);
            var line = result.First();

            Assert.Equal("C", line.Segments[0].Chord);
            Assert.Equal("   Espaços   ", line.Segments[0].Lyric);
            
            Assert.Equal("D", line.Segments[1].Chord);
            Assert.Equal("   ", line.Segments[1].Lyric);
        }
    }
}