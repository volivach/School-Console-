using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsGroup
{
    class Student : User
    {
        private RecordBook _recordBook;

        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (value > 0 && value < 100)
                    _age = value;
            }
        }


        public Student()
        {

        }

        public Student(string firstName, string lastName, string secondName, string imageUri, int age)
            : base(firstName, lastName, secondName, imageUri, age)
        {
        }

        internal bool IsValid()
        {
            return true;
        }

        public override string CreatePassword()
        {
            return "12345";
        }
    }
}

