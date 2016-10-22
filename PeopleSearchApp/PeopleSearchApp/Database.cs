using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleSearchApp
{
    public class Database
    {
        private HashSet<User> users;

        public Database()
        {
            users = new HashSet<User>();
        }

        public bool addUser(User user)
        {
            if (user == null) throw new ArgumentNullException();

            return users.Add(user);
        }

        public IEnumerable<User> allUsers()
        {
            return new HashSet<User>(users);
        }

        public User searchUser(string name)
        {
            User user = new User("Victor", "Kersey");
            user.Address = "2046 N 650 W, Layton, UT";
            user.Age = 28;
            user.Interests = "Programming, other stuff";
            return user;

            //return null;
        }

        public void save()
        {

        }

        public void load()
        {

        }
    }
}
