namespace Warehouse.Core.InputModels
{
    public class ReportViewModel
    {
        public string BlankName { get; set; }
        public int OpeningBalance { get; set; }
        public int TotalReceipts { get; set; }
        public int TotalIssues { get; set; }
        public int DefectiveCount { get; set; }
        public int ClosingBalance { get; set; }
    }
}
