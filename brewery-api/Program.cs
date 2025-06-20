using brewery_api;
using brewery_api.Utilities;

var db = new BreweryContext();
var generator = new MockDataGenerator(db);
await generator.GenerateAsync();