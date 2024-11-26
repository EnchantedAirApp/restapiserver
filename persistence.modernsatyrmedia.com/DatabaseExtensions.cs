using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistence.modernsatyrmedia.com
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Dapper;
    using Microsoft.Extensions.Configuration;

    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Creates a table based on the properties of the specified generic type T, supporting MySQL, MSSQL, SQLite, and PostgreSQL.
        /// </summary>
        /// <typeparam name="T">The type to map to a SQL table.</typeparam>
        /// <param name="connection">The database connection.</param>
        /// <param name="tableName">The name of the table to create.</param>
        public static void CreateTable<T>(this IDbConnection connection, string tableName)
        {
            // Determine the database type based on the connection type
            string databaseType = GetDatabaseType(connection);

            // Generate the CREATE TABLE script based on type T and database type
            string createTableScript = GenerateCreateTableScript<T>(tableName, databaseType);

            // Execute the create table script
            connection.Execute(createTableScript);
        }

        /// <summary>
        /// Determines the type of the database based on the IDbConnection object.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <returns>The database type as a string.</returns>
        public static string GetDatabaseType(this IDbConnection connection)
        {
            string connectionType = connection.GetType().Name.ToLower();

            if (connectionType.Contains("mysql"))
                return "MySQL";
            if (connectionType.Contains("sqlconnection") || connectionType.Contains("mssql"))
                return "MSSQL";
            if (connectionType.Contains("sqlite"))
                return "SQLite";
            if (connectionType.Contains("npgsql") || connectionType.Contains("postgres"))
                return "PostgreSQL";

            throw new NotSupportedException("Unsupported database type.");
        }

        /// <summary>
        /// Generates a SQL CREATE TABLE script based on the properties of type T and the database type.
        /// </summary>
        /// <typeparam name="T">The type whose properties will be used to create the table.</typeparam>
        /// <param name="tableName">The name of the table to create.</param>
        /// <param name="databaseType">The type of database (MySQL, MSSQL, SQLite, PostgreSQL).</param>
        /// <returns>The generated CREATE TABLE SQL script.</returns>
        public static string GenerateCreateTableScript<T>(this string tableName, string databaseType)
        {
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE [{tableName}] (");

            foreach (var prop in properties)
            {
                string columnName = prop.Name;
                string columnType = GetSqlType(prop.PropertyType, databaseType);

                sb.AppendLine($"    [{columnName}] {columnType},");
            }

            // Remove the last comma
            sb.Length -= 3;
            sb.AppendLine(");");

            return sb.ToString();
        }
        public static IDbConnection ConnectionDB(this IConfiguration config) =>
            default;
        /// <summary>
        /// Maps a .NET type to an appropriate SQL data type based on the database type.
        /// </summary>
        /// <param name="type">The .NET type.</param>
        /// <param name="databaseType">The database type (MySQL, MSSQL, SQLite, PostgreSQL).</param>
        /// <returns>The corresponding SQL data type.</returns>
        public static string GetSqlType(this Type type, string databaseType)
        {
            switch (databaseType)
            {
                case "MySQL":
                    return GetMySqlType(type);
                case "MSSQL":
                    return GetMsSqlType(type);
                case "SQLite":
                    return GetSqliteType(type);
                case "PostgreSQL":
                    return GetPostgreSqlType(type);
                default:
                    throw new NotSupportedException($"Unsupported database type: {databaseType}");
            }
        }

        // Type mapping for MySQL
        public static string GetMySqlType(this Type type)
        {
            if (type == typeof(int)) return "INT";
            if (type == typeof(long)) return "BIGINT";
            if (type == typeof(short)) return "SMALLINT";
            if (type == typeof(bool)) return "TINYINT(1)";
            if (type == typeof(string)) return "VARCHAR(255)";
            if (type == typeof(DateTime)) return "DATETIME";
            if (type == typeof(decimal)) return "DECIMAL(18, 2)";
            if (type == typeof(float)) return "FLOAT";
            if (type == typeof(double)) return "DOUBLE";
            if (type == typeof(Guid)) return "CHAR(36)";  // MySQL doesn't have a native GUID type
            throw new NotSupportedException($"The type {type.Name} is not supported for MySQL.");
        }

        // Type mapping for MSSQL
        public static string GetMsSqlType(this Type type)
        {
            if (type == typeof(int)) return "INT";
            if (type == typeof(long)) return "BIGINT";
            if (type == typeof(short)) return "SMALLINT";
            if (type == typeof(bool)) return "BIT";
            if (type == typeof(string)) return "NVARCHAR(MAX)";
            if (type == typeof(DateTime)) return "DATETIME";
            if (type == typeof(decimal)) return "DECIMAL(18, 2)";
            if (type == typeof(float)) return "REAL";
            if (type == typeof(double)) return "FLOAT";
            if (type == typeof(Guid)) return "UNIQUEIDENTIFIER";
            throw new NotSupportedException($"The type {type.Name} is not supported for MSSQL.");
        }

        // Type mapping for SQLite
        public static string GetSqliteType(this Type type)
        {
            if (type == typeof(int)) return "INTEGER";
            if (type == typeof(long)) return "BIGINT";
            if (type == typeof(short)) return "SMALLINT";
            if (type == typeof(bool)) return "INTEGER";  // SQLite doesn't have a native BOOLEAN type
            if (type == typeof(string)) return "TEXT";
            if (type == typeof(DateTime)) return "DATETIME";
            if (type == typeof(decimal)) return "REAL";  // SQLite doesn't have a DECIMAL type
            if (type == typeof(float)) return "REAL";
            if (type == typeof(double)) return "REAL";
            if (type == typeof(Guid)) return "TEXT";  // SQLite stores GUID as TEXT
            throw new NotSupportedException($"The type {type.Name} is not supported for SQLite.");
        }

        // Type mapping for PostgreSQL
        public static string GetPostgreSqlType(this Type type)
        {
            if (type == typeof(int)) return "INTEGER";
            if (type == typeof(long)) return "BIGINT";
            if (type == typeof(short)) return "SMALLINT";
            if (type == typeof(bool)) return "BOOLEAN";
            if (type == typeof(string)) return "TEXT";
            if (type == typeof(DateTime)) return "TIMESTAMP";
            if (type == typeof(decimal)) return "NUMERIC(18, 2)";
            if (type == typeof(float)) return "REAL";
            if (type == typeof(double)) return "DOUBLE PRECISION";
            if (type == typeof(Guid)) return "UUID";  // PostgreSQL supports UUID
            throw new NotSupportedException($"The type {type.Name} is not supported for PostgreSQL.");
        }
    }

    public static class ConnectionStringExtensions
    {
        // SQL Server Connection String
        public static string ToSqlServerConnectionString(this ConnectionStringParameters parameters)
        {
            return parameters.IntegratedSecurity
                ? $"Server={parameters.Server};Database={parameters.Database};Integrated Security=True;"
                : $"Server={parameters.Server};Database={parameters.Database};User Id={parameters.Username};Password={parameters.Password};";
        }

        // PostgreSQL Connection String
        public static string ToPostgreSqlConnectionString(this ConnectionStringParameters parameters)
        {
            var portSegment = parameters.Port > 0 ? $";Port={parameters.Port}" : "";
            return $"Host={parameters.Server};Database={parameters.Database};Username={parameters.Username};Password={parameters.Password}{portSegment};";
        }

        // MySQL Connection String
        public static string ToMySqlConnectionString(this ConnectionStringParameters parameters)
        {
            var portSegment = parameters.Port > 0 ? $";Port={parameters.Port}" : "";
            return $"Server={parameters.Server};Database={parameters.Database};User={parameters.Username};Password={parameters.Password}{portSegment};";
        }
    }


}
