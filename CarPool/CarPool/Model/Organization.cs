using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Organization
    {
        public Int16 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<OrgEvent> Events { get; set; }

        public Organization()
        {
            Events = new List<OrgEvent>();
        }
    }

    public class OrgEvent
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Organization Organization { get; set; } = new Organization();
        public DateTime Time { get; set; }
        public Location Location { get; set; } = new Location();
        public List<Ride> Rides { get; set; } = new List<Ride>();
        public List<Person> RideRequests { get; set; } = new List<Person>();

        public String GetDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name);
            if (Description != null)
            {
                sb.Append(" ").Append(Description);
            }
            sb.Append(" ").Append(Time.ToString());
            sb.Append(" ").Append(Location.ToString());
            return sb.ToString();
        }

        public bool IsDriver(String email)
        {
            if (email == null)
            {
                return false;
            }
            foreach (Ride ride in Rides)
            {
                if (email.Equals(ride.Driver.Email))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPassenger(String email)
        {
            if (email == null)
            {
                return false;
            }
            foreach (Ride ride in Rides)
            {
                foreach (Person passenger in ride.Passengers)
                    if (email.Equals(passenger.Email))
                    {
                        return true;
                    }
            }
            foreach (Person passenger in RideRequests)
            {
                if (email.Equals(passenger.Email))
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveRideRequest(Person passenger)
        {
            int counter = 0;
            foreach ( Person person in RideRequests)
            {
                if (person.Email.Equals(passenger.Email))
                {
                    RideRequests.RemoveAt(counter);
                    break;
                }
                counter++;
            }
        }

            public void RemovePassenger(Person passenger)
        {
            foreach (Ride ride in Rides)
            {
                if (ride.RemovePassenger(passenger))
                {
                    return;
                }
            }
        }

        public String GetDriver(String email)
        {
            if (email == null)
            {
                return null; ;
            }
            foreach (Ride ride in Rides)
            {
                foreach (Person passenger in ride.Passengers)
                    if (email.Equals(passenger.Email))
                    {
                        return ride.Driver.GetFullName();
                    }
            }
            return null;
        }

        public List<Person> GetPassengers(User user)
        {
            if (!user.Driver)
            {
                return new List<Person>();
            }
            foreach (Ride ride in Rides)
            {
                if (ride.Driver != null && ride.Driver.Email != null && ride.Driver.Email.Equals(user.Email))
                {
                    return ride.Passengers;
                }
            }
            return new List<Person>();
        }

    }



    public class Ride
    {
        public Person Driver { get; set; }
        public List<Person> Passengers { get; set; } = new List<Person>();

        public bool RemovePassenger( Person passengerToBeRemoved)
        {
            int counter = 0;
            foreach ( Person passenger in Passengers)
            {
                if (passengerToBeRemoved.Email.Equals(passenger.Email))
                {
                    Passengers.RemoveAt(counter);
                    return true;
                }
                counter++;
            }
            return false;
        }
    }
}
