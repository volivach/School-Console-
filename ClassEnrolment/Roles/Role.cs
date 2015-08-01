using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ClassEnrolment
{
    enum UserPermissions
    {
        AddStudent,
        EditStudent,
        RemoveStudent,
        ViewStudent,
        AddTeacher,
        RemoveTeacher,
        EditTeacher
    }

    abstract class Role : IRole
    {
        List<UserPermissions> _userPermissions = new List<ClassEnrolment.UserPermissions>();

        public List<UserPermissions> UserPermissions
        {
            get
            {
                return _userPermissions;
            }
        } 
    }

    class TeacherRole : Role
    {
        

        public TeacherRole()
        {
            UserPermissions.Add(ClassEnrolment.UserPermissions.AddStudent);
            UserPermissions.Add(ClassEnrolment.UserPermissions.EditStudent);
            //UserPermissions.Add(ClassEnrolment.UserPermissions.RemoveStudent);
            UserPermissions.Add(ClassEnrolment.UserPermissions.ViewStudent);
        }
    }

    class AdministratorRole : Role
    {
        public AdministratorRole()
        {
            UserPermissions.Add(ClassEnrolment.UserPermissions.AddStudent);
            UserPermissions.Add(ClassEnrolment.UserPermissions.EditStudent);
            UserPermissions.Add(ClassEnrolment.UserPermissions.RemoveStudent);
            UserPermissions.Add(ClassEnrolment.UserPermissions.ViewStudent);
            UserPermissions.Add(ClassEnrolment.UserPermissions.AddTeacher);
            UserPermissions.Add(ClassEnrolment.UserPermissions.RemoveTeacher);
        }
    }

    class StudentRole : Role
    {
        public StudentRole()
        {
            UserPermissions.Add(ClassEnrolment.UserPermissions.ViewStudent);
        }
    }

    class GuestRole : Role
    {
    }
}
