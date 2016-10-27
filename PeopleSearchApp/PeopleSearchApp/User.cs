namespace PeopleSearchApp
{
    public class User
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="firstName">Users first name</param>
        /// <param name="lastName">Users last name</param>
        public User(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = "";
            Age = 0;
            Interests = "";
            PicFilename = "pack://application:,,,/PeopleSearchApp;component/Resources/default.png";
        }


        public string FirstName{ get; }
        public string LastName { get; }
        public string Name { get { return LastName + ", " + FirstName; }}
        public string Address { get; set; }
        public int Age { get; set; }
        public string Interests { get; set; }
        public string PicFilename { get; set; }

        /// <summary>
        /// Custom hashcode based on users name, address, and age
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode() + this.Address.GetHashCode() + this.Age.GetHashCode();
        }

        /// <summary>
        /// Custom equals based on hashcode
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public override bool Equals(object that)
        {
            return this.GetHashCode() == that.GetHashCode();
        }
    }
}
