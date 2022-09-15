using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vmapi.Data;
using vmapi.Models;

namespace vmapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MachinesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Machines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachine()
        {
          if (_context.Machine == null)
          {
              return NotFound();
          }

          return await _context.Machine
                .Include(m => m.MachineInventory.Items)
                .ToListAsync();
        }

        // GET: api/Machines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Machine>> GetMachine(long id)
        {
          if (_context.Machine == null)
          {
              return NotFound();
          }
            var machine = await _context.Machine.FindAsync(id);

            if (machine == null)
            {
                return NotFound();
            }

            return await _context.Machine
                .Include(m => m.MachineInventory.Items)
                .Where(m => m.Id == id)
                .SingleAsync();
        }

        // PUT: api/Machines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMachine(long id, Machine machine)
        {
            if (id != machine.Id)
            {
                return BadRequest();
            }

            _context.Entry(machine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MachineExists(id))
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

        // POST: api/Machines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Machine>> PostMachine(Machine machine)
        {
          if (_context.Machine == null)
          {
              return Problem("Entity set 'AppDbContext.Machine'  is null.");
          }
            _context.Machine.Add(machine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMachine", new { id = machine.Id }, machine);
        }

        // DELETE: api/Machines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(long id)
        {
            if (_context.Machine == null)
            {
                return NotFound();
            }
            var machine = await _context.Machine.FindAsync(id);
            if (machine == null)
            {
                return NotFound();
            }

            _context.Machine.Remove(machine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MachineExists(long id)
        {
            return (_context.Machine?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
