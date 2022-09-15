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
    public class MachineInventoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MachineInventoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MachineInventories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MachineInventory>>> GetMachineInventory()
        {
          if (_context.MachineInventory == null)
          {
              return NotFound();
          }

          return await _context.MachineInventory
              .Include(m => m.Items)
              .ToListAsync();
        }

        // GET: api/MachineInventories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MachineInventory>> GetMachineInventory(long id)
        {
          if (_context.MachineInventory == null)
          {
              return NotFound();
          }
          
          var machineInventory = await _context.MachineInventory
              .Include(m => m.Items)
              .Where(m => m.Id == id)
              .SingleAsync();

            if (machineInventory == null)
            {
                return NotFound();
            }

            return machineInventory;
        }

        // PUT: api/MachineInventories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMachineInventory(long id, MachineInventory machineInventory)
        {
            if (id != machineInventory.Id)
            {
                return BadRequest();
            }

            _context.Entry(machineInventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MachineInventoryExists(id))
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

        // POST: api/MachineInventories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MachineInventory>> PostMachineInventory(MachineInventory machineInventory)
        {
          if (_context.MachineInventory == null)
          {
              return Problem("Entity set 'AppDbContext.MachineInventory'  is null.");
          }
            _context.MachineInventory.Add(machineInventory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMachineInventory", new { id = machineInventory.Id }, machineInventory);
        }

        // DELETE: api/MachineInventories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachineInventory(long id)
        {
            if (_context.MachineInventory == null)
            {
                return NotFound();
            }
            var machineInventory = await _context.MachineInventory.FindAsync(id);
            if (machineInventory == null)
            {
                return NotFound();
            }

            _context.MachineInventory.Remove(machineInventory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MachineInventoryExists(long id)
        {
            return (_context.MachineInventory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
