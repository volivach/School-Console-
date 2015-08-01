using ClassEnrolment;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace StudentsGroup
{
    [Serializable]
    public struct AuthData
    {
        [XmlAttribute]
        public string Login;
        [XmlAttribute]
        public string Password;
        [XmlAttribute]
        public string Role;

    }

    class AuthentificationManager
    {
        /// <summary>
        /// key - login
        /// value - password
        /// </summary>
        Dictionary<string, AuthData> _authData;
        IRole _activeUser;

        public AuthentificationManager()
        {
            _authData = new Dictionary<string, AuthData>();

            _authData.Add("Guest", new AuthData() { Login = "Guest", Password = "", Role = "Guest" });
            _authData.Add("Adm", new AuthData() { Login = "Adm", Password = "1", Role = "Administrator" });
        }

        public IRole CurrentActiveUser
        {
            get
            {
                return _activeUser;
            }
        }

        public void OnAuthRequest(AuthRequestInfo info)
        {
            if (_authData.ContainsKey(info.Login))
            {
                AuthData authData = _authData[info.Login];
                if (authData.Password.Equals(info.Password))
                {
                    _activeUser = CreateUser(authData.Role);
                }
            }
        }

        public void RegisterNewUser(User user, string type)
        {
            string login = String.Format("{0}.{1}", user.FirstName, user.SecondName);
            if (_authData.ContainsKey(login))
                return;

            AuthData userRegisterData = new AuthData() { Login = login, Password = user.CreatePassword(), Role = type };
            
            _authData.Add(login, userRegisterData);
        }

        public void SafeData()
        {
            //serialize
            using (System.IO.Stream stream = File.Open("AuthData.bin", FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, _authData);
            }

            SerializeToXML();
            //WriteDataToDatabase();

        }
        public void ReadData()
        {
            try
            {
                //deserialize
                using (Stream stream = File.Open("AuthData.bin", FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    _authData = (Dictionary<string, AuthData>)bformatter.Deserialize(stream);
                }
            }
            catch (FileNotFoundException)
            {
                // first time run
            }
            catch (Exception)
            {
                Console.Write("exception");
            }

            DeserializeFromXML();
            //ReadDataFromDatabase();
        }

        //private void ReadDataFromDatabase()
        //{
        //    using (AuthDataContainer context = new AuthDataContainer())
        //    {
        //        var query =
        //            from authDataEntity in context.AuthDataEntities
        //            select new 
        //            {
        //                Login = authDataEntity.Login,
        //                Password = authDataEntity.Password,
        //                Role = authDataEntity.Role.Role
        //            };

        //        if(query.Count() > 0)
        //            _authData = query.ToDictionary(i => i.Login, i => new AuthData() { Login = i.Login, Password = i.Password, Role = i.Role});
        //    }
        //}

        //private void WriteDataToDatabase()
        //{
        //    using (AuthDataContainer context = new AuthDataContainer())
        //    {

        //        context.Roles.Load();
        //        foreach (var it in _authData)
        //        {
        //            AuthDataEntity authData = new AuthDataEntity()
        //            {
        //                Login = it.Value.Login,
        //                Password = it.Value.Password,

        //            };

                    
        //            var roles = context.Roles.Local.ToList();
        //            var currentRole = roles.FirstOrDefault(i => it.Value.Role == i.Role);

        //            if (currentRole == null)
        //            {
        //                currentRole = roles.LastOrDefault();
        //                int roleId = currentRole != null ? currentRole.Id + 1 : 1;
        //                context.Roles.Add(new Roles() { Role = it.Value.Role, Id = roleId});
        //                authData.RolesId = roleId;
        //            }
        //            else
        //                authData.RolesId = currentRole.Id;



        //            context.AuthDataEntities.Add(authData);
        //        }
        //        context.SaveChanges();
        //    }
        //}

        private void SerializeToXML()
        {
            ////deserialize
            //using (Stream stream = File.Open("AuthData.xml", FileMode.Create))
            //{
            //    TextWriter writer = new StreamWriter(stream);
            //    XmlDocument xmlDoc = new XmlDocument();
            //    XmlSerializer ser = new XmlSerializer(typeof(XmlElement));
            //    var rootElement = xmlDoc.CreateElement("AuthentificationData");
            //    xmlDoc.AppendChild(rootElement);
            //    foreach (var it in _authData)
            //    {
            //        XmlElement myElement = xmlDoc.CreateElement("AuthData");
            //        myElement.SetAttribute("login", it.Key);
            //        myElement.SetAttribute("password", it.Value.Password);
            //        myElement.SetAttribute("type", it.Value.Role);
            //        rootElement.AppendChild(myElement);
            //    }
            //    ser.Serialize(writer, xmlDoc);
            //}
            using (Stream stream = File.Open("AuthData.xml", FileMode.Create))
            {
                XmlSerializer xsSubmit = new XmlSerializer(typeof(AuthData[]), new XmlRootAttribute("AuthentificationData"));
                var auth = _authData.Select(kv => new AuthData() { Login = kv.Key, Password = kv.Value.Password, Role = kv.Value.Role });
                xsSubmit.Serialize(stream, auth.ToArray());
            }
        }

        private void DeserializeFromXML()
        {
            //deserialize
            //using (Stream stream = File.Open("AuthData.xml", FileMode.Open))
            //{
            //    TextReader writer = new StreamReader(stream);
            //    XmlDocument xmlDoc = new XmlDocument();
            //    XmlReader xmlReader = XmlReader.Create(stream);
            //    while (xmlReader.Read())
            //    {
            //        if (xmlReader.Name == "AuthData")
            //        {
            //            AuthData authData;
            //            string login = xmlReader.GetAttribute("login");
            //            authData.Password = xmlReader.GetAttribute("password");
            //            authData.Role = xmlReader.GetAttribute("type");

            //            if (!string.IsNullOrEmpty(login) && string.IsNullOrEmpty(authData.Password) && string.IsNullOrEmpty(authData.Role))
            //                _authData.Add(login, authData);
            //        }
            //    }
            try
            {
                using (Stream stream = File.Open("AuthData.xml", FileMode.Open))
                {
                    XmlSerializer xsSubmit = new XmlSerializer(typeof(AuthData[]), new XmlRootAttribute("AuthentificationData"));
                    AuthData[] authentificationData = xsSubmit.Deserialize(stream) as AuthData[];
                    _authData = authentificationData.ToDictionary(i => i.Login, i => i);
                }
            }
            catch (InvalidOperationException)
            {

            }
            catch (FileNotFoundException)
            {

            }

            //}
        }

        private IRole CreateUser(string userType)
        {
            if (userType == "Administrator")
                return new AdministratorRole();
            else if (userType == "Teacher")
                return new TeacherRole();
            else if (userType == "Student")
                return new StudentRole();
            else if (userType == "Guest")
                return new GuestRole();

            return null;
        }
    }
}
