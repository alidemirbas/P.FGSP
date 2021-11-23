You wanna create IQueryable object from http request directly?..

```csharp
IEnumerable<KendoFooDataTable> dataResult = _fooQueryableDataTableResult
                .Build(_requestForm) //like HttpContext.Request.Form
                .ToList();
```

It's that much easy 😏 Check the QueryTest.cs file for more examples. https://github.com/alidemirbas/P.FGSP/blob/master/QueryTest.cs

Ok but wait this time...
What is that? 😕 (check out the image too https://github.com/alidemirbas/P.FGSP/blob/master/_http_post_request_form_example.PNG)

-----------------------------------------------------

"page", "1"
"take", "2"

"filter[logic]", "and"

"filter[filters][0][field]", "Number")
"filter[filters][0][operator]", "eq"
"filter[filters][0][value]", "1"

"filter[filters][1][field]", "Name"
"filter[filters][1][operator]", "eq"
"filter[filters][1][value]", "a"

"sort[0][field]", "Number"
"sort[0][dir]", "asc"

-----------------------------------------------------

let me clarify...
That is a http post request form values from something like javascript datatable for "server side" querying (filtering, grouping, sorting, paging)
(btw it's from Telerik Kendo)
(and im gonna implement for jquery datatable in this century 🤗)
