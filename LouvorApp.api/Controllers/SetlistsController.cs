using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LouvorApp.Api.Data;
using LouvorApp.Api.Models;

namespace LouvorApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetlistsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SetlistsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Setlists (Traz todos os repertórios com suas músicas)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Setlist>>> GetSetlists()
        {
            return await _context.Setlists
                .Include(s => s.SetlistSongs)
                .ThenInclude(ss => ss.Song)
                .ToListAsync();
        }

        // POST: api/Setlists (Cria um repertório novo)
        [HttpPost]
        public async Task<ActionResult<Setlist>> PostSetlist(Setlist setlist)
        {
            _context.Setlists.Add(setlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSetlists), new { id = setlist.Id }, setlist);
        }
    }
}