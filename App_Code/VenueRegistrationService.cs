using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "VenueRegistrationService" in code, svc and config file together.
public class VenueRegistrationService : IVenueRegistrationService
{
    ShowTrackerEntities showTrackerDb = new ShowTrackerEntities();

    public bool RegisterVenue(Venue v, VenueLogin vl)
    {
        bool result = true;

        try
        {
            PasswordHash phash = new PasswordHash();
            KeyCode kcode = new KeyCode();
            int key = kcode.GetKeyCode();
            byte[] hash = phash.HashIt(vl.VenueLoginPasswordPlain, key.ToString());

            Venue venue = new Venue();
            venue.VenueName = v.VenueName;
            venue.VenueEmail = v.VenueEmail;
            venue.VenueWebPage = v.VenueWebPage;
            venue.VenuePhone = v.VenuePhone;
            venue.VenueAddress = v.VenueAddress;
            venue.VenueCity = v.VenueCity;
            venue.VenueState = v.VenueState;
            venue.VenueZipCode = v.VenueZipCode;
            venue.VenueAgeRestriction = v.VenueAgeRestriction;
            venue.VenueDateAdded = DateTime.Now;
            showTrackerDb.Venues.Add(venue);

            VenueLogin venueLogin = new VenueLogin();
            venueLogin.Venue = venue;
            venueLogin.VenueLoginUserName = vl.VenueLoginUserName;
            venueLogin.VenueLoginPasswordPlain = vl.VenueLoginPasswordPlain;
            venueLogin.VenueLoginHashed = hash;
            venueLogin.VenueLoginDateAdded = DateTime.Now;
            venueLogin.VenueLoginRandom = key;
            showTrackerDb.VenueLogins.Add(venueLogin);

            showTrackerDb.SaveChanges();
        }

        catch
        {
            result = false;
        }

            return result;
    }

    public int Login(string password, string username)
    {
        LoginClass login = new LoginClass(password, username);
        int key = login.ValidateLogin();
        return key;
    }
}
