## Challenge description
Challenge as part of Csharpacademy curriculum: https://www.thecsharpacademy.com/project/64/brewery-api

### Requirements
Welcome to Belgium! You've been contacted to create a management system for breweries and wholesalers. 

Below are listed the functional and technical requirements sent by your client

- List all beers by brewery
- A brewer can add, delete and update beers
- Add the sale of an existing beer to an existing wholesaler
- Upon a sale, the quantity of a beer needs to be incremented in the wholesaler's inventory
- A client can request a quote from a wholesaler.
- If successful, the quote returns a price and a summary of the quote. A 10% discount is applied for orders above 10 units. A 20% discount is applied for orders above 20 drinks.
- If there is an error, it returns an exception and a message to explain the reason: order cannot be empty; wholesaler must exist; there can't be any duplicates in the order; the number of beers ordered cannot be greater than the wholesaler's stock; the beer must be sold by the wholesaler
- A brewer brews one or several beers
- A beer is always linked to a brewer
- A beer can be sold by several wholesalers
- A wholesaler sells a defined list of beers, from any brewer, and has only a limited stock of those beers
- The beers sold by the wholesaler have a fixed price imposed by the brewery
- For this assessment, it is considered that all sales are made without tax
- The database is pre-filled by you
- No front-end is needed, just the API
- Use REST architecture
- Use Entity Framework
- No migrations are needed; use Ensure Deleted and Ensure Created to facilitate development and code reviews.

## Translating requirements to code
Each brewery has a number of beers, but no inventory.
Wholesaler has an inventory of beers.
- each beer is linked to brewery by id, and price.
Wholesaler has a table with recorded sales.

## Example flow to get first working version - simplest possible scenario

- Create a brewery, with 1 beer type
- Create a wholesaler, with 1 beer type
- Create a client
- Add beer to brewery
- Add beer to wholesaler

- Wholesaler requests a sale of 10 units of beer from brewery.
- Brewery sells 10 units of beer to wholesaler => wholesaler has 10 units of beer in inventory.

- Client requests quote for 10 units of beer to wholesaler.
- Quote is returned with price.
- Client requests quote for 20 units of beer to wholesaler.
- Quote is returned with error message.
