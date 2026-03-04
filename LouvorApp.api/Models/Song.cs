namespace LouvorApp.Api.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string OriginalKey { get; set; } = "C"; // Tom original (C, D, G...)
        
        // A cifra bruta com o texto (Padrão ChordPro)
        public string RawChordText { get; set; } = string.Empty; 

        // Relacionamento N:N
        public ICollection<SetlistSong> SetlistSongs { get; set; } = new List<SetlistSong>();
    }
}