SqlConnection.UrlParser
===

This is a utility to parse connection string url to a known connection string used in `SqlConnection` (also works with EntityFramework).

Installation
---

Open Package Manager Console in Visual Studio and run

```
PM> Install-Package SqlConnection.UrlParser
```

Usages
---

Use the `UrlParser` to parse basic connection url.

```
var url = "mssql://myuser:pass1@localhost/my_db";
var connectionString = UrlParser.Parse(url);

// server=localhost;database=my_db;user id=myuser;password=pass1
```

Supported Url Formats
---

### Basic Url

This is a connection url with basic credentials.

```
mssql://myuser:pass1@localhost/my_db

Output: server=localhost;database=my_db;user id=myuser;password=pass1
```

### Add Parameters Using Query String

To add additional parameters, use query strings.

```
mssql://myuser:pass1@localhost/my_db?secured_connection=false&multipleactiveresultsets=true

Output: server=localhost;database=my_db;user id=myuser;password=pass1;secured_connection=false;multipleactiveresultsets=true
```

### Using Secured Connection

To use `Secured_Connection` add this parameter as a query string.  If you use `Integrated Security`, be sure to url encode the space like so: `Integrated%20Security`.  

**Notes**

- To normalize verbiage, `secured_connection` will be used if you choose to use `Integrated Security`.
- If `secured_connection` is `false` when basic auth is provided, this will throw `ArgumentException`.

```
mssql://localhost/my_db?secured_connection=false

Output: server=localhost;database=my_db;user id=;password=;secured_connection=true;
