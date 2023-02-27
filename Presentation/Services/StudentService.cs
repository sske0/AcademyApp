using Core.Entities;
using Core.Extensions;
using Core.Helpers;
using Data.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Services
{
    public class StudentService
    {
        private readonly GroupService _groupService;
        private readonly GroupRepository _groupRepository;
        private readonly StudentRepository _studentRepository;
        public StudentService()
        {
            _groupService= new GroupService();
            _groupRepository = new GroupRepository();
            _studentRepository = new StudentRepository();
        }
        public void GetAll()
        {
            var students = _studentRepository.GetAll();

            ConsoleHelper.WriteWithColor("All Students:", ConsoleColor.Cyan);
            foreach(var student in students)
            {
                ConsoleHelper.WriteWithColor($"Id: {student.Id}, Fullname: {student.Name} {student.Surname}, Email: {student.Email}, Group: {student.Group?.Name} Created by: {student.CreatedBy} ", ConsoleColor.Magenta);
            }
        }
        public void GetllAllByGroup()
        {
            _groupService.GetAll();

            GroupIdInput: ConsoleHelper.WriteWithColor("Enter group's id: ", ConsoleColor.Cyan);
            int groupId;
            bool isSucceeded = int.TryParse(Console.ReadLine(), out groupId);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto GroupIdInput;
            }

            var group = _groupRepository.Get(groupId);
            if (group is null)
            {
                ConsoleHelper.WriteWithColor("There is no group with this id!", ConsoleColor.Yellow);
                return;
            }

            if (group.Students.Count == 0)
            {
                ConsoleHelper.WriteWithColor("No students in this group", ConsoleColor.Yellow);
                return;
            }
            else
            {
                foreach (var student in group.Students)
                {
                    ConsoleHelper.WriteWithColor($"Id: {student.Id}, Fullname: {student.Name} {student.Surname}, Email: {student.Email}", ConsoleColor.Magenta);
                }
            }
        }
        public void Create(Admin admin)
        {
            if (_groupRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("There are no groups. You have to create one before adding students", ConsoleColor.Yellow);
                return;
            }

            ConsoleHelper.WriteWithColor("Enter student's name: ", ConsoleColor.DarkCyan);
            string name = Console.ReadLine();

            ConsoleHelper.WriteWithColor("Enter student's surname: ", ConsoleColor.DarkCyan);
            string surname = Console.ReadLine();

            EmailInput: ConsoleHelper.WriteWithColor("Enter student's email: ", ConsoleColor.DarkCyan);
            string email = Console.ReadLine();

            if (!email.IsEmail())
            {
                ConsoleHelper.WriteWithColor("Invalid Email format", ConsoleColor.Red);
                goto EmailInput;
            }

            if (_studentRepository.IsDublicated(email))
            {
                ConsoleHelper.WriteWithColor("This email is currently used", ConsoleColor.Red);
                goto EmailInput;
            }

            BirthDateInput: ConsoleHelper.WriteWithColor("-- Enter birth date: ", ConsoleColor.DarkCyan);
            DateTime birthDate;
            bool isSucceeded = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Birth date's format is not correct!", ConsoleColor.Red);
                goto BirthDateInput;
            }

            GroupIdInput: _groupService.GetAll();

            ConsoleHelper.WriteWithColor("Enter group's id: ");

            int groupId;
            isSucceeded = int.TryParse(Console.ReadLine(), out groupId);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto GroupIdInput;
            }

            var group = _groupRepository.Get(groupId);
            if (group is null)
            {
                ConsoleHelper.WriteWithColor("Group with this id does not exist", ConsoleColor.Red);
                goto GroupIdInput;
            }

            var student = new Student
            {
                Name = name,
                Surname = surname,
                Email = email,
                Group = group,
                GroupId = group.Id,
                CreatedBy = admin.Username
            };

            if (group.MaxSize <= group.Students.Count())
            {
                ConsoleHelper.WriteWithColor("The group is full", ConsoleColor.Red);
                goto GroupIdInput;
            }

            group.Students.Add(student);
            _studentRepository.Add(student);
            ConsoleHelper.WriteWithColor($"{student.Name} {student.Surname} was successfully added", ConsoleColor.Green);
        }    
        public void Delete()
        {
            GetAll();

            StudentIdInput: ConsoleHelper.WriteWithColor("Enter id: ", ConsoleColor.Cyan);
            int id;
            bool isSucceeded = int.TryParse(Console.ReadLine(), out id); 
            if (!isSucceeded) 
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto StudentIdInput;
            }

            var student = _studentRepository.Get(id); 
            if (student is null) 
            {
                ConsoleHelper.WriteWithColor("No student with this id", ConsoleColor.Yellow);
            }
            
            _studentRepository.Delete(student);
            ConsoleHelper.WriteWithColor($"{student.Name}, {student.Surname} was deleted", ConsoleColor.Green);
        }
        public void Update(Admin admin) 
        {
            StudentIdInput: GetAll();

            ConsoleHelper.WriteWithColor("Enter id: ", ConsoleColor.Cyan);
            int id;
            bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto StudentIdInput;
            }

            var student = _studentRepository.Get(id);
            if (student is null)
            {
                ConsoleHelper.WriteWithColor("There is no student with this id", ConsoleColor.Red);
                goto StudentIdInput;
            }

            ConsoleHelper.WriteWithColor("Enter a new name: ", ConsoleColor.Cyan);
            string name = Console.ReadLine();

            ConsoleHelper.WriteWithColor("Enter a new surname: ", ConsoleColor.Cyan);
            string surname = Console.ReadLine();

            BirthDateInput: ConsoleHelper.WriteWithColor("Enter birth date: ", ConsoleColor.Cyan);
            DateTime birthDate;
            isSucceeded = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Birth date's format is not correct!", ConsoleColor.Red);
                goto BirthDateInput;
            }

            GroupIdInput: _groupService.GetAll();

            ConsoleHelper.WriteWithColor("Enter group id", ConsoleColor.Red);
            int groupId;
            isSucceeded = int.TryParse(Console.ReadLine(), out groupId);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto GroupIdInput;
            }

            var group = _groupRepository.Get(groupId);
            if (group is null)
            {
                ConsoleHelper.WriteWithColor("There is no group with this id", ConsoleColor.Red);
                goto GroupIdInput;
            }

            student.Name = name;
            student.Surname = surname;
            student.BithDate = birthDate;
            student.Group = group;
            student.GroupId= groupId;
            student.ModifiedBy = admin.Username;

            _studentRepository.Update(student);
            ConsoleHelper.WriteWithColor($"{student.Name} {student.Surname}, Group: {student.Group.Name} was successfully updated", ConsoleColor.Green);

        }
    }
}
