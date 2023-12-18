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
    public class EditorController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public EditorController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<EditorDto>>> GetEditors()
        {
            var Editors = await _dbContext.Editors.ToListAsync();
            var EditorsDto = _mapper.Map<List<EditorDto>>(Editors);
            return Ok(EditorsDto);
        }

        [HttpPost]
        public async Task<ActionResult<EditorDto>> AddEditor([FromBody] EditorDto EditorDto)
        {
            if (EditorDto == null)
            {
                return BadRequest();
            }

            var Editor = _mapper.Map<Editor>(EditorDto);

            _dbContext.Editors.Add(Editor);
            await _dbContext.SaveChangesAsync();

            return Created("Editor", _mapper.Map<EditorDto>(Editor));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEditor(int id, [FromBody] EditorDto EditorDto)
        {
            if (id != EditorDto.Id)
            {
                return BadRequest();
            }

            var Editor = await _dbContext.Editors.FindAsync(id);
            if (Editor == null)
            {
                return NotFound();
            }

            _mapper.Map(EditorDto, Editor);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EditorExists(id))
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
        public async Task<IActionResult> DeleteEditor(int id)
        {
            var Editor = await _dbContext.Editors.FindAsync(id);
            if (Editor == null)
            {
                return NotFound();
            }

            _dbContext.Editors.Remove(Editor);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool EditorExists(int id)
        {
            return _dbContext.Editors.Any(e => e.Id == id);
        }
    }
}