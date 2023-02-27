using Core.Constants;
using Core.Entities;
using Core.Extensions;
using Core.Helpers;
using Data;
using Data.Repositories.Concrete;
using Presentation.Services;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Presentation
{
    public static class Program
    {
        private readonly static GroupService _groupService;
        private readonly static StudentService _studentService;
        private readonly static TeacherService _teacherService;
        private readonly static AdminService _adminService;



        static Program()
        {
            Console.OutputEncoding = Encoding.UTF8;
            DbInitializer.SeedAdmins();

            _groupService = new GroupService();
            _studentService = new StudentService();
            _teacherService= new TeacherService();
            _adminService = new AdminService();
        }
        static void Main()
        {
            ConsoleHelper.WriteWithColor("Welcome!", ConsoleColor.Cyan);

            Authorize: var admin = _adminService.Authorize();
            if (admin is not null)
            {
                ConsoleHelper.WriteWithColor($"Welcome,{admin.Username}!", ConsoleColor.Cyan);

                while (true)
                {
                    MainMenu: ConsoleHelper.WriteWithColor("1 - Groups", ConsoleColor.DarkCyan);
                    ConsoleHelper.WriteWithColor("2 - Students", ConsoleColor.DarkCyan);
                    ConsoleHelper.WriteWithColor("3 - Teachers", ConsoleColor.DarkCyan);
                    ConsoleHelper.WriteWithColor("0 - Logout", ConsoleColor.DarkCyan);

                    int number;
                    bool isSucceeded = int.TryParse(Console.ReadLine(), out number);
                    if (!isSucceeded)
                    {
                        ConsoleHelper.WriteWithColor("invalid format!", ConsoleColor.Red);
                        goto MainMenu;
                    }
                    else
                    {
                        switch (number)
                        {
                            case (int)MainMenuOptions.Groups:
                                while (true)
                                {
                                GroupsMenu: ConsoleHelper.WriteWithColor("1 - Group Creation", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("2 - Update Group", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("3 - Delete Group", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("4 - Get All Groups", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("5 - Get Group By Id", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("6 - Get Group By Name", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("7 - Get All Groups By Teacher", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("0 - Back to Main Menu", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("--- Select option ---", ConsoleColor.Cyan);
                                    isSucceeded = int.TryParse(Console.ReadLine(), out number);
                                    if (!isSucceeded)
                                    {
                                        ConsoleHelper.WriteWithColor("Inputed number's format is not valid", ConsoleColor.Red);
                                    }
                                    else
                                    {


                                        switch (number)
                                        {
                                            case (int)GroupOptions.GroupCreation:
                                                _groupService.Create(admin);
                                                break;
                                            case (int)GroupOptions.UpdateGroup:
                                                _groupService.Update(admin);
                                                break;
                                            case (int)GroupOptions.DeleteGroup:
                                                _groupService.Delete();
                                                break;
                                            case (int)GroupOptions.GetAllGroups:
                                                _groupService.GetAll();
                                                break;
                                            case (int)GroupOptions.GetGroupById:
                                                _groupService.GetGroupById(admin);
                                                break;
                                            case (int)GroupOptions.GetGroupByName:
                                                _groupService.GetGroupByName();
                                                break;
                                            case (int)GroupOptions.GetAllGroupsByTeacher:
                                                _groupService.GetAllGroupsByTeacher();
                                                break;
                                            case (int)GroupOptions.BackToMainMenu:
                                                goto MainMenu;
                                                break;

                                            default:
                                                ConsoleHelper.WriteWithColor("Choose a number from 0 to 7!", ConsoleColor.Red);
                                                goto GroupsMenu;
                                                break;
                                        }
                                    }
                                }
                            case (int)MainMenuOptions.Students:
                                while (true)
                                {
                                    StudentsMenu: ConsoleHelper.WriteWithColor("1 - Student Creation", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("2 - Update Student", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("3 - Delete Student", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("4 - Get All Students", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("5 - Get All Students By Group", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("0 - Back to Main Menu", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("--- Select option ---", ConsoleColor.Cyan);
                                    isSucceeded = int.TryParse(Console.ReadLine(), out number);
                                    if (!isSucceeded)
                                    {
                                        ConsoleHelper.WriteWithColor("Inputed number's format is not valid", ConsoleColor.Red);
                                    }
                                    else
                                    {
                                        switch (number)
                                        {
                                            case (int)StudentOptions.CreateStudent:
                                                _studentService.Create(admin);
                                                break;
                                            case (int)StudentOptions.UpdateStudent:
                                                _studentService.Update(admin);
                                                break;
                                            case (int)StudentOptions.DeleteStudent:
                                                _studentService.Delete();
                                                break;
                                            case (int)StudentOptions.GetAllStudents:
                                                _studentService.GetAll();
                                                break;
                                            case (int)StudentOptions.GetAllStudentsByGroup:
                                                _studentService.GetllAllByGroup();
                                                break;
                                            case (int)StudentOptions.BackToMainMenu:
                                                goto MainMenu;
                                                break;

                                            default:
                                                ConsoleHelper.WriteWithColor("Choose a number from 0 to 5!", ConsoleColor.Red);
                                                goto StudentsMenu;
                                                break;
                                        }


                                    }
                                }
                            case (int)MainMenuOptions.Teachers:
                                while (true)
                                {
                                    TeachersMenu: ConsoleHelper.WriteWithColor("1 - Teacher Creation", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("2 - Update Teacher", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("3 - Delete Teacher", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("4 - Get All Teachers", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("0 - Back to Main Menu", ConsoleColor.DarkCyan);
                                    ConsoleHelper.WriteWithColor("--- Select option ---", ConsoleColor.Cyan);
                                    isSucceeded = int.TryParse(Console.ReadLine(), out number);
                                    if (!isSucceeded)
                                    {
                                        ConsoleHelper.WriteWithColor("Inputed number's format is not valid", ConsoleColor.Red);
                                    }
                                    else
                                    {
                                        switch (number)
                                        {
                                            case (int)TeacherOptions.CreateTeacher:
                                                _teacherService.Create();
                                                break;
                                            case (int)TeacherOptions.UpdateTeacher:
                                                _teacherService.Update();
                                                break;
                                            case (int)TeacherOptions.DeleteTeacher:
                                                _teacherService.Delete();
                                                break;
                                            case (int)TeacherOptions.GetAllTeachers:
                                                _teacherService.GetAll();
                                                break;
                                            case (int)TeacherOptions.BackToMainMenu:
                                                goto MainMenu;
                                                break;

                                            default:
                                                ConsoleHelper.WriteWithColor("Choose a number from 0 to 4!", ConsoleColor.Red);
                                                goto TeachersMenu;
                                                break;
                                        }


                                    }
                                }
                            case (int)MainMenuOptions.Logout:
                                goto Authorize;


                            default:
                                ConsoleHelper.WriteWithColor("There is no such an option!", ConsoleColor.Red);
                                goto MainMenu;


                        }
                    }

                }
            }
            




        }
    }
}