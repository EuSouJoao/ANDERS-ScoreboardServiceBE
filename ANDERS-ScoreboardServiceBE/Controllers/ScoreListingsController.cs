using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ANDERS_ScoreboardServiceBE;
using ANDERS_ScoreboardServiceBE.Models;
using ANDERS_ScoreboardServiceBE.Data;
using Newtonsoft.Json;

namespace ANDERS_ScoreboardServiceBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreListingsController : ControllerBase
    {
        private readonly ScoreboardContext _context;
        private readonly IDataRepository<ScoreListing> _repo;

        public ScoreListingsController(ScoreboardContext context, IDataRepository<ScoreListing> repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: api/ScoreListings
        [HttpGet]
        public IActionResult GetScoreListing([FromQuery] Parameters parameters)
        {
            PagedList<ScoreListing> scores;
            switch (parameters.Sort)
            {
                case "name asc":
                    scores = PagedList<ScoreListing>.ToPagedList(_context.ScoreListing.OrderBy(p => p.Name),
                                    parameters.PageNumber,
                                    parameters.PageSize);
                    break;
                case "name desc":
                    scores = PagedList<ScoreListing>.ToPagedList(_context.ScoreListing.OrderByDescending(p => p.Name),
                                    parameters.PageNumber,
                                    parameters.PageSize);
                    break;
                case "score asc":
                    scores = PagedList<ScoreListing>.ToPagedList(_context.ScoreListing.OrderBy(p => p.Score),
                                    parameters.PageNumber,
                                    parameters.PageSize);
                    break;
                case "score desc":
                    scores = PagedList<ScoreListing>.ToPagedList(_context.ScoreListing.OrderByDescending(p => p.Score),
                                    parameters.PageNumber,
                                    parameters.PageSize);
                    break;
                default:
                    scores = PagedList<ScoreListing>.ToPagedList(_context.ScoreListing,
                                    parameters.PageNumber,
                                    parameters.PageSize);
                    break;
            }

            var metadata = new
            {
                scores.TotalCount,
                scores.PageSize,
                scores.CurrentPage,
                scores.TotalPages,
                scores.HasNext,
                scores.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(scores);
        }

        // GET: api/ScoreListings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScoreListing([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var scoreListing = await _context.ScoreListing.FindAsync(id);

            if (scoreListing == null)
            {
                return NotFound();
            }

            return Ok(scoreListing);
        }

        // PUT: api/ScoreListings/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScoreListing([FromRoute] Guid id, [FromBody] ScoreListing scoreListing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != scoreListing.ID)
            {
                return BadRequest();
            }

            _context.Entry(scoreListing).State = EntityState.Modified;

            try
            {
                _repo.Update(scoreListing);
                var save = await _repo.SaveAsync(scoreListing);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScoreListingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ScoreListings
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<IActionResult> PostScoreListing([FromBody] ScoreListing scoreListing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.Add(scoreListing);
            var save = await _repo.SaveAsync(scoreListing);

            return CreatedAtAction("GetScoreListing", new { id = scoreListing.ID }, scoreListing);
        }

        // DELETE: api/ScoreListings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScoreListing([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var scoreListing = await _context.ScoreListing.FindAsync(id);
            if (scoreListing == null)
            {
                return NotFound();
            }

            _repo.Delete(scoreListing);
            var save = await _repo.SaveAsync(scoreListing);

            return Ok(scoreListing);
        }

        private bool ScoreListingExists(Guid id)
        {
            return _context.ScoreListing.Any(e => e.ID == id);
        }
    }
}
