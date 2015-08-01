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
            SchoolController.Instance.ReadDataFromDatabase();

            IView view = new ConsoleView();
            view.AuthentificationRequest += authManager.OnAuthRequest;
            view.Authentificate();
            

            view.AddStudentEvent += (stInfo) =>
            {
                SchoolController.Instance.AddStudent(stInfo, new GroupID("C#-15-01"));
                authManager.SafeData();
                //! TODO: Oprimeze that
                SchoolController.Instance.WriteDataToDatabase(); 
            };

            view.AddTeacherEvent += (stInfo) =>
            {
                SchoolController.Instance.AddTeacher(stInfo);
                authManager.SafeData();
#warning TODO[Andrey, 08.1.2015] - this is not very good idea write to database after each add student operation
                SchoolController.Instance.WriteDataToDatabase();
            };

            view.ViewStudentEvent += (number) =>
            {
                try
                {
                    Student st = SchoolController.Instance.Groups.First()[number - 1];
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
