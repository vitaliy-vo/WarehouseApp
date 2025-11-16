using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Core.Dtos
{
    public class MonthlyBalanceDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } 
        public int WarehouseId { get; set; }
        public int BlankId { get; set; }
        public int OpeningBalance { get; set; } 
        public int TotalIssues { get; set; } 
        public int TotalReceipts { get; set; } 
        public int DefectiveCount { get; set; } 
        public int ClosingBalance { get; set; } 
    }
}
