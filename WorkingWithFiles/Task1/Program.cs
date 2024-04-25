using System;
using System.IO;

namespace Task1
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

                Console.WriteLine("Выполняется очистка каталога от файлов и папок, которые не использовались более 30 минут.");

                var rootDirectory = new DirectoryInfo(directoryPath);

                DeleteFiles(rootDirectory);

                DeleteFolders(rootDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
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
    }
}
