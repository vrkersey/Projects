Author
---------------------------------------------------------------------------
Victor Kersey
vrkersey@gmail.com
https://github.com/vrkersey


PeopleSearchApp
---------------------------------------------------------------------------
People Search App is an application for storing users and user profiles in a 
database. The GUI provides a way for people to search for users and, when 
found, update the profile for the user. User profiles contain an immutable
first and last name, as well as their address, age, interests and a picture.

The application can accept up to 1000 users before prefix takes too long to
run.

TODO's
---------------------------------------------------------------------------
* Add the ability to Save and Load a database
	- find the best method to fully import user pictures

* Find a better way of searching for a user when the database contains two
  or more users with the same name
	- May be possible to search by a UserID

* A way to delete users
	- This could be done by a button in Users that is enabled only when 
	  a user has been found by a search

* Implement a more efficient prefix search
	- sort the Users by first and last name then use a binary search to find
	  the first and last users that match then return those users.

Bugs
---------------------------------------------------------------------------
* If there are two or more users with the same name, when that name is 
  searched the user that was added first will be selected.
