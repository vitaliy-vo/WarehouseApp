namespace Warehouse.Core.OutPutModels
{
    public class WarehouseTransactionOutModel
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string WarehousName { get; set; }
        public string BlankName { get; set; }
        public string TransactionType { get; set; }
        public int Quantity { get; set; }
        public string Reference { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCancelled { get; set; }
    }
}
