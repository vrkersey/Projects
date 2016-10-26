using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeopleSearchApp;
using System.Collections.Generic;

namespace Unit_Tests
{
    [TestClass]
    public class UnitTest1
    {
        #region UserTests

        [TestMethod]
        public void HashCodeTest()
        {
            User u1 = new User("firstName","lastName");
            User u2 = new User("firstName", "lastname");
            Assert.IsFalse(u1.GetHashCode().Equals(u2.GetHashCode()));
        }

        [TestMethod]
        public void EqualityTest()
        {
            User u1 = new User("firstName", "lastName");
            User u2 = new User("firstName", "lastName");
            Assert.IsTrue(u1.Equals(u2));

            u1.Age = 25;
            u2.Age = 25;
            Assert.IsTrue(u1.Equals(u2));

            u1.Interests = "some stuff";
            u2.Interests = "other stuff";
            Assert.IsTrue(u1.Equals(u2));

            u1.Address = "123 Main St";
            u2.Address = "123 State St";
            Assert.IsFalse(u1.Equals(u2));
        }

        #endregion

        #region DatabaseTests

        [TestMethod]
        public void EmptyDatabaseTest()
        {
            Database db = new Database();
            HashSet<User> set = new HashSet<User>(db.allUsers());
            Assert.AreEqual(set.Count,0);

        }

        [TestMethod]
        public void addUserTest()
        {
            Database db = new Database();
            db.addUser(new User("firstName", "lastName"));
            HashSet<User> set = new HashSet<User>(db.allUsers());
            Assert.AreEqual(set.Count, 1);
        }

        [TestMethod]
        public void searchUser()
        {
            string firstName = "firstName";
            string lastName = "lastName";
            Database db = new Database();
            User newUser = new User(firstName, lastName);
            db.addUser(newUser);

            User returnedUser = db.searchUser(lastName + ", " + firstName);
            Assert.IsTrue(returnedUser.Equals(newUser));
        }

        [TestMethod]
        public void searchByPrefix()
        {
            User u1 = new User("AFirstName", "BLastName");
            User u2 = new User("BFirstName", "CLastName");
            Database db = new Database();
            db.addUser(u1);
            db.addUser(u2);
            HashSet<User> set1 = new HashSet<User>(db.prefix("a"));
            HashSet<User> set2 = new HashSet<User>(db.prefix("B"));
            HashSet<User> set3 = new HashSet<User>(db.prefix("z"));

            Assert.AreEqual(set1.Count, 1);
            Assert.AreEqual(set2.Count, 2);
            Assert.AreEqual(set3.Count, 0);
        }

        [TestMethod]
        public void LoadTest()
        {
            Database db = new Database();
            int numberOfUsers = 10000;
            for (int i = 0; i < numberOfUsers; i++)
            {
                User u = new User(((char)(66 + i % 25)).ToString(), "LastName");
                u.Age = i;
                db.addUser(u);
            }
            HashSet<User> set1 = new HashSet<User>(db.prefix("a"));
            HashSet<User> set2 = new HashSet<User>(db.prefix("B"));
            HashSet<User> set3 = new HashSet<User>(db.prefix("LastName"));

            Assert.AreEqual(set1.Count, 0);
            Assert.AreEqual(set2.Count, numberOfUsers/25);
            Assert.AreEqual(set3.Count, numberOfUsers);
        }

        #endregion
    }
}
