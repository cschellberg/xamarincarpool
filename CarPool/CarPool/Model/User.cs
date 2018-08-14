using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Model
{
    public class User : Person
    {
        public string Password { get; set; }

    }

    public class Person {

        public string Email { get; set; }

        public bool Driver { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Location Location { get; set; } = new Location();

        public List<Int16> Organizations { get; set; } = new List<Int16>();

        public String PhoneNumber { get; set; }

        public String GetFullName()
        {
            return FirstName + " " + LastName;
        }
        public String GetAddress()
        {
            StringBuilder sb = new StringBuilder();
            if ( Location != null)
            {
                if ( Location.Street != null)
                {
                    sb.Append(Location.Street).Append(", ");
                }
                if ( Location.City != null)
                {
                    sb.Append(Location.City);
                }
            }
            return sb.ToString();
        }

    }

    public class Location
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public GeoLocation location { get; set; }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ").Append(Street).Append(" ").Append(City).Append(" ").Append(State);
            return sb.ToString();
        }

        public String GetLocationParameter()
        {
            return Street + "," + City + "," + State + "," + Country;
        }
    }

    public class GeoLocation
    {
        public GeoLocation(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Latitude {get;}

        public double Longitude {get;}
    }

    public class RideRequest
    {
        public OrgEvent Event { get; set; }
        public Person Passenger { get; set; }
    }

    public class PhoneNumber
    {

        public PhoneNumber(PhoneType type, string number)
        {
            this.Type = type;
            this.Number = number;
        }
        public PhoneType Type { get; }
        public string Number { get; }

        public override String ToString()
        {
            return Number;
        }
    }

    public enum PhoneType
    {
        CELL,
        HOME,
        WORK
    }
}
