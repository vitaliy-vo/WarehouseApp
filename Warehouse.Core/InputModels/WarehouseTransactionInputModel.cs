using System.ComponentModel.DataAnnotations;

namespace Warehouse.Core.InputModels
{
    public class WarehouseTransactionInputModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Дата транзакции обязательна")]
        public DateTime TransactionDate { get; set; }
        [Required(ErrorMessage = "Склад обязателен")]
        public int WarehouseId { get; set; }
        [Required(ErrorMessage = "Заготовка обязательна")]
        public int BlankId { get; set; }
        [Required(ErrorMessage = "Тип транзакции обязателен")]
        public int TransactionTypeId { get; set; }
        [Required(ErrorMessage = "Количество обязательно")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0")]
        public int Quantity { get; set; }
        public string Reference { get; set; }
        public string CreatedBy { get; set; }
        [Required(ErrorMessage = "Поле 'Создал' обязательно")]
        public DateTime CreatedAt { get; set; }
        public bool IsCancelled { get; set; }
    }
}
