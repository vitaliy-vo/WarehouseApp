namespace Warehouse.Core.InputModels
{
    public class WriteOffModelInputModel
    {
        public List<WriteOffItemInputModel> Items { get; set; } = new();
        public int WarehouseId { get; set; }
        public int TransactionTypeId { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
