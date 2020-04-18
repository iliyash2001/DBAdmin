using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using System.Data.Common;
using System.Configuration;

namespace DBAdmin.Shared
{
    public static class SharedLib
    {

        public static string GetRDSConnectionString()
        {
            var appConfig = ConfigurationManager.AppSettings;

            string dbname = appConfig["RDS_DB_NAME"];

            if (string.IsNullOrEmpty(dbname)) return null;

            string username = appConfig["RDS_USERNAME"];
            string password = appConfig["RDS_PASSWORD"];
            string hostname = appConfig["RDS_HOSTNAME"];
            string port = appConfig["RDS_PORT"];

            return "metadata=res://*/DBAdminEntities.csdl|res://*/DBAdminEntities.ssdl|res://*/DBAdminEntities.msl;provider=System.Data.SqlClient;provider connection string='Data Source=" + hostname + ";Initial Catalog=" + dbname + ";User ID=" + username + ";Password=" + password + ";App=EntityFramework'";
        }

        private static DbConnection CheckSqlConnection(Server server)
        {
            var connection = new SqlConnection("Data Source='" + server.ServerName.Trim() + "';Initial Catalog=DBAdmin;User ID='" + server.UserId.Trim() + "';Password='" + server.Password.Trim() + "'");


            try

            {
                connection.Open();
            }

            catch (Exception)

            {


            }

            return connection;
        }

        private static DbConnection CheckPostgressConnection(Server server)
        {
            string connstring = String.Format("Server={0};Port={1};" +
                    "User Id={2};Password={3};Database={4};",
                    server.ServerName, "5432", server.UserId,
                    server.Password, "pgcr");
            // Making connection with Npgsql provider
            NpgsqlConnection connection = new NpgsqlConnection(connstring);

            try

            {
                connection.Open();
               
            }

            catch (Exception)

            {


            }

            return connection;


        }

        public static ConnectionState CheckConnection(Server server)

        {

            switch (server.DBType.ToString().Trim())
            {
                case "SQL":
                    return CheckSqlConnection(server).State;
                case "Postgress":
                    return CheckPostgressConnection(server).State;
                default:
                    return ConnectionState.Broken;

            }


        }

        public static bool CreateDataBase(Server server, string dbname)

        {

            switch (server.DBType.ToString().Trim())
            {
                case "SQL":
                    return CreateSqlDataBase(server,dbname);
                case "Postgress":
                    return CreatePostgressDataBase(server, dbname);
                default:
                    return false;

            }
        }

        public static bool CreatePostgressDataBase(Server server, string DbNAme)
        {
            
            string command = "CREATE DATABASE " + DbNAme.Trim() + " WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'en_US.UTF-8' LC_CTYPE = 'en_US.UTF-8';";
            

            string connstring = String.Format("Server={0};Port={1};" +
                    "User Id={2};Password={3};Database={4};",
                    server.ServerName.Trim(), "5432", server.UserId.Trim(),
                    server.Password.Trim(), "pgcr");
            // Making connection with Npgsql provider
            NpgsqlConnection connection = new NpgsqlConnection(connstring);
            NpgsqlCommand cmd = new NpgsqlCommand(command, connection);


            try

            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }

            catch (Exception)

            {
                return false;

            }

            return false;


        }
        public static bool CreatePostgressTable(string DbNAme, Dictionary<string, DataBaseType> tableColumns)
        {
            return true;
        }

        public static bool CreateSqlDataBase(Server server, string DbNAme)
        {
            var connection = new SqlConnection("Data Source='" + server.ServerName.Trim() +
                "';Initial Catalog=DBAdmin;User ID='" + server.UserId.Trim() + "';Password='" + 
                server.Password.Trim() + "'");

            var strCmd = "CREATE DATABASE " + DbNAme.Trim();
            SqlCommand cmd = new SqlCommand(strCmd, connection);

            
            try

            {
                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }

            catch (Exception)

            {

                return false;
            }
        }
        public static bool CreateSqlTable(DBAdmin.Controllers.DataTable dataTable, Server server)
        {

            string cmd = "DROP TABLE [" + dataTable.TableName.Trim() + "] CREATE TABLE [" + dataTable.TableName.Trim() + "]( ";
            var i = 0;
            foreach (var row in dataTable.Rows)
            {
                if(i > 0)
                {
                    cmd += " , ";
                }
                cmd += row.ColumnName + " " + row.ColumnType;
                i++;
            }
            cmd += " )";
            var connection = new SqlConnection("Data Source='" + server.ServerName.Trim() +
               "';Initial Catalog=" + dataTable.DBName + ";User ID='" + server.UserId.Trim() + "';Password='" +
               server.Password.Trim() + "'");

            SqlCommand sqlCmd = new SqlCommand(cmd, connection);
           
            try

            {
                connection.Open();
                sqlCmd.ExecuteScalar();
                return true;
            }

            catch (Exception ex)
            
            {
                try
                {
                    cmd = "select count(*) from " + dataTable.DBName;
                    sqlCmd = new SqlCommand(cmd, connection);
                    var r = sqlCmd.ExecuteScalar();
                    return true;
                }
                catch (Exception ex1)
                {
                    return false;

                }
                

                return false;
            }

           
        }

        public static List<string> GetSqlDBType()
        {
            var DBType = new List<string>();
            DBType.Add("int");
            DBType.Add("varchar(50)");
            DBType.Add("varchar(100)");
            DBType.Add("varchar(200)");
            DBType.Add("varchar(500)");
            DBType.Add("bool");
            DBType.Add("double");

            return DBType;

        }

        public static List<string> GetPgDBType()
        {
            var DBType = new List<string>();
            DBType.Add("int");
            DBType.Add("varchar(50)");
            DBType.Add("varchar(100)");
            DBType.Add("varchar(200)");
            DBType.Add("varchar(500)");
            DBType.Add("bool");
            DBType.Add("double");

            return DBType;

        }


        public static List<DataBase>  GetDataBases(Server server)
        {
            var lstDB = new List<DataBase>();
            var connection = new SqlConnection("Data Source='" + server.ServerName.Trim() +
                "';Initial Catalog=DBAdmin;User ID='" + server.UserId.Trim() + "';Password='" +
                server.Password.Trim() + "'");

            var strCmd = "select [name] as database_name  from sys.databases order by name" ;
            SqlCommand cmd = new SqlCommand(strCmd, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {                while (reader.Read())
                {
                    lstDB.Add(new DataBase
                    {
                        DBName = reader.GetString(0)
                    });
                }
            }
            else
            {
                lstDB.Add(new DataBase
                {
                    DBName = "No rows found."
                });

               
            }

            reader.Close();

            return lstDB;
        }

    }

    


    

}

