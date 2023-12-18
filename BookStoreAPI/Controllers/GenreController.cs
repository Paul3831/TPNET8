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
    public class GenreController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GenreController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenreDto>>> GetGenres()
        {
            var Genres = await _dbContext.Genres.ToListAsync();
            var GenresDto = _mapper.Map<List<GenreDto>>(Genres);
            return Ok(GenresDto);
        }

        [HttpPost]
        public async Task<ActionResult<GenreDto>> AddGenre([FromBody] GenreDto GenreDto)
        {
            if (GenreDto == null)
            {
                return BadRequest();
            }

            var Genre = _mapper.Map<Genre>(GenreDto);

            _dbContext.Genres.Add(Genre);
            await _dbContext.SaveChangesAsync();

            return Created("Genre", _mapper.Map<GenreDto>(Genre));
        }
    }
}