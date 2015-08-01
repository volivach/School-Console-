using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentsGroup
{
    class Teacher : User
    {
        Regex _regex = new Regex(@"[a-z]");

        public Teacher(string firstName, string lastName, string secondName, string uri, int age)
            : base(firstName, lastName, secondName, uri, age)
        {
        }

        public override string CreatePassword()
        {
            List<char> symbols = new List<char>();

            Random rand = new Random(DateTime.Now.Second);

            for (int i = 0; i < 4; i++)
            {
                symbols.Add((char)rand.Next('a', 'z'));
            }

            for (int i = 0; i < 2; i++)
            {
                symbols.Add((char)rand.Next('0', '9'));
            }

            symbols.AddRange("%^$#@!&*()()_+?><}{|");

            _regex = new Regex(@"[a-z]+\d+[^!,^,#,(,<,>,&,?,),@]+");

            string passwordGen = _regex.Match(new string(symbols.ToArray())).Value;

            return passwordGen;
        }
    }
}
