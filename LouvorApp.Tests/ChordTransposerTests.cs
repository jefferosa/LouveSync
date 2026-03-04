using Xunit;
using LouvorApp.api.Utils;

namespace LouvorApp.Tests
{
    public class ChordTransposerTests
    {
        [Theory]
        [InlineData("C", 2, "D")]
        [InlineData("G", -2, "F")]
        [InlineData("C#m7", 1, "Dm7")]
        [InlineData("D/F#", 2, "E/G#")]
        [InlineData("Am7(b5)", 5, "Dm7(b5)")]
        public void Should_Transpose_Correctly(string originalChord, int shift, string expectedChord)
        {
            var result = ChordTransposer.Transpose(originalChord, shift);
            Assert.Equal(expectedChord, result);
        }

        [Theory]
        [InlineData("C")]
        [InlineData("F#m7")]
        [InlineData("G/B")]
        public void Shifting_12_Semitones_Should_Return_Original_Chord(string chord)
        {
            var resultUp = ChordTransposer.Transpose(chord, 12);
            var resultDown = ChordTransposer.Transpose(chord, -12);

            Assert.Equal(chord, resultUp);
            Assert.Equal(chord, resultDown);
        }
    }
}