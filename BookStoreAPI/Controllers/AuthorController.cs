using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreAPI.Entities;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AuthorController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorDto>>> GetAuthors()
        {
            var Authors = await _dbContext.Authors.ToListAsync();
            var AuthorsDto = _mapper.Map<List<AuthorDto>>(Authors);
            return Ok(AuthorsDto);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> AddAuthor([FromBody] AuthorDto AuthorDto)
        {
            if (AuthorDto == null)
            {
                return BadRequest();
            }

            var Author = _mapper.Map<Author>(AuthorDto);

            _dbContext.Authors.Add(Author);
            await _dbContext.SaveChangesAsync();

            return Created("Author", _mapper.Map<AuthorDto>(Author));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorDto AuthorDto)
        {
            if (id != AuthorDto.Id)
            {
                return BadRequest();
            }

            var Author = await _dbContext.Authors.FindAsync(id);
            if (Author == null)
            {
                return NotFound();
            }

            _mapper.Map(AuthorDto, Author);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var Author = await _dbContext.Authors.FindAsync(id);
            if (Author == null)
            {
                return NotFound();
            }

            _dbContext.Authors.Remove(Author);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _dbContext.Authors.Any(e => e.Id == id);
        }
    }
}