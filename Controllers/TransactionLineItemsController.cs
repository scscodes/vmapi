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
    public class TransactionLineItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionLineItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TransactionLineItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionLineItem>>> GetTransactionLineItem()
        {
          if (_context.TransactionLineItem == null)
          {
              return NotFound();
          }
          
          return await _context.TransactionLineItem.ToListAsync();
        }

        // GET: api/TransactionLineItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionLineItem>> GetTransactionLineItem(long id)
        {
          if (_context.TransactionLineItem == null)
          {
              return NotFound();
          }
          
          var transactionLineItem = await _context.TransactionLineItem.FindAsync(id);

            if (transactionLineItem == null)
            {
                return NotFound();
            }

            return transactionLineItem;
        }

        // PUT: api/TransactionLineItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionLineItem(long id, TransactionLineItem transactionLineItem)
        {
            if (id != transactionLineItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(transactionLineItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionLineItemExists(id))
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

        // POST: api/TransactionLineItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransactionLineItem>> PostTransactionLineItem(TransactionLineItem transactionLineItem)
        {
          if (_context.TransactionLineItem == null)
          {
              return Problem("Entity set 'AppDbContext.TransactionLineItem'  is null.");
          }
            _context.TransactionLineItem.Add(transactionLineItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactionLineItem", new { id = transactionLineItem.Id }, transactionLineItem);
        }

        // DELETE: api/TransactionLineItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionLineItem(long id)
        {
            if (_context.TransactionLineItem == null)
            {
                return NotFound();
            }
            var transactionLineItem = await _context.TransactionLineItem.FindAsync(id);
            if (transactionLineItem == null)
            {
                return NotFound();
            }

            _context.TransactionLineItem.Remove(transactionLineItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionLineItemExists(long id)
        {
            return (_context.TransactionLineItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
