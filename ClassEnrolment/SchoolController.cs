using ClassEnrolment;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsGroup
{
    class SchoolController
    {
        private List<Group> _groups = new List<Group>();
        private List<User> _users = new List<User>();
        private AuthentificationManager _authManager = new AuthentificationManager();
        static SchoolController _instance;

        private SchoolController()
        {
        }

        public void WriteDataToDatabase()
        {
            using (SchoolEntityModelContainer context = new SchoolEntityModelContainer())
            {

                context.Groups.Load();
                context.Groups.Add(new Groups() { Id = 1, Name = "C#-15-01" });

                context.Users.Load();
                foreach (var it in _users)
                {
                    Users user = new Users()
                    {
                        FirstName = it.FirstName,
                        SecondName = it.SecondName,
                        LastName = it.LastName,
                        imageUri = it.Uri,
                        Age = (short)it.Age,
                        GroupsId = 1
                    };


                    var roles = context.Roles.Local.ToList();
                    var currentRole = roles.FirstOrDefault(i => it.GetRoleType() == i.Role);

                    if (currentRole == null)
                    {
                        currentRole = roles.LastOrDefault();
                        int roleId = currentRole != null ? currentRole.Id + 1 : 1;
                        context.Roles.Add(new Roles() { Role = it.GetRoleType(), Id = roleId });
                        user.RolesId = roleId;
                    }
                    else
                        user.RolesId = currentRole.Id;



                    context.Users.Add(user);
                }
                context.SaveChanges();
            }
        }

        public void ReadDataFromDatabase()
        {
            using (SchoolEntityModelContainer context = new SchoolEntityModelContainer())
            {
                List<string> groupNames =
                    (from currGroup in context.Groups
                    select currGroup.Name).ToList();

                var users =
                (from currUser in context.Users
                 select new
                 {
                     FirstName = currUser.FirstName,
                     Role = currUser.Role,
                     LastName = currUser.LastName,
                     SecondName = currUser.SecondName,
                     Age = currUser.Age,
                     imageUri = currUser.imageUri
                 }
                 ).ToList();

                foreach (var currUser in users)
                {
                    _users.Add(User.CreateUser(currUser.FirstName, currUser.LastName, currUser.SecondName,
                     currUser.imageUri, currUser.Age, currUser.Role.Role));
                 }

                _groups.Add(new Group("C#-15-01", _users.OfType<Student>().ToList()));

                foreach (var it in groupNames)
                {
                    _groups.Add(new Group(it));
                }
            }
        }

        public static SchoolController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SchoolController();

                return _instance;
            }
        }

        public List<Group> Groups
        {
            get
            {
                return _groups;
            }
        }

        public List<User> Users
        {
            get
            {
                return _users;
            }
        }

        public void AddStudent(UserInfo userInfo, GroupID id)
        {
            var group = _groups.Find(it => it.Id == id);
            if (group == null)
                throw new GroupNotFoundException(String.Format("Can't find group : {0}", id.Id));

            Student st = new Student(userInfo.FirstName, userInfo.LastName, userInfo.SecondName, userInfo.Uri, userInfo.Age);
            group.AddStudent(st);
            AddUser(st, "Student");
        }

        public void AddTeacher(UserInfo userInfo)
        {
            Teacher st = new Teacher(userInfo.FirstName, userInfo.LastName, userInfo.SecondName, userInfo.Uri, userInfo.Age);
            AddUser(st, "Teacher");
        }

        public Student FindStudentByString(string st)
        {
            return null;
        }

        private void AddUser(User user, string type)
        {
            _authManager.RegisterNewUser(user, type);
            _users.Add(user);
        }
    }
}
