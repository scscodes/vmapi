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
    public class MachineInventoryLineItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MachineInventoryLineItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MachineInventoryLineItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MachineInventoryLineItem>>> GetMachineInventoryLineItem()
        {
          if (_context.MachineInventoryLineItem == null)
          {
              return NotFound();
          }
            return await _context.MachineInventoryLineItem.ToListAsync();
        }

        // GET: api/MachineInventoryLineItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MachineInventoryLineItem>> GetMachineInventoryLineItem(long id)
        {
          if (_context.MachineInventoryLineItem == null)
          {
              return NotFound();
          }
            var machineInventoryLineItem = await _context.MachineInventoryLineItem.FindAsync(id);

            if (machineInventoryLineItem == null)
            {
                return NotFound();
            }

            return machineInventoryLineItem;
        }

        // PUT: api/MachineInventoryLineItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMachineInventoryLineItem(long id, MachineInventoryLineItem machineInventoryLineItem)
        {
            if (id != machineInventoryLineItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(machineInventoryLineItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MachineInventoryLineItemExists(id))
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

        // POST: api/MachineInventoryLineItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MachineInventoryLineItem>> PostMachineInventoryLineItem(MachineInventoryLineItem machineInventoryLineItem)
        {
          if (_context.MachineInventoryLineItem == null)
          {
              return Problem("Entity set 'AppDbContext.MachineInventoryLineItem'  is null.");
          }
            _context.MachineInventoryLineItem.Add(machineInventoryLineItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMachineInventoryLineItem", new { id = machineInventoryLineItem.Id }, machineInventoryLineItem);
        }

        // DELETE: api/MachineInventoryLineItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachineInventoryLineItem(long id)
        {
            if (_context.MachineInventoryLineItem == null)
            {
                return NotFound();
            }
            var machineInventoryLineItem = await _context.MachineInventoryLineItem.FindAsync(id);
            if (machineInventoryLineItem == null)
            {
                return NotFound();
            }

            _context.MachineInventoryLineItem.Remove(machineInventoryLineItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MachineInventoryLineItemExists(long id)
        {
            return (_context.MachineInventoryLineItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
