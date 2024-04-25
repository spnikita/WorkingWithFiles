using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Task4
{
    class Program
    {
        /// <summary>
        /// Имя бинарного файла, содержащего базу данных студентов
        /// </summary>
        private const string BinaryFileName = "students.dat";

        /// <summary>
        /// Имя результирующей папки
        /// </summary>       
        private const string ResultFolderName = "Students";

        static void Main(string[] args)
        {
            try
            {
                var studentsToWrite = new List<Student>
                {
                    new Student { Name = "Жульен", Group = "G1", DateOfBirth = new DateTime(2001, 10, 22), AverageScore = 3.3},
                    new Student { Name = "Боб", Group = "G1", DateOfBirth = new DateTime(1999, 5, 25), AverageScore = 4.5},
                    new Student { Name = "Лилия", Group = "F2", DateOfBirth = new DateTime(1999, 1, 11), AverageScore = 5},
                    new Student { Name = "Роза", Group = "F2", DateOfBirth = new DateTime(1989, 9, 19), AverageScore = 3.7}
                };

                Console.WriteLine("Выполняется запись базы данных студентов в бинарный файл");

                WriteStudentsToBinFile(studentsToWrite, BinaryFileName);

                Console.WriteLine("Выполняется чтение базы данных студентов из бинарного файла");

                var studentsToRead = ReadStudentsFromBinFile(BinaryFileName);

                if (Directory.Exists(ResultFolderName))
                    Directory.Delete(ResultFolderName, true);

                Directory.CreateDirectory(ResultFolderName);

                Console.WriteLine("Выполняется запись базы данных студентов текстовые файлы с группировкой по группам");

                WriteStudentsToTextFiles(studentsToRead, ResultFolderName);
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
        /// Записать данные о студентах групп в файлы
        /// </summary>
        /// <param name="students">Список студентов</param>
        /// <param name="folderName">Результирующая папка с данными</param>
        private static void WriteStudentsToTextFiles(IReadOnlyCollection<Student> students, string folderName)
        {
            foreach (var studentsGroup in students.GroupBy(el => el.Group))
            {
                var filename = string.Concat(studentsGroup.Key, ".txt");

                var filePath = Path.Combine(folderName, filename);

                using var fs = new FileStream(filePath, FileMode.Create);
                using var sr = new StreamWriter(fs);
                foreach (var student in studentsGroup)
                {
                    var row = $"{student.Name}, {student.DateOfBirth:yyyyMMdd}, {student.AverageScore.ToString(CultureInfo.InvariantCulture)}";

                    sr.WriteLine(row);
                }
            }
        }

        /// <summary>
        /// Записать список студентов в бинарный файл
        /// </summary>
        /// <param name="students">Список студентов</param>
        /// <param name="fileName">Имя файла</param>
        private static void WriteStudentsToBinFile(IReadOnlyCollection<Student> students, string fileName)
        {
            using var fs = new FileStream(fileName, FileMode.Create);
            using var bw = new BinaryWriter(fs);

            foreach (Student student in students)
            {
                bw.Write(student.Name);
                bw.Write(student.Group);
                bw.Write(student.DateOfBirth.ToBinary());
                bw.Write(student.AverageScore);
            }           
        }

        /// <summary>
        /// Считать данные о студентах из бинарного файла
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static IReadOnlyCollection<Student> ReadStudentsFromBinFile(string fileName)
        {
            List<Student> result = new List<Student>();

            using var fs = new FileStream(fileName, FileMode.Open);          
            using var br = new BinaryReader(fs);

            while (fs.Position < fs.Length)
            {
                Student student = new Student();
                student.Name = br.ReadString();
                student.Group = br.ReadString();
                long dt = br.ReadInt64();
                student.DateOfBirth = DateTime.FromBinary(dt);
                student.AverageScore = br.ReadDouble();

                result.Add(student);
            }           
            return result;
        }
    }
}
