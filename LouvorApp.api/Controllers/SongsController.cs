using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LouvorApp.Api.Data;
using LouvorApp.Api.Models;

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
            // Começa a montar a consulta no banco
            var query = _context.Songs.AsQueryable();
        
            // Se o usuário digitou algo na busca, filtra por título ou artista
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.Title.Contains(search) || s.Artist.Contains(search));
            }
        
            // Aplica a paginação e executa a busca no banco
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

            // Retorna o status 201 Created e a música recém-criada
            return CreatedAtAction(nameof(GetSong), new { id = song.Id }, song);
        }
    }
}