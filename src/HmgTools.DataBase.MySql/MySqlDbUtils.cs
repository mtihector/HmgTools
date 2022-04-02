using MySqlConnector;
using System.Data;

namespace HmgTools.DataBase.MySql;

public class MySqlDbUtils
{



    public static List<T> ExecuteReader<T>(String connectionString, string commandText,
        object inputParams,
        CommandType commandType = CommandType.StoredProcedure,
        object outputParams = null,
        string prefixVariableName="") where T : new() 
    {
        using IDbConnection connection = new MySqlConnection(connectionString);

        IDbCommand command = connection.CreateCommand();
        command.CommandText = commandText;
        command.CommandType = commandType;

        if (inputParams != null)
        {
            command.AddParametersFromObject(inputParams, prefixVariableName);
        }
        if (outputParams != null)
        {
            command.AddParametersFromObject(outputParams, prefixVariableName , ParameterDirection.Output);
        }


        connection.Open();

        var reader = command.ExecuteReader();


        List<T> result = reader.ConvertReaderToList<T>();


        if (outputParams != null)
        {
            //read parameters 
            command.ReadOutputParameters(outputParams);
        }
        

        connection.Close();

        return result;
    }


    public static List<DataTable> ExecuteReaderDataTable(String connectionString, string commandText,
        object inputParams,
        CommandType commandType = CommandType.StoredProcedure,
        object outputParams = null,
        string prefixVariableName = "") 
    {
        using IDbConnection connection = new MySqlConnection(connectionString);

        IDbCommand command = connection.CreateCommand();
        command.CommandText = commandText;
        command.CommandType = commandType;

        if (inputParams != null)
        {
            command.AddParametersFromObject(inputParams);
        }
        if (outputParams != null)
        {
            command.AddParametersFromObject(outputParams,prefixVariableName, ParameterDirection.Output);
        }


        connection.Open();

        var reader = command.ExecuteReader();


        List<DataTable> result = new List<DataTable>();

        while (!reader.IsClosed)
        {
            DataTable dt = new DataTable();
            dt.Load(reader);

            result.Add(dt);
        }


        if (outputParams != null)
        {
            //read parameters 
            command.ReadOutputParameters(outputParams);
        }


        connection.Close();

        return result;
    }

}
