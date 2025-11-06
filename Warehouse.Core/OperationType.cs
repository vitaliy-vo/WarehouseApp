namespace Warehouse.BLL
{
    public enum OperationType
    {
        Replenishment = 1,              // Поступление из центрального склада
        WriteOff = 2,                   // Списание в производство
        DefectiveWriteOff = 3,          // Списание бракованных заготовок
        SendToCentral = 4,              // Отправка в центральный склад
        DefectReplenishment = 5,        // Отмена-Поступление из центрального склада
        DefectWriteOff = 6,             // Отмена-Списание в производство
        DefectDefectiveWriteOff = 7,    // Отмена-Списание бракованных заготовок
        DefectSendToCentral = 8         // Отмена-Отправка в центральный склад
    }
}

