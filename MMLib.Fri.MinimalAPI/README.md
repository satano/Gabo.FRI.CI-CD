- [ ] Vytvor projekt cez dotnet cli
- [ ] Vyhod co tam je - minimalny funkčný projekt
- [ ] Pridaj len get na staticky text
- [ ] Ukáž v prehliadači, vysvetli že dlho si clovek nevystačí
  - [ ] Prečo použiť minimal API?
  - [ ] Rýchlosť učenia
  - [ ] Rýchlosť aplikácie
  - [ ] Kompilačná vec
- [ ] Vytvor triedu Contact
- [ ] Vytvor dummy data
- [ ] Vytvor všetky endpointy ale inplace
- [ ] Extrahuj do repository a ukáž dependency injection
- [ ] Ukáž validáciu
  - [ ] 404 - get, put, delete
  - [ ] 400 - post
  - [ ] Result pattern
  - [ ] Extrahuj validáciu do endpoint filtra
- [ ] Dokumentácia
  - [ ] pridaj MapOpenApi
  - [ ] pridaj Scalar
  - [ ] pridaj description
  - [ ] prečo tam už je info o 400?
- [ ] Header parameter
  - [ ] Kešovanie
- [ ] Middlewares
  - [ ] Output cache
  - [ ] Rate limiting
  - [ ] Vlastný middleware
- [ ] Architektúra
  - [ ] Ukázať ako to môže byť organizované

```csharp
services.AddRateLimiter(limiterOptions =>
{
    limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    limiterOptions.AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 2;
        //options.QueueLimit = 2;
        options.Window = TimeSpan.FromSeconds(10);
    });
});
```

```csharp
services.AddOutputCache(options =>
{
    options.AddPolicy("Expire5", builder =>
        builder.Expire(TimeSpan.FromSeconds(5)));
});
```

```csharp
app.Use(async (context, next) =>
{
    var watch = Stopwatch.StartNew();

    await next();

    watch.Stop();
    app.Logger.LogInformation("Request {Method} {Path} took {Duration}ms", context.Request.Method, context.Request.Path, watch.ElapsedMilliseconds);
});
```
