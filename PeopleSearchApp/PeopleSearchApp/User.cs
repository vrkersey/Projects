using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleSearchApp
{
    
    public class User
    {
        private string fName;
        private string lName;
        private string address;
        private int age;
        private string interests;
        private string pictureFilePath;

        public User(string firstName, string lastName)
        {
            fName = firstName;
            lName = lastName;
            address = "";
            age = 0;
            interests = "";
        }

        public string Name
        {
            get { return lName + ", " + fName; }
        }

        public string FirstName
        {
            get { return fName; }
        }

        public string LastName
        {
            get { return lName; }
        }

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }

        public int Age
        {
            get
            {
                return age;
            }

            set
            {
                age = value;
            }
        }

        public string Interests
        {
            get
            {
                return interests;
            }

            set
            {
                interests = value;
            }
        }

        public string PicFilename
        {
            get
            {
                return pictureFilePath;
            }

            set
            {
                pictureFilePath = value;
            }
        }

    }
}
