using ClassEnrolment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsGroup
{
    interface IView
    {
        event Action<UserInfo> AddStudentEvent;
        event Action<UserInfo> RemoveStudentEvent;
        event Action<UserInfo> AddTeacherEvent;
        event Action<UserInfo> RemoveTeacherEvent;
        event Action<UserInfo> EditStudentEvent;
        event Action<int> ViewStudentEvent;
        event Action<AuthRequestInfo> AuthentificationRequest;

        void ViewStudent(UserInfo st);
        void Authentificate();
        void Run(List<UserPermissions> userPermissions);
    }
}
