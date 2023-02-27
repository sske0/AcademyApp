using Core.Entities;
using Core.Helpers;
using Data.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Services
{
    public class TeacherService
    {
        private readonly TeacherRepository _teacherRepository;


        public TeacherService()
        {
            _teacherRepository = new TeacherRepository();
        }
        public void GetAll()
        {
            var teachers = _teacherRepository.GetAll();
            if (teachers.Count == 0) 
            {
                ConsoleHelper.WriteWithColor("No teachers yet", ConsoleColor.Yellow);
            }
            foreach (var teacher in teachers) 
            {
                if (teacher.Groups.Count == 0)
                {
                    ConsoleHelper.WriteWithColor("Teacher has no groups yet", ConsoleColor.Yellow);
                }
                ConsoleHelper.WriteWithColor($"Id: {teacher.Id} Fullname: {teacher.Name} {teacher.Surname} Speciality: {teacher.Speciality}", ConsoleColor.Magenta);
                foreach (var group in teacher.Groups)
                {
                    ConsoleHelper.WriteWithColor($"Id: {teacher.Id} Name: {teacher.Name}", ConsoleColor.Magenta);
                }

                Console.WriteLine();
            }

        }
        public void Create()
        {
            ConsoleHelper.WriteWithColor("Enter teacher's name", ConsoleColor.Cyan);
            string name = Console.ReadLine();

            ConsoleHelper.WriteWithColor("Enter teacher's surname", ConsoleColor.Cyan);
            string surname = Console.ReadLine();

            BirthDateInput: ConsoleHelper.WriteWithColor("Enter teacher's birth date: ", ConsoleColor.DarkCyan);
            DateTime birthDate;
            bool isSucceeded = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Birth date's format is not correct!", ConsoleColor.Red);
                goto BirthDateInput;
            }

            ConsoleHelper.WriteWithColor("Enter teacher's speciality", ConsoleColor.Cyan);
            string speciality = Console.ReadLine();

            var teacher = new Teacher
            {
                Name = name,
                Surname = surname,
                BirthDate = birthDate,
                Speciality = speciality,
                CreatedAt = DateTime.Now 
            };

            _teacherRepository.Add(teacher);
            string teacherBirthDate = teacher.BirthDate.ToString("dddd, dd MMMM yyyy");
            ConsoleHelper.WriteWithColor($"Name: {teacher.Name} Surname: {teacher.Surname} Speciality: {teacher.Speciality} Birth date: {teacher.BirthDate}", ConsoleColor.Magenta);  
        }
        public void Delete()
        {
            if (_teacherRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("No teachers yet", ConsoleColor.Yellow);
            }
            else
            {
                TeacherIdInput: GetAll();
                ConsoleHelper.WriteWithColor("Enter teacher's id: ", ConsoleColor.Cyan);
                int id;
                bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto TeacherIdInput;
                }

                var teacher = _teacherRepository.Get(id);
                if (teacher is null)
                {
                    ConsoleHelper.WriteWithColor("There is no teacher with this id", ConsoleColor.Yellow);
                }

                _teacherRepository.Delete(teacher);
                ConsoleHelper.WriteWithColor($"{teacher.Name} {teacher.Surname} was deleted", ConsoleColor.Green);

            }
        }
        public void Update()
        {
            GetAll();
            TeacherIdInput: ConsoleHelper.WriteWithColor("Enter teacher's id: ", ConsoleColor.Cyan);
            int id;
            bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto TeacherIdInput;
            }

            var teacher = _teacherRepository.Get(id);
            if (teacher is null)
            {
                ConsoleHelper.WriteWithColor("There is no teacher with this id", ConsoleColor.Yellow);
                goto TeacherIdInput;
            }
            ConsoleHelper.WriteWithColor("Enter name: ", ConsoleColor.Cyan);
            string name = Console.ReadLine();

            ConsoleHelper.WriteWithColor("Enter surname: ", ConsoleColor.Cyan);
            string surname = Console.ReadLine();

            BirthDateInput: ConsoleHelper.WriteWithColor("Enter birth date: ", ConsoleColor.DarkCyan);
            DateTime birthDate;
            isSucceeded = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Birth date's format is not correct!", ConsoleColor.Red);
                goto BirthDateInput;
            }

            ConsoleHelper.WriteWithColor("Enter speciality: ", ConsoleColor.Cyan);
            string speciality = Console.ReadLine();

            teacher.Name = name;
            teacher.Surname = surname;
            teacher.BirthDate = birthDate;
            teacher.Speciality = speciality;

            _teacherRepository.Update(teacher);

            ConsoleHelper.WriteWithColor($"{teacher.Name} {teacher.Surname} was updated", ConsoleColor.Green);

        }
    }
}
