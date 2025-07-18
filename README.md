﻿## Challenge description
![brewery-banner.png](assets/brewery-banner.png)
Challenge as part of Csharpacademy curriculum: https://www.thecsharpacademy.com/project/64/brewery-api

### Given requirements
Welcome to Belgium! You've been contacted to create a management system for breweries and wholesalers. 
Below are listed the functional and technical requirements sent by your client

- List all beers by brewery
- A brewer can add, delete and update beers
- Add the sale of an existing beer to an existing wholesaler
- Upon a sale, the quantity of a beer needs to be incremented in the wholesaler's inventory
- A client can request a quote from a wholesaler.
- If successful, the quote returns a price and a summary of the quote. A 10% discount is applied for orders above 10 units. A 20% discount is applied for orders above 20 drinks.
- If there is an error, it returns an exception and a message to explain the reason: 
  - order cannot be empty; wholesaler must exist; 
  - there can't be any duplicates in the order; 
  - the number of beers ordered cannot be greater than the wholesaler's stock; 
  - the beer must be sold by the wholesaler
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

table: beers (beerid, breweryid, price)
wholesaler inventory (wholesalerid, beerid, stock)

## To do
- Simplify data model
- Add quote capability to an endpoint
- Improve how list of list is shown in wholesaler and brewery endpoint.
- Have test cases for exceptions -> Tests added. Could still replace with exceptions instead of string literals.
- Return Exceptions instead of strings for errors
- Decide on flat tables vs (wholesaler -> list of beers)

## Api endpoints - by entity
### Beer 
- GET /beer/ => get all beers
- GET /beer/id={id} => get a beer matching a specific id
- POST /brewery/1/beer/name=Jupilerprice=4.5 => Add a new beer to a brewery, passing through the name and price.
- PATCH /beer/id={id}price={price} => change the price of a specific beer
- DELETE /beer/{id} => delete a specific beer

### Brewery
- GET /brewery/ => get all breweries
- GET /brewery/{breweryId} => get 1 brewery by id

Note: updating (patch) or deleting beers can be done with the generic `Beer` endpoints.

### Wholesaler
- GET /wholesaler/ => get all wholesalers
- GET /wholesaler/{id} => get 1 wholesaler by id

### Quote
- GET /quote/sample -> get an example of a quote, using dummy data.
TODO:
- POST /quote/ -> 
- POST /quote/ -> get a real qoute. have to pass the data through (through json I would think, using the existing data classes.)

# Drafts and thought process - clean up later

## Thought process

We can separate the project into:
- SQL database to store data
- EF to talk to database
- REST API
- Business Logic

## DB Schema

## Translating requirements to code
Each brewery has a number of beers, but no inventory.
Wholesaler has an inventory of beers.
- each beer is linked to brewery by id, and price.
Wholesaler has a table with recorded sales.

## Example flow to get first working version - simplest possible scenario

- Create a brewery, with 1 beer type
- Create a wholesaler, with 1 beer type
- Wholesaler purchases 10 beers from brewery, and their inventory is increased.

- Client, through REST api ->
1. Submits a request for an order and gets a summary quote

- Create a client
- Add beer to brewery
- Add beer to wholesaler

- Wholesaler requests a sale of 10 units of beer from brewery.
- Brewery sells 10 units of beer to wholesaler => wholesaler has 10 units of beer in inventory.

- Client requests quote for 10 units of beer to wholesaler.
- Quote is returned with price.
- Client requests quote for 20 units of beer to wholesaler.
- Quote is returned with error message.
