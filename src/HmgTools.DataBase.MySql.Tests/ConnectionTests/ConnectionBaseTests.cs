using Microsoft.Extensions.Configuration;
using MySqlConnector;
using NUnit.Framework;
using System.Data;

namespace HmgTools.DataBase.MySql.Tests.ConnectionTests;

[TestFixture]
public class ConnectionBaseTests
{

    public IConfiguration GetConfiguration()
    {

        IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        return builder.Build();

    }

    [Test]
    public void ConnectionCommandTextTest1()
    {
        var configuration = GetConfiguration();

        var connectionString = configuration.GetConnectionString(ConnectionConst.SecurityConnection);

        using IDbConnection connection = new MySqlConnection(connectionString);

        System.Data.IDbCommand command = connection.CreateCommand();
        command.CommandText = "select Id, Description from Objects";
        command.CommandType = CommandType.Text;
        
        connection.Open();

        var reader = command.ExecuteReader();

        
        int idIx = reader.GetOrdinal("Id");
        int descriptionIX = reader.GetOrdinal("Description");

        /*

            https://stackoverflow.com/questions/681653/can-you-get-the-column-names-from-a-sqldatareader

            by Rob Stevenson-Leggett
             var reader = cmd.ExecuteReader();

             var columns = new List<string>();

             for(int i=0;i<reader.FieldCount;i++)
             {
                columns.Add(reader.GetName(i));
             }

        or 
            var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
         */




        while (reader.Read())
        {
            
            Guid id = reader.GetGuid(idIx);
            string description = reader.GetString(descriptionIX);


            Console.WriteLine($"{id} - {description}");
        }




        connection.Close();


    }


    [Test]
    public void ConnectionCommandTextTest2()
    {
        var configuration = GetConfiguration();

        var connectionString = configuration.GetConnectionString(ConnectionConst.SecurityConnection);

        using IDbConnection connection = new MySqlConnection(connectionString);

        System.Data.IDbCommand command = connection.CreateCommand();
        command.CommandText = "select Id, Description from Objects where id = @Id";
        command.CommandType = CommandType.Text;

        var idParam = command.CreateParameter();
        idParam.ParameterName = "@Id";
        idParam.DbType = DbType.Guid;
        idParam.Value = new Guid("9ca68e5a-ad3b-11ec-8f4b-28ee521c19e9");

        command.Parameters.Add(idParam);

        connection.Open();

        var reader = command.ExecuteReader();


        int idIx = reader.GetOrdinal("Id");
        int descriptionIX = reader.GetOrdinal("Description");


        while (reader.Read())
        {

            Guid id = reader.GetGuid(idIx);
            string description = reader.GetString(descriptionIX);


            Console.WriteLine($"{id} - {description}");
        }




        connection.Close();


    }


    [Test]
    public void ConnectionCommandTextInjectionTest3()
    {
        var configuration = GetConfiguration();

        var connectionString = configuration.GetConnectionString(ConnectionConst.SecurityConnection);

        using IDbConnection connection = new MySqlConnection(connectionString);

        System.Data.IDbCommand command = connection.CreateCommand();
        command.CommandText = "select Id, Description from Objects where id ='9ca68e5a-ad3b-11ec-8f4b-28ee521c19e9'; update Objects set Description='Changed' where id ='9ca68e5a-ad3b-11ec-8f4b-28ee521c19e9' ";
        command.CommandType = CommandType.Text;


        connection.Open();

        var reader = command.ExecuteReader();


        int idIx = reader.GetOrdinal("Id");
        int descriptionIX = reader.GetOrdinal("Description");


        while (reader.Read())
        {

            Guid id = reader.GetGuid(idIx);
            string description = reader.GetString(descriptionIX);


            Console.WriteLine($"{id} - {description}");
        }




        connection.Close();


    }


    [Test]
    public void ConnectionCommandTextInjectionTest4()
    {
        var configuration = GetConfiguration();

        var connectionString = configuration.GetConnectionString(ConnectionConst.SecurityConnection);

        using IDbConnection connection = new MySqlConnection(connectionString);

        System.Data.IDbCommand command = connection.CreateCommand();
        command.CommandText = "select Id, Description from Objects where id = @Id";
        command.CommandType = CommandType.Text;

        var idParam = command.CreateParameter();
        idParam.ParameterName = "@Id";
        idParam.DbType = DbType.Guid;
        idParam.Value = $"9ca68e5a-ad3b-11ec-8f4b-28ee521c19e9'' ; update Objects set Description='Injected in Param' where id ='9ca68e5a-ad3b-11ec-8f4b-28ee521c19e9'; --  ";

        command.Parameters.Add(idParam);

        connection.Open();

        var reader = command.ExecuteReader();


        int idIx = reader.GetOrdinal("Id");
        int descriptionIX = reader.GetOrdinal("Description");

        int totRows = 0;
        while (reader.Read())
        {


            Guid id = reader.GetGuid(idIx);
            string description = reader.GetString(descriptionIX);
            Console.WriteLine($"{id} - {description}");

            totRows++;
        }


        Assert.AreEqual(0, totRows, "Error el parámetro no fue sanitizado");




        connection.Close();


    }

}
