namespace WpfEfCoreTest
{
    // https://metanit.com/sharp/wpf/22.6.php

    public interface IDialogService
    {
        string FilePath { get; set; } // путь к выбранному файлу
        void ShowMessage(string message); // показ сообщения
        bool OpenFileDialog(); // открытие файла
        bool SaveFileDialog(); // сохранение файла
    }
}