using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsGroup
{
    class Group
    {
        readonly private List<Student> _students;
        private GroupID _id;
        public const int MaxCountOfStudents = 50;

        public Student this[int index]
        {
            get
            {
                return _students[index];
            }
        }

        public Group(string id)
        {
            _id = new GroupID(id);

            _students = new List<Student>();
        }

        public Group(string id, List<Student> students)
            : this(id)
        {
            _students.AddRange(students);
        }

        public GroupID Id
        {
            get
            {
                return _id;
            }
        }


        public void AddStudent(Student st)
        {
            if (_students.Count == MaxCountOfStudents)
                throw new ClassFullException("Can't add student, cuz class is full");

            if (st.IsValid())
                _students.Add(st);
        }

        public void RemoveStudent(Student st)
        {
            _students.Remove(st);
        }
    }
}
