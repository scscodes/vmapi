using Microsoft.EntityFrameworkCore;
using vmapi.Models;

namespace vmapi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Item> Item => Set<Item>();
        public DbSet<Machine> Machine => Set<Machine>();
        public DbSet<MachineInventory> MachineInventory => Set<MachineInventory>();
        public DbSet<MachineInventoryLineItem> MachineInventoryLineItem => Set<MachineInventoryLineItem>();
        public DbSet<Transaction> Transaction => Set<Transaction>();
        public DbSet<TransactionLineItem> TransactionLineItem => Set<TransactionLineItem>();
    }
}
