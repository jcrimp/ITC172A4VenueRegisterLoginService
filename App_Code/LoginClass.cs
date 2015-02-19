using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This class takes in the user name and password,
/// retrieves information from the database
/// adn then hashes the password and key to
/// see if it matches the database hash
/// </summary>
public class LoginClass
{
    //class level variables-fields
    private string pass;
    private string username;
    private int seed;
    private byte[] dbHash;
    private int key;
    private byte[] newHash;
	
    //constructor logic takes in password and username
    public LoginClass(string pass, string username)
    {
        this.pass = pass;
        this.username = username;
    }

    //get the user info from database
    private void GetUserInfo()
    {
        //declare the ADO Entities
        ShowTrackerEntities showTrackerDb = new ShowTrackerEntities();
        //query the fields
        var info = from c in showTrackerDb.VenueLogins
                   where c.VenueLoginUserName.Equals(username)
                   select new { c.VenueLoginHashed, c.VenueLoginRandom, c.VenueKey };//FanLoginRandom is the random number

        //loop through the results and assign the
        //values to the field variables
        foreach (var u in info)
        {
            seed = (int)u.VenueLoginRandom;
            dbHash = u.VenueLoginHashed;
            key = (int)u.VenueKey;//cast it to an int
        }
    }

    private void GetNewHash()
    {
        //get the new hash
        PasswordHash h = new PasswordHash();
        newHash = h.HashIt(pass, seed.ToString());
    }

    private bool CompareHash()
    {
        //compare the hashes
        bool goodLogin = false;

        //if the hash doesn't exist
        //because not a valid user
        //the return will be false

        if(dbHash != null)
        {
            //if the hashes do match, return true
            if(newHash.SequenceEqual(dbHash))//SequenceEqual compares arrays
                goodLogin = true;
        }

        return goodLogin;
    }

    public int ValidateLogin()
    {
        //call the methods
        GetUserInfo();
        GetNewHash();
        bool result = CompareHash();//find out if it is false

        //if the result is not true
        //set the key to 0
        if (!result)
            key = 0;

        return key;
    }

}