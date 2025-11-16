using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Core.Dtos
{
    [Keyless]
    public class DetailedBalanceReportDto
    {
        public int BlankId { get; set; }
        public string BlankName { get; set; }
        public int OpeningBalance { get; set; }
        public int ReceiptFromCentral { get; set; }
        public string ReceiptReference { get; set; }
        public int ReturnDefective { get; set; } = 0; // Всегда 0
        public string ReturnDefectiveReference { get; set; } = "—"; // Всегда прочерк
        public int IssuedToProduction { get; set; }
        public string IssueReference { get; set; }
        public int TransferredToOtherWarehouse { get; set; }
        public string TransferReference { get; set; }
        public int WrittenOff { get; set; }
        public string WriteOffReference { get; set; }
        public int ClosingBalance { get; set; }
    }
}
