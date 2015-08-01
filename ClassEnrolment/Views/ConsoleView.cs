using ClassEnrolment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudentsGroup
{
    class ConsoleView : IView
    {
        public event Action<UserInfo> AddStudentEvent;
        public event Action<UserInfo> EditStudentEvent;
        public event Action<UserInfo> RemoveStudentEvent;
        public event Action<int> ViewStudentEvent;
        public event Action<AuthRequestInfo> AuthentificationRequest;
        public event Action<UserInfo> AddTeacherEvent;
        public event Action<UserInfo> RemoveTeacherEvent;

        public void ViewStudent(UserInfo st)
        {
            Console.WriteLine(st.FirstName);
            Console.WriteLine(st.SecondName);
            Console.WriteLine(st.LastName);
            Console.WriteLine(st.Age);

            Console.ReadKey();
        }

        public ConsoleView()
        {

        }

        public void Run(List<UserPermissions> userPermissions)
        {
            string selection;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\t\t\t\tClass Enrolment");

                if (userPermissions.Contains(UserPermissions.AddStudent))
                    Console.WriteLine("1) Add student");
                if (userPermissions.Contains(UserPermissions.RemoveStudent))
                    Console.WriteLine("2) Remove student");
                if (userPermissions.Contains(UserPermissions.EditStudent))
                    Console.WriteLine("3) Edit student");
                if (userPermissions.Contains(UserPermissions.ViewStudent))
                    Console.WriteLine("4) View student");
                if (userPermissions.Contains(UserPermissions.AddTeacher))
                    Console.WriteLine("5) Add Teacher");
                if (userPermissions.Contains(UserPermissions.RemoveTeacher))
                    Console.WriteLine("6) Remove Teacher");

                Console.WriteLine("Select operation: ");
                selection = Console.ReadLine();
                UserInfo userInfo = new UserInfo();

                switch (selection)
                {
                    case "1":
                        if (!userPermissions.Contains(UserPermissions.AddStudent))
                            break;


                        Console.WriteLine("Enter first name: ");
                        userInfo.FirstName = Console.ReadLine();
                        Console.WriteLine("Enter second name: ");
                        userInfo.SecondName = Console.ReadLine();
                        Console.WriteLine("Enter last name: ");
                        userInfo.LastName = Console.ReadLine();
                        Console.WriteLine("Enter age: ");
                        userInfo.Age = int.Parse(Console.ReadLine());

                        var temp = Interlocked.CompareExchange<Action<UserInfo>>(ref AddStudentEvent, null, null);
                        if (temp != null)
                            temp(userInfo);

                        break;
                    case "2":
                        if (!userPermissions.Contains(UserPermissions.RemoveStudent))
                            break;

                        break;
                    case "3":

                        break;
                    case "4":
                        Console.Write("Enter number of student: ");
                        int number = int.Parse(Console.ReadLine());

                        if (ViewStudentEvent != null)
                            ViewStudentEvent(number);

                        break;

                    case "5":

                        if (!userPermissions.Contains(UserPermissions.AddTeacher))
                            break;

                        Console.WriteLine("Enter first name: ");
                        userInfo.FirstName = Console.ReadLine();
                        Console.WriteLine("Enter second name: ");
                        userInfo.SecondName = Console.ReadLine();
                        Console.WriteLine("Enter last name: ");
                        userInfo.LastName = Console.ReadLine();
                        Console.WriteLine("Enter age: ");
                       userInfo.Age = int.Parse(Console.ReadLine());

                        if (AddTeacherEvent != null)
                            AddTeacherEvent(userInfo);

                        break;

                    default:
                        Console.WriteLine("Incorrect operation!");
                        break;
                }
            }
        }

        public void Authentificate()
        {
            AuthRequestInfo authInfo = new AuthRequestInfo();
            Console.Write("Enter login: ");
            authInfo.Login = Console.ReadLine();
            Console.Write("Enter password: ");
            authInfo.Password = Console.ReadLine();

            if (AuthentificationRequest != null)
                AuthentificationRequest(authInfo);
        }
    }
}
