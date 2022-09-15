using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using vmapi.Data;
using vmapi.Models;

namespace vmapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private decimal TotalCost { get; set; }
        private Denomination _denomination;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransaction()
        {
          if (_context.Transaction == null)
          {
              return NotFound();
          }

          return await _context.Transaction
                .Include(t => t.TransactionLineItem)
                .ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(long id)
        {
          if (_context.Transaction == null)
          {
              return NotFound();
          }
            var transaction = await _context.Transaction.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return await _context.Transaction
                .Include(t => t.TransactionLineItem)
                .Where(t => t.Id == id)
                .SingleAsync();
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(long id, Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
          if (_context.Transaction == null || _context.Machine == null)
          {
              return Problem("Entity set 'AppDbContext.Transaction'  is null.");
          }

          switch (transaction.TransactionLineItem.ToList().Count)
          {
              case <= 0:
                  return BadRequest(); // purchase contains <= 0 items, invalid
              case > 0 when transaction.AmountTendered <= 0:
                  return BadRequest(); // purchase contains > 0 items but no amount tendered, invalid
          }

          // purchase items : machine quantity is valid
          // set updated quantity
          var isQuantityValid = await AdequateItems(transaction.TransactionLineItem.ToList());
          if (isQuantityValid == false) return BadRequest(); // inventory mismatch, invalid 
          
          // purchase items cost : total cost
          var isCostValid = await SumTotalCost(transaction.TransactionLineItem.ToList());
          if (isCostValid == false) return BadRequest(); // break while calculating costs, invalid
          if (TotalCost > transaction.AmountTendered) return BadRequest(); // insufficient payment, invalid
          
          // @ this point: machine quantity updated. TotalCost defined and is <= t.AmountTendered
          
          // dispense items in transaction, dispense change
          DispenseItem(transaction.TransactionLineItem.ToList());
          
          // dispense change for AmountTendered - TotalCost
          DispenseChange(transaction.AmountTendered - TotalCost);
          
          _context.Transaction.Add(transaction);
          await _context.SaveChangesAsync();

          return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(long id)
        {
            if (_context.Transaction == null)
            {
                return NotFound();
            }
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(long id)
        {
            return (_context.Transaction?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<bool> AdequateItems(IEnumerable<TransactionLineItem> lineItems)
        {
            // Evaluate list of transaction items. If conditions are met, set updated quantity and return true
            try
            {
                foreach (var li in lineItems)
                {
                    var mId = li.MachineId;
                    var iId = li.ItemId;
                    var iQuantity = li.Quantity;
                
                    // get current machine inventory
                    var ctxMachine = await _context.Machine
                        .Include(m => m.MachineInventory.Items)
                        .Where(m => m.Id == mId)
                        .SingleAsync();
                    var ctxMachineInventory = ctxMachine.MachineInventory.Items.ToList();
                    if (ctxMachineInventory.Count <= 0) return false; // no m.inventory with stocked items, invalid
                    if (ctxMachineInventory.All(item => item.ItemId != iId)) return false; // no m.inventory items contain item id, invalid
                
                    foreach (var invItem in ctxMachineInventory)
                    {
                        if (invItem.ItemId == iId && invItem.Quantity >= iQuantity)
                        {
                            // @ this point: inventory valid; inventory.Item.id == li.Id; inventory.Item.Quantity >= li.Quantity
                    
                            // set new (reduced) quantity to inventory item
                            invItem.Quantity -= iQuantity;
                            _context.MachineInventoryLineItem.Update(invItem);
                            // stage context to update new invItem.Quantity value
                            _context.Entry(invItem).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    } // end inventory line items
                } // end transaction line items
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> SumTotalCost(IEnumerable<TransactionLineItem> lineItems)
        {
            // Sum the cost of all items in the transaction; TotalCost = (...liCost = Item.Price * li.Quantity)
            try
            {
                var transactionTotalCost = 0m;
                foreach (var li in lineItems)
                {
                    var iId = li.ItemId;
                    // var ctxItem = await _context.Item
                    //     .Where(i => i.Id == iId)
                    //     .SingleAsync(); // Item.Id == li.Id
                    var ctxItem = await _context.Item.FindAsync(iId);
                    
                    if (ctxItem != null)
                    {
                        transactionTotalCost += ctxItem.Price * li.Quantity;
                    }
                }

                TotalCost = transactionTotalCost;
                return true;
            }
            catch
            {
                TotalCost = 0m;
                return false;
            }
        }

        private Denomination CalculateChange(decimal changeAmount)
        {
            return new Denomination(changeAmount);
        }
        private void DispenseItem(IEnumerable<TransactionLineItem> lineItems)
        {
            // Additional logic interacting w/ physical machine: dispense item(s)
            Console.WriteLine("## Dispensing the following items:");
            Console.WriteLine(lineItems.ToJson());
        }

        private void DispenseChange(decimal changeAmount)
        {
            // Additional logic interacting w/ physical machine: dispense item(s)
            Console.WriteLine($"## Total Change: {changeAmount}");
            var change = new Denomination(changeAmount).ToJson();
            Console.WriteLine(change);
        }
    }
}
