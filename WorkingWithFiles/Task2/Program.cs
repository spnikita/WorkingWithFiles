using System;
using System.IO;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Введите полное имя каталога, размер которого необходимо посчитать: ");

                var directoryPath = Console.ReadLine();

                if (!Directory.Exists(directoryPath))
                    throw new Exception("Указанная директория не существует.");

                Console.WriteLine("Выполняется подсчет размера каталога.");

                var di = new DirectoryInfo(directoryPath);

                var size = CalculateFolderSize(di);

                Console.WriteLine($"Размер каталога/папки: {size} байт.");
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
