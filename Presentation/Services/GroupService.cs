using Core.Entities;
using Core.Helpers;
using Data.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Presentation.Services
{
    class GroupService
    {
        private readonly GroupRepository _groupRepository;
        private readonly StudentRepository _studentRepository;
        private readonly TeacherRepository _teacherRepository;
        public GroupService()
        {
            _groupRepository = new GroupRepository();
            _studentRepository = new StudentRepository();
            _teacherRepository = new TeacherRepository();
        }
        public void GetAll()
        {
            var groups = _groupRepository.GetAll();
            foreach (var group in groups)
            {
                ConsoleHelper.WriteWithColor($"Id: {group.Id} Name: {group.Name} Max size: {group.MaxSize}, Start date: {group.StartDate} End date: {group.EndDate} Created by: {group.CreatedBy}", ConsoleColor.Magenta);
            }
        }
        public void GetAllGroupsByTeacher()
        {
            var teachers = _teacherRepository.GetAll();
            foreach (var teacher in teachers)
            {
                ConsoleHelper.WriteWithColor($"Id: {teacher.Id}, Fullname: {teacher.Name} {teacher.Surname}", ConsoleColor.Magenta);
            }

            TeacherIdInput: ConsoleHelper.WriteWithColor("Enter teacher's id: ", ConsoleColor.Cyan);

            int id;
            bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto TeacherIdInput;
            }

            var dbTeacher = _teacherRepository.Get(id);
            if (dbTeacher is null)
            {
                ConsoleHelper.WriteWithColor("There is no teacher with this id", ConsoleColor.Yellow);
            }

            else
            {
                foreach (var group in dbTeacher.Groups)
                {
                    ConsoleHelper.WriteWithColor($"Id: {group.Id}, Name: {group.Name}", ConsoleColor.Magenta);
                }
            }
        }
        public void GetGroupById(Admin admin)
        {

            var groups = _groupRepository.GetAll();

            if (groups.Count == 0)
            {
                Decision: ConsoleHelper.WriteWithColor("There are no groups, wanna create one? --- y n?", ConsoleColor.DarkRed);
                char decision;
                bool isSucceededResult = char.TryParse(Console.ReadLine(), out decision);
                if (!isSucceededResult)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto Decision;
                }
                if (!(decision == 'y' || decision == 'n'))
                {
                    ConsoleHelper.WriteWithColor("Choose between y and n!", ConsoleColor.Red);
                    goto Decision;
                }
                if (decision == 'y')
                {
                    Create(admin);
                }
            }
            else
            {
                GetAll();
                IdEntering: ConsoleHelper.WriteWithColor("--- Enter Id: ---", ConsoleColor.Cyan);
                int id;
                bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid Id format!", ConsoleColor.Red);
                    goto IdEntering;
                }

                var group = _groupRepository.Get(id);
                if (group is null)
                {
                    ConsoleHelper.WriteWithColor("No group with this Id", ConsoleColor.Red);
                    goto IdEntering;
                }
                ConsoleHelper.WriteWithColor($"Id: {group.Id} Name: {group.Name} Max size: {group.MaxSize}, Start date: {group.StartDate} End date: {group.EndDate} Created by: {group.CreatedBy}", ConsoleColor.Magenta);
            }


        }
        public void GetGroupByName()
        {
            GetAll();

        NameInput: ConsoleHelper.WriteWithColor("Enter group name", ConsoleColor.Cyan);
            string name = Console.ReadLine();

            var group = _groupRepository.GetByName(name);
            if (group is null)
            {
                ConsoleHelper.WriteWithColor("There is no any group with this name", ConsoleColor.Red);
                goto NameInput;
            }

            ConsoleHelper.WriteWithColor($"Id: {group.Id} Name: {group.Name} Max size: {group.MaxSize}, Start date: {group.StartDate} End date: {group.EndDate}", ConsoleColor.Magenta);

        }
        public void Create(Admin admin)
        {
            if (_teacherRepository.GetAll().Count == 0)
            {
                ConsoleHelper.WriteWithColor("A teacher must be created beforehand", ConsoleColor.Yellow);
            }
            else
            {
                GroupNameInput: ConsoleHelper.WriteWithColor("-- Enter name: ", ConsoleColor.DarkCyan);
                string name = Console.ReadLine();
                var group = _groupRepository.GetByName(name);
                if (group is not null)
                {
                    ConsoleHelper.WriteWithColor("Group with this name was added", ConsoleColor.Yellow);
                    goto GroupNameInput;
                }

                MaxSizeInput: ConsoleHelper.WriteWithColor("-- Enter max size of the group: ", ConsoleColor.DarkCyan);
                int maxSize;
                bool isSucceeded = int.TryParse(Console.ReadLine(), out maxSize);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Inputed size format is not valid", ConsoleColor.Red);
                    goto MaxSizeInput;
                }

                if (maxSize > 20)
                {
                    ConsoleHelper.WriteWithColor("Max size of group is 20", ConsoleColor.Red);
                    goto MaxSizeInput;
                }

                StartDateInput: ConsoleHelper.WriteWithColor("-- Enter start date: ", ConsoleColor.DarkCyan);
                DateTime startDate;
                isSucceeded = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Start date's format is not correct!", ConsoleColor.Red);
                    goto StartDateInput;
                }

                DateTime boundaryDate = new DateTime(2015, 1, 1);

                if (startDate < boundaryDate)
                {
                    ConsoleHelper.WriteWithColor("Start date is not chosen right", ConsoleColor.Red);
                    goto StartDateInput;
                }

                EndDateInput: ConsoleHelper.WriteWithColor("-- Enter end date: ", ConsoleColor.DarkCyan);
                DateTime endDate;
                isSucceeded = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Start date's format is not correct!", ConsoleColor.Red);
                    goto EndDateInput;
                }

                if (startDate > endDate)
                {
                    ConsoleHelper.WriteWithColor("End date cant be earlier than start date!", ConsoleColor.Red);
                    goto EndDateInput;
                }

                var teachers = _teacherRepository.GetAll();
                foreach (var teacher in teachers)
                {
                    ConsoleHelper.WriteWithColor($"Id: {teacher.Id}, Fullname: {teacher.Name} {teacher.Surname}", ConsoleColor.Magenta);
                }

                TeacherIdInput: ConsoleHelper.WriteWithColor($"Enter teacher's id: ", ConsoleColor.Cyan);
                int teacherId;
                isSucceeded = int.TryParse(Console.ReadLine(), out teacherId);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                    goto TeacherIdInput;
                }

                var dbTeacher = _teacherRepository.Get(teacherId);
                if (dbTeacher is null)
                {
                    ConsoleHelper.WriteWithColor("No teacher with this id!", ConsoleColor.Red);
                    goto TeacherIdInput;
                }

                group = new Group
                {
                    Name = name,
                    MaxSize = maxSize,
                    StartDate = startDate,
                    EndDate = endDate,
                    CreatedBy = admin.Username,
                    Teacher = dbTeacher
                };

                dbTeacher.Groups.Add(group);
                _groupRepository.Add(group);
                ConsoleHelper.WriteWithColor($"Group was successfully created with Name: {group.Name}\n Max size: {group.MaxSize}\n Start date: {group.StartDate.ToLongDateString()}\n End date: {group.EndDate.ToLongDateString()}", ConsoleColor.Magenta);
            }
        }
        public void Update(Admin admin)
        {
            GetAll();

        EnterGroup: ConsoleHelper.WriteWithColor("enter group \n 1. id \n 2. name: ", ConsoleColor.DarkCyan);

            int number;
            bool isSucceeded = int.TryParse(Console.ReadLine(), out number);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto EnterGroup;
            }

            if (!(number == 1 || number == 2))
            {
                ConsoleHelper.WriteWithColor("incorrect inputed number", ConsoleColor.Red);
                goto EnterGroup;
            }
            if (number == 1)
            {
            EnterGroupId: ConsoleHelper.WriteWithColor("enter group Id: ", ConsoleColor.DarkCyan);
                int id;
                isSucceeded = int.TryParse(Console.ReadLine(), out id);
                if (!isSucceeded)
                {
                    ConsoleHelper.WriteWithColor("Invalid format", ConsoleColor.Red);
                    goto EnterGroupId;
                }

                var group = _groupRepository.Get(id);
                if (group is null)
                {
                    ConsoleHelper.WriteWithColor("No such a group with this Id", ConsoleColor.Red);
                }
                InternalUpdate(group, admin);
            }
            else
            {
                EnterGroupName: ConsoleHelper.WriteWithColor("enter group name: ", ConsoleColor.DarkCyan);
                string name = Console.ReadLine();
                var group = _groupRepository.GetByName(name);
                if (group is null)
                {
                    ConsoleHelper.WriteWithColor("No such a group with this name", ConsoleColor.Red);
                    goto EnterGroupName;
                }
                InternalUpdate(group, admin);
            }
        }

        private void InternalUpdate(Group group, Admin admin)
        {
            ConsoleHelper.WriteWithColor("Enter new name:");
            string name = Console.ReadLine();

        MaxSize: ConsoleHelper.WriteWithColor("Enter new max size:");

            int maxSize;
            bool isSucceeded = int.TryParse(Console.ReadLine(), out maxSize);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Invalid format!", ConsoleColor.Red);
                goto MaxSize;
            }

            ConsoleHelper.WriteWithColor("Enter new start date:", ConsoleColor.Cyan);

            StartDateInput: ConsoleHelper.WriteWithColor("-- Enter new start date: ", ConsoleColor.DarkCyan);
            DateTime startDate;
            isSucceeded = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Start date's format is not correct!", ConsoleColor.Red);
                goto StartDateInput;
            }

            EndDateInput: ConsoleHelper.WriteWithColor("-- Enter new end date: ", ConsoleColor.DarkCyan);
            DateTime endDate;
            isSucceeded = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Start date's format is not correct!", ConsoleColor.Red);
                goto EndDateInput;
            }
            group.Name = name;
            group.MaxSize = maxSize;
            group.StartDate = startDate;
            group.EndDate = endDate;
            group.ModifiedBy = admin.Username;
            _groupRepository.Update(group);
        }
        public void Delete()
        {
            GetAll();
        IdInput: ConsoleHelper.WriteWithColor("-- Enter Id: ", ConsoleColor.DarkCyan);

            int id;
            bool isSucceeded = int.TryParse(Console.ReadLine(), out id);
            if (!isSucceeded)
            {
                ConsoleHelper.WriteWithColor("Id's format is not correct!", ConsoleColor.Red);
                goto IdInput;
            }

            var dbGroup = _groupRepository.Get(id);
            if (dbGroup is null)
                ConsoleHelper.WriteWithColor("There is no such a group with written id", ConsoleColor.Red);
            else
            {
                foreach (var student in dbGroup.Students)
                {
                    student.Group = null;
                    _studentRepository.Update(student);
                }
                _groupRepository.Delete(dbGroup);
                ConsoleHelper.WriteWithColor("The group was successfully deleted", ConsoleColor.Green);
            }
        }
    }
}
