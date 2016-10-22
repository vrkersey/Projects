using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleSearchApp
{
    public class Database
    {
        private List<User> fullName;
        private List<User> firstName;
        private List<User> lastName;

        public Database()
        {
            fullName = new List<User>();
            firstName = new List<User>();
            lastName = new List<User>();
        }

        public bool addUser(User user)
        {
            if (user == null) throw new ArgumentNullException();

            fullName.Add(user);
            firstName.Add(user);
            lastName.Add(user);
            fullName.Sort(delegate (User u1, User u2) { return u1.Name.CompareTo(u2.Name); });
            firstName.Sort(delegate (User u1, User u2) { return u1.FirstName.CompareTo(u2.FirstName); });
            lastName.Sort(delegate (User u1, User u2) { return u1.LastName.CompareTo(u2.LastName); });
            return true;
        }

        public IEnumerable<User> allUsers()
        {
            return new HashSet<User>(fullName);
        }

        public User searchUser(string name)
        {
            foreach (User u in fullName)
                if (u.Name == name)
                    return u;

            foreach (User u in firstName)
                if (u.FirstName == name)
                    return u;

            foreach (User u in lastName)
                if (u.FirstName == name)
                    return u;

            return null;
        }

        public IEnumerable<User> prefix(int r)
        {


            return null;
        }

        public void save()
        {

        }

        public void load()
        {

        }
    }
}
