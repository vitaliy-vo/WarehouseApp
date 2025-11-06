namespace Warehouse.Core.Dtos
{
    public class WarehouseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<BlankDto> listBlank = new List<BlankDto>();

    }
}
