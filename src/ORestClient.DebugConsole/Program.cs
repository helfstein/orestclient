using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ORest;
using ORest.Interfaces;
using ORestClient.DebugConsole.ODataEntities;

namespace ORestClient.Samples {
    class Program {
        private static IORestClient gwClient;

        static async Task Main() {
            
            try {
                var endpoint = "https://services.odata.org/TripPinRESTierService/(S(vr5pauqj22rd43uupqjzuq2k))/";
                var settings = new ORestClientSettings {
                    BaseUrl = $"{endpoint}",
                    UseBasicAuth = false,
                    ImplementationType = ODataImplementation.ODataV4
                };

                gwClient = new ORest.ORestClient(settings);
                Expression<Func<Person, bool>> exp = p => p.FirstName == "Russell";

                var nome = "Mundy";

                if (!string.IsNullOrWhiteSpace(nome)) {
                    exp.And(p => p.FirstName.Equals(nome));
                }

                var av = await gwClient.For<Person>("People").Filter(exp).Expand(x=>x.AllFriends).FindEntriesAsync();

                var friends = await gwClient.For<Person>("People").Key("russellwhyte")
                    .Navigate<Person>(x => x.AllFriends).Expand(x => x.AllFriends).FindEntriesAsync();
                
                var username = DateTime.Now.ToString("yyyMMddhhmmss");
                var person = new Person {
                    UserName = username,
                    FirstName = "asdasdasd",
                    LastName = "aisjsiadjs",
                    //FavoriteFeature = Feature.Feature2,
                    Emails = new List<string> {
                        "teste@ttt.com"
                    },
                    AddressInfo = new List<Location> {
                        new Location {
                            Address = "Teste",
                            City = new City {
                                CountryRegion = "Brazil",
                                Region = "One",
                                Name = "São Paulo"
                            }
                        }
                    }

                };

                try {

                    var a = await gwClient
                        .For<Person>("People")
                        .Key("russellwhyte")
                        .Navigate<Trip>("Trips")
                        .Key(0)
                        .Unbound<Person>()
                        .Function("Microsoft.OData.Service.Sample.TrippinInMemory.Models.GetInvolvedPeople")
                        .ExecuteAsEnumerableAsync();
                    Console.WriteLine($"função retorno count = {a?.Count()}");

                    await gwClient.For<Person>("People")
                        .Key("russellwhyte")
                        .Unbound()
                        .Action("Microsoft.OData.Service.Sample.TrippinInMemory.Models.ShareTrip", new {
                            userName = "scottketchum",
                            tripId = 0
                        })
                        .ExecuteAsync();
                    //await gwClient.Unbound<Person>().Function("").UseMethod(Method.Get).ExecuteAsync()
                    var lista4 = await gwClient.For<Person>("People")
                        .FindEntriesAsync();
                    Console.WriteLine($"{lista4.Count()} antes");
                    var addedPerson = await gwClient.For<Person>("People").InsertEntryAsync(person);
                    Console.WriteLine($"Nome antes de editar = {addedPerson.LastName}");
                    Console.WriteLine(addedPerson);
                    lista4 = await gwClient.For<Person>("People")
                        //.Filter(x=>x.UserName == username.ToString())
                        .FindEntriesAsync();
                    Console.WriteLine($"{lista4.Count()} depois");
                    await gwClient.For<Person>("People")
                        .Key(username)
                        .UpdateEntryAsync(new {
                            LastName = $"Edit=>{DateTime.Now}"
                        });

                    var confEdited = await gwClient.For<Person>("People")
                        .Key(username)
                        .FindEntryAsync();
                    Console.WriteLine($"Nome editado = {confEdited.LastName}");

                    await gwClient.For<Person>("People")
                        .Key(username)
                        .DeleteEntryAsync();
                    lista4 = await gwClient.For<Person>("People").FindEntriesAsync();
                    Console.WriteLine($"{lista4.Count()} depois deletado");

                    var customAttrName = await gwClient.For<Person>("People")
                        .Expand(x => x.AllFriends)
                        .FindEntriesAsync();
                    Console.WriteLine($"{customAttrName.Count()} depois deletado");
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    //throw;
                }


            }
            catch (Exception e) {
                Console.WriteLine(e);
            }

            Console.ReadKey();


        }
    }
}
