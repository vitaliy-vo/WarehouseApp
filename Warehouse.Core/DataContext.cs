using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Dtos;

namespace Warehouse.Core
{
    public class DataContext : DbContext
    {
        public DbSet<BlankDto> Blanks { get; set; }

        public DbSet<WarehouseDTO> Warehouses { get; set; }

        public DbSet<UserDTO> Users { get; set; }

        public DbSet<WarehouseTransactionDTO> Warehousetransaction { get; set; }

        public DbSet<DailyBalanceDto> DailyBalanceReport { get; set; }

        public DbSet<TransactionTypeDto> TransactionType { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
