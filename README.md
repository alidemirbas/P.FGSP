
Do you want to make server side querying retrieve data from http request directly?..  
For example;
```csharp
var data = _queryFetcher.Fetch(_context.Roles);
```
For doing that, you only have to do add the service like below...
```csharp
builder.Services.AddJQueryQueryFetcher();
//or
builder.Services.AddKendoQueryFetcher();
```

- F Filtering
- G Grouping
- S Sorting
- P Paging

All is done!

It's that much easy 😏


Let me clarify something more...
When you use a javascript library like 'Kendo' or 'Jquert Datatable'
When you create a data table these libraries send a post request and contains data like below 

JQUERY POST

draw: 1

columns[0][data]: id

columns[0][name]: 

columns[0][searchable]: false

columns[0][orderable]: false

columns[0][search][value]: 

columns[0][search][regex]: false

start: 0

length: 10

search[value]: 

search[regex]: false

---------------------------
KENDO POST

"page", "1"

"take", "2"

"filter[logic]", "and"

"filter[filters][0][field]", "Number")

"filter[filters][0][operator]", "eq"

"filter[filters][0][value]", "1"

"sort[0][field]", "Number"

"sort[0][dir]", "asc"


So this library renders these data and retrieves.

All you have to do is injecting IQueryFetcher in a class...
