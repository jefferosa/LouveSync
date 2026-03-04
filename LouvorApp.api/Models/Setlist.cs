namespace LouvorApp.Api.Models
{
    public class Setlist
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; // Ex: "Culto de Domingo - Manhã"
        public DateTime EventDate { get; set; }
        
        // Relacionamento N:N
        public ICollection<SetlistSong> SetlistSongs { get; set; } = new List<SetlistSong>();
    }
}