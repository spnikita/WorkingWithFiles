using System;

namespace Task4
{
    /// <summary>
    /// Студент
    /// </summary>
    internal class Student
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Группа
        /// </summary>
        public string Group { get; set; }
        
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        
        /// <summary>
        /// Средний балл
        /// </summary>
        public double AverageScore { get; set; }
    }
}
