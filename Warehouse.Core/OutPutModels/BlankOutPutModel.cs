namespace Warehouse.Core.OutPutModels
{
    public class BlankOutPutModel
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public string NameValue { get; set; }
        public int Count { get; set; }
        public int IdWarehouse { get; set; }
        public string? PathPhoto { get; set; }
        public bool? IsActive { get; set; }
    }
}
