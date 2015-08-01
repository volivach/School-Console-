using ClassEnrolment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsGroup
{
    class Program
    {
        static void Main(string[] args)
        {
            Group studentGroup = new Group("C#-15-01");
            AuthentificationManager authManager = new AuthentificationManager();
            authManager.ReadData();
            IView view = new ConsoleView();
            view.AuthentificationRequest += authManager.OnAuthRequest;
            view.Authentificate();
            

            view.AddStudentEvent += (stInfo) =>
            {
                Student st = new Student(stInfo.FirstName, stInfo.LastName, stInfo.SecondName, "", stInfo.Age);
                studentGroup.AddStudent(st);
                authManager.RegisterNewUser(st, "Student");

                authManager.SafeData();
            };

            view.AddTeacherEvent += (stInfo) =>
            {
                Teacher st = new Teacher(stInfo.FirstName, stInfo.LastName, stInfo.SecondName, "", stInfo.Age);
                authManager.RegisterNewUser(st, "Teacher");

                authManager.SafeData();
            };

            view.ViewStudentEvent += (number) =>
            {
                try
                {
                    Student st = studentGroup[number - 1];
                    UserInfo stInfo = new UserInfo() { Age = st.Age };
                    view.ViewStudent(stInfo);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Exception occured: can't find student with number:" + number);
                }

            };

            view.Run(authManager.CurrentActiveUser.UserPermissions);
        }
    }
}
