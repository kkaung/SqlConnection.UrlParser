using System;
using Xunit;

namespace SqlConnection.UrlParser.Tests
{
    public class UrlParserTests
    {
        [Fact]
        public void Parse_Url_With_Basic_Auth()
        {
            var url = "mssql://myuser:pass1@localhost/my_db";

            var parsedUrl = UrlParser.Parse(url);

            Assert.Equal("server=localhost;database=my_db;user id=myuser;password=pass1;", parsedUrl);
        }

        [Fact]
        public void Parse_Url_With_Integrated_Security_Parses_With_Secured_Connection()
        {
            var url = "mssql://myuser:pass1@localhost/my_db?integrated%20security=false";

            var parsedUrl = UrlParser.Parse(url);

            Assert.Equal("server=localhost;database=my_db;user id=myuser;password=pass1;secured_connection=false", parsedUrl);
        }

        [Theory]
        [InlineData("mssql://localhost/my_db?integrated%20security=true")]
        [InlineData("mssql://localhost/my_db?secured_connection=true")]
        public void Parse_Url_Using_Secured_Connection(string url)
        {
            var parsedUrl = UrlParser.Parse(url);

            Assert.Equal("server=localhost;database=my_db;user id=;password=;secured_connection=true", parsedUrl);
        }

        [Theory]
        [InlineData("mssql://localhost/my_db")]
        [InlineData("mssql://localhost/my_db?integrated%20security=false")]
        [InlineData("mssql://localhost/my_db?secured_connection=false")]
        public void Parse_Url_Without_Basic_Auth_And_Without_Secured_Connection_Throws(string url)
        {
            Assert.Throws<ArgumentException>(() => UrlParser.Parse(url));
        }
    }
}