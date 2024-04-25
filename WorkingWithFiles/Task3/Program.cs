using System;
using System.IO;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Введите полное имя каталога, который необходимо очистить: ");

                var directoryPath = Console.ReadLine();

                if (!Directory.Exists(directoryPath))
                    throw new Exception("Указанная директория не существует.");

                var di = new DirectoryInfo(directoryPath);

                var sizeBeforeDeletion = CalculateFolderSize(di);

                Console.WriteLine($"Исходный размер каталога/папки: {sizeBeforeDeletion} байт.");

                Console.WriteLine("Выполняется очистка каталога от файлов и папок, которые не использовались более 30 минут.");

                DeleteFiles(di);

                DeleteFolders(di);

                var sizeAfterDeletion = CalculateFolderSize(di);

                Console.WriteLine($"Освобождено: {sizeBeforeDeletion - sizeAfterDeletion} байт.");

                Console.WriteLine($"Текущий размер каталога/папки: {sizeAfterDeletion} байт.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("Работа программы завершена");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Удалить файлы в каталоге
        /// </summary>
        /// <param name="di">Каталог</param>
        private static void DeleteFiles(DirectoryInfo di)
        {
            foreach (var fi in di.GetFiles())
            {
                try
                {
                    if (DateTime.Now - fi.LastAccessTime > TimeSpan.FromMinutes(30))
                    {
                        fi.Delete();
                    }
                    else
                    {
                        Console.WriteLine($"Файл {fi.FullName} использовался менее, чем 30 минут назад. Последнее время использования файла: {fi.LastAccessTime}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка удаления файла {fi.FullName}: {ex}");
                }
            }
        }

        /// <summary>
        /// Удалить папки в каталоге
        /// </summary>
        /// <param name="di">Каталог</param>
        private static void DeleteFolders(DirectoryInfo di)
        {
            foreach (DirectoryInfo childDirectory in di.GetDirectories())
            {
                try
                {
                    if (DateTime.Now - childDirectory.LastAccessTime > TimeSpan.FromMinutes(30))
                    {
                        childDirectory.Delete(true);
                    }
                    else
                    {
                        Console.WriteLine($"Каталог {childDirectory.FullName} использовался менее, чем 30 минут назад. Последнее время использования файла: {childDirectory.LastAccessTime}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка удаления каталога {childDirectory.FullName}: {ex}");
                }
            }
        }

        /// <summary>
        /// Посчитать размер каталога
        /// </summary>
        /// <param name="di">Каталог</param>
        /// <returns>Размер каталога, байт</returns>
        private static long CalculateFolderSize(DirectoryInfo di)
        {
            long size = 0;

            FileInfo[] fis;

            try
            {
                fis = di.GetFiles();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка доступа к директории {di.FullName}: {ex.Message}");

                return 0;
            }

            foreach (var fi in fis)
            {
                try
                {
                    size += fi.Length;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка доступа к файлу {fi.FullName}: {ex.Message}");
                }
            }

            foreach (var childDirectory in di.GetDirectories())
            {
                size += CalculateFolderSize(childDirectory);
            }

            return size;
        }
    }    
}
