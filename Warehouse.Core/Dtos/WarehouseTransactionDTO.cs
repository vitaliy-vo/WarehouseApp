namespace Warehouse.Core.Dtos
{
    public class WarehouseTransactionDTO
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public int WarehouseId { get; set; }
        public int BlankId { get; set; }
        public int TransactionTypeId { get; set; }
        public int Quantity { get; set; }
        public string? Reference { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCancelled { get; set; }
    }
}
