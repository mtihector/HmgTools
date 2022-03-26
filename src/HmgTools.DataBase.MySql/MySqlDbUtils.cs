using MySqlConnector;
using System.Data;

namespace HmgTools.DataBase.MySql;

public class MySqlDbUtils
{



    public static List<T> ExecuteReader<T>(String connectionString, string commandText,
        object inputParams,
        CommandType commandType = CommandType.StoredProcedure )  where T: new()
    {
        using IDbConnection connection = new MySqlConnection(connectionString);

        IDbCommand command = connection.CreateCommand();
        command.CommandText = commandText;
        command.CommandType = commandType;

        if (inputParams != null)
        {
            command.AddParametersFromObject(inputParams);
        }


        connection.Open();

        var reader = command.ExecuteReader();


        List<T> result = reader.ConvertReaderToList<T>();

        

        connection.Close();

        return result;
    }

}
