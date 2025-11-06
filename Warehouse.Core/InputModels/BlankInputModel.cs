using System.ComponentModel.DataAnnotations;

namespace Warehouse.Core.InputModels
{
    public class BlankInputModel
    {
        [Required(ErrorMessage = "Атирикул обязателен")]
        [StringLength(50, MinimumLength = 2)]
        public string Article { get; set; }
        [Required(ErrorMessage = "Имя ценности обязателено")]
        [StringLength(50, MinimumLength = 2)]
        public string NameValue { get; set; }
        [Required(ErrorMessage = "Значение поля больше или равно 0")]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
        public int IdWarehouse { get; set; }
        public string? PathPhoto { get; set; }
        public bool IsActive { get; set; }
    }
}
