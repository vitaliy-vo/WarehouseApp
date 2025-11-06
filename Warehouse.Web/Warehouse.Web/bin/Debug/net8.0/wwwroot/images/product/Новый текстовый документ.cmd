@echo off
setlocal enabledelayedexpansion

:: Путь к исходной картинке
set SOURCE_IMAGE=F:\C#\WarehouseApp\Warehouse.Web\Warehouse.Web\wwwroot\images\product\product.jpg

:: Папка для сохранения
set TARGET_FOLDER=F:\C#\WarehouseApp\Warehouse.Web\Warehouse.Web\wwwroot\images\product

:: Проверяем существование папки
if not exist "%TARGET_FOLDER%" (
    mkdir "%TARGET_FOLDER%"
)

:: Счетчик для имен файлов
set COUNTER=0

:loop
:: Формируем имя файла
set FILE_NAME=%TARGET_FOLDER%\product!COUNTER!.jpg

:: Проверяем, существует ли файл
if exist "%FILE_NAME%" (
    set /a COUNTER+=1
    goto loop
)

:: Копируем файл
copy "%SOURCE_IMAGE%" "%FILE_NAME%"
echo Файл успешно скопирован как %FILE_NAME%

endlocal
