using brewery_api;

var db = new BreweryContext();
var generator = new MockDataGenerator(db);
await generator.GenerateAsync();