using System.Collections.Generic;
using Newtonsoft.Json;
using ORest;

namespace ORestClient.DebugConsole.ODataEntities {
    public class Person {

        public Person() {
            Emails = new List<string>();
            AddressInfo = new List<Location>();
            //Features = new List<Feature>();
            //Friends = new List<Person>();
        }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public PersonGender? Gender { get; set; }
        public long? Age { get; set; }
        public List<string> Emails { get; set; }
        public List<Location> AddressInfo { get; set; }
        public Location HomeAddress { get; set; }
        public Feature? FavoriteFeature { get; set; }
        public List<Feature> Features { get; set; }
        public Person BestFriend { get; set; }

        //[ORestProperty("Friends")]
        [JsonProperty("Friends")]
        public List<Person> AllFriends { get; set; }

    }
}