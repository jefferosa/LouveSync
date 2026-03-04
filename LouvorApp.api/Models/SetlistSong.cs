namespace LouvorApp.Api.Models
{
    public class SetlistSong
    {
        public int SongId { get; set; }
        public Song Song { get; set; } = null!;

        public int SetlistId { get; set; }
        public Setlist Setlist { get; set; } = null!;

        // Ordem da música no repertório (Essencial para o Drag and Drop depois)
        public int SortOrder { get; set; }
        
        // Bônus Arquitetural: Se a banda decidir tocar a música em um tom 
        // diferente do original NESTE culto, salvamos a transposição aqui!
        public int? TransposeShift { get; set; } 
    }
}