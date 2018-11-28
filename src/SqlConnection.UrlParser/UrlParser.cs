using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlConnection.UrlParser
{
    public static class UrlParser
    {
        private const string SECURED_CONNECTION = "secured_connection";

        public static readonly Dictionary<string, string> KnownKeys = new Dictionary<string, string>
        {
            ["integrated security"] = SECURED_CONNECTION,
        };

        /// <summary>
        /// Parse connection url.
        /// </summary>
        /// <param name="url">Connection url.</param>
        /// <returns>Connection string.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string Parse(string url)
        {
            var builder = new StringBuilder();

            var uri = new Uri(url);
            var username = string.Empty;
            var password = string.Empty;
            var server = uri.Host;
            var database = uri.LocalPath.TrimStart('/');

            if (!string.IsNullOrEmpty(uri.UserInfo))
            {
                var userInfoParts = uri.UserInfo.Split(':');
                username = userInfoParts[0];
                password = userInfoParts[1];
            }

            builder.Append($"server={server};")
                   .Append($"database={database};")
                   .Append($"user id={username};")
                   .Append($"password={password};")
                   .AppendQueryStringParameters(uri);

            var connectionString = builder.ToString();

            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                if (!connectionString.Contains($"{SECURED_CONNECTION}=true"))
                    throw new ArgumentException("Connection string must use secured_connection when basic credentials are not provided.");

            return connectionString;
        }

        private static StringBuilder AppendQueryStringParameters(this StringBuilder builder, Uri uri)
        {
            var queryParams = uri.Query
                .TrimStart('?')
                .Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(q => q.Split('='));

            foreach (var values in queryParams)
            {
                var key = Uri.UnescapeDataString(values[0]).ToLower();
                var value = Uri.UnescapeDataString(values[1]).ToLower();

                if (KnownKeys.ContainsKey(key))
                    key = KnownKeys[key];

                builder.Append($"{key}={value}");
            }

            return builder;
        }
    }
}
