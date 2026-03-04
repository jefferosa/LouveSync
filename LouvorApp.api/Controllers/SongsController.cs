using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LouvorApp.Api.Data;
using LouvorApp.Api.Models;
using LouvorApp.api.Utils; // Importa as nossas ferramentas mágicas!

namespace LouvorApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SongsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Songs (Lista com busca e paginação)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs(
            [FromQuery] string? search, 
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Songs.AsQueryable();
        
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.Title.Contains(search) || s.Artist.Contains(search));
            }
        
            var songs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        
            return Ok(songs);
        }

        // GET: api/Songs/5 (Busca uma música específica pelo ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Song>> GetSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            return song;
        }

        // POST: api/Songs (Cria uma música nova)
        [HttpPost]
        public async Task<ActionResult<Song>> PostSong(Song song)
        {
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSong), new { id = song.Id }, song);
        }

        // 🚀 O NOVO ENDPOINT DE PALCO: GET /api/Songs/5/play?shift=2
        [HttpGet("{id}/play")]
        public async Task<ActionResult<object>> PlaySong(int id, [FromQuery] int shift = 0)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            // 1. Usa o Parser para fatiar o texto bruto em blocos de Letra/Cifra
            var parsedLines = ChordParser.Parse(song.RawChordText);

            // 2. Se o usuário pediu para mudar o tom, aplica a matemática em cada acorde
            if (shift != 0)
            {
                foreach (var line in parsedLines)
                {
                    foreach (var segment in line.Segments)
                    {
                        if (!string.IsNullOrEmpty(segment.Chord))
                        {
                            segment.Chord = ChordTransposer.Transpose(segment.Chord, shift);
                        }
                    }
                }
            }

            // 3. Monta um JSON bonitão e envelopado para o App consumir
            return Ok(new
            {
                song.Id,
                song.Title,
                song.Artist,
                OriginalKey = song.OriginalKey,
                AppliedShift = shift,
                Lines = parsedLines
            });
        }
    }
}