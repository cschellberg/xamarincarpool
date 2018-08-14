using System;
using System.Collections.Generic;
using Model;

namespace Service
{
    public class EventService
    {
        private static EventService eventService;

        private List<OrgEvent> orgEvents;

        public EventService()
        {
            orgEvents = createOrgEvents();
        }

        public static EventService getInstance()
        {
            if (eventService == null)
            {
                eventService = new EventService();
            }
            return eventService;
        }

        public List<OrgEvent> GetOrgEvents(int month, int year)
        {
            return orgEvents;
        }

        private OrgEvent getOrgEvent(Int32 eventId)
        {
            foreach (OrgEvent orgEvent in orgEvents)
            {
                if (orgEvent.Id == eventId)
                {
                    return orgEvent;
                }
            }
            return null;
        }

        public bool HasUserRequestedRide(OrgEvent orgEvent, Person passenger)
        {
            if (orgEvent.RideRequests != null)
            {
                foreach (Person person in orgEvent.RideRequests)
                {
                    if (person.Email.Equals(passenger.Email))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Person> GetPassengers(Int32 eventId)
        {
            User user = UserService.getInstance().GetUser();
            OrgEvent orgEvent = getOrgEvent(eventId);
            if (orgEvent != null)
            {
                return orgEvent.GetPassengers(user);

            }
            return new List<Person>();
        }


        public bool CancelRideRequest(RideRequest rideRequest)
        {
            rideRequest.Event.RemoveRideRequest(rideRequest.Passenger);
            rideRequest.Event.RemovePassenger(rideRequest.Passenger);
            return true;
        }

        public bool AddRideRequest(RideRequest rideRequest)
        {
            rideRequest.Event.RideRequests.Add(rideRequest.Passenger);
            return true;
        }

        public bool AssignRequest(Int32 eventId, Person requester)
        {
            OrgEvent orgEvent = getOrgEvent(eventId);
            if (orgEvent != null && orgEvent.RideRequests != null)
            {
                foreach (Person person in orgEvent.RideRequests)
                {
                    if (person.Email.Equals(requester.Email))
                    {
                        orgEvent.RideRequests.Remove(person);
                        return true;
                    }
                }
            }
            return false;
        }

        private List<OrgEvent> createOrgEvents()
        {
            Organization organization = new Organization();
            organization.Name = "Philadelphia Baha'i Community";
            Location location = new Location();
            location.Street = "Bryn Mahr Ave";
            location.City = "Philadelphia";
            location.State = "Pa";
            location.Country = "USA";
            List<OrgEvent> retList = new List<OrgEvent>();
            for (int ii = 0; ii < 9; ii++)
            {
                DateTime dateTime = DateTime.Now;
                dateTime.AddDays(2 * ii);
                OrgEvent orgEvent = new OrgEvent();
                orgEvent.Id = ii;
                orgEvent.Time = dateTime;
                orgEvent.Name = "Devotional";
                orgEvent.Location = location;
                orgEvent.Organization = organization;
                User user = UserService.getInstance().GetUser();

                if (ii % 3 == 0)
                {
                    Ride ride = new Ride();
                    ride.Driver = user;
                    Person passenger = createPerson();
                    ride.Passengers.Add(passenger);
                    orgEvent.Rides.Add(ride);
                }
                else if (ii % 3 == 1)
                {
                    Ride ride = new Ride();
                    ride.Driver = createPerson();
                    ride.Passengers.Add(user);
                    orgEvent.Rides.Add(ride);
                }
                else
                {
                    Person passenger = createPerson();
                    orgEvent.RideRequests.Add(passenger);
                }
                retList.Add(orgEvent);
            }
            return retList;
        }

        public List<Person> GetRequests(Int32 eventId)
        {
            foreach (OrgEvent orgEvent in orgEvents)
            {
                if (orgEvent.Id == eventId)
                {
                    if (orgEvent.RideRequests != null)
                    {
                        return orgEvent.RideRequests;
                    }
                    else
                    {
                        return new List<Person>();
                    }
                }
            }
            return new List<Person>();
        }

        private Person createPerson()
        {
            Person person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "jdoe@gmail.com",
                PhoneNumber = "48468883233",

            };
            person.Location = new Location
            {
                Street = "815 W. Bridge Street",
                City = "Phoenixville",
                State = "Pa",
                Country = "USA"
            };
            return person;
        }

    }
}
