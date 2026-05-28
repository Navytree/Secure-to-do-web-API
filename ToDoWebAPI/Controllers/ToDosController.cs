using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ToDoUserWebAPI.Data;
using ToDoUserWebAPI.DTOs;
using ToDoUserWebAPI.Models;
using ToDoUserWebAPI.Extensions;

namespace ToDoUserWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly ToDoContext _context;

        public ToDosController(ToDoContext context)
        {
            _context = context; 
        }

        // GET: api/ToDos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowDTO>>> GetToDos()
        {
            int userId = HttpContext.GetUserIdFromToken();
            if (userId == 0) return Unauthorized();

            var today = DateOnly.FromDateTime(DateTime.Now);

            var todos = await _context.ToDos
                    .Where(i => i.UserId == userId && i.Done == false && i.DateTo >= today)
                    .Select(d => new ShowDTO
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Done = d.Done,
                        DateTo = d.DateTo
                    })
                    .ToListAsync();
            return Ok(todos);
        }

        // GET: api/ToDos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowDTO>> GetToDo(int id)
        {
            int userId = HttpContext.GetUserIdFromToken();
            if (userId == 0) return Unauthorized();

            var toDo = await _context.ToDos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (toDo == null)
            {
                return NotFound();
            }
            var dto = new ShowDTO
            {
                Id = toDo.Id,
                Name = toDo.Name,
                Done = toDo.Done,
                DateTo = toDo.DateTo
            };

            return Ok(dto); 
        }

        // POST: api/ToDos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDo>> PostToDo(PostDTO dto)
        {
            int userId = HttpContext.GetUserIdFromToken();
            if (userId == 0) return Unauthorized();

            var todo = new ToDo
            {
                UserId = userId,
                Name = dto.Name,
                Done = false,
                DateTo = dto.DateTo
            };
            _context.ToDos.Add(todo);
            await _context.SaveChangesAsync();

            var resultdto = new ShowDTO
            {
                Id = todo.Id,
                Name = todo.Name,
                Done = todo.Done,
                DateTo = todo.DateTo
            };
            return CreatedAtAction("GetToDo", new { id = resultdto.Id }, resultdto);
        }

        // PUT: api/ToDos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo([FromRoute] int id, [FromBody] PutDTO dto)
        {
            int userId = HttpContext.GetUserIdFromToken();  
            if (userId == 0) return Unauthorized();

            if (await IsTodoAlreadyDone(id, userId))
            {
                return BadRequest("Nie można modyfikować zakończonego zadania!");
            }

            var originaTodo = await _context.ToDos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (originaTodo == null) { return NotFound(); } 

            originaTodo.Name = dto.Name;
            originaTodo.Done = dto.Done;
            originaTodo.DateTo = dto.DateTo;

            await _context.SaveChangesAsync();
            return NoContent();

        }


        // DELETE: api/ToDos/5
        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteToDo(int id)
        {
            int userId = HttpContext.GetUserIdFromToken();
            if (userId == 0) return Unauthorized();

            if (await IsTodoAlreadyDone(id, userId))
            {
                return BadRequest("Nie można usunąć zakończonego zadania!");
            }
            var toDo = await _context.ToDos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (toDo == null)
            { return NotFound(); }

            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private async Task<bool> IsTodoAlreadyDone(int id, int userId)
        {
            var toDo = await _context.ToDos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (toDo == null) return false; 
            return toDo.Done;
        }


        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<ShowDTO>>> GetHistory()
        {
            int userId = HttpContext.GetUserIdFromToken();
            if (userId == 0) return Unauthorized();

            var today = DateOnly.FromDateTime(DateTime.Now);

            var todos = await _context.ToDos
                    .Where(i => i.UserId == userId && (i.Done == true || i.DateTo <= today))
                    .Select(d => new ShowDTO
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Done = d.Done,
                        DateTo = d.DateTo
                    })
                    .ToListAsync();
            return Ok(todos);
        }


    }


}
