using System.Data;
using System.Reflection;

namespace HmgTools.DataBase
{
    public static class DbCommand
    {

        public static void AddParameter(this IDbCommand command, string parameterName, object? value, DbType dbType, ParameterDirection direction)
        {
            IDbDataParameter? param = command.CreateParameter();
            param.ParameterName = parameterName;
            param.Value = value;
            param.DbType = dbType;
            param.Direction = direction;
            command.Parameters.Add(param);
        }



        public static void AddParametersFromObject(this IDbCommand command, object parameters, string prefixVariableName = "", ParameterDirection direction = ParameterDirection.Input)
        {
            if (parameters == null)
            {
                //when parameter is null should not execute this method
                //TODO: Add some warning in the log
                return;
            }

            var mapper = DbTypeMapper.GetInstance();
            var properties = parameters.GetType().GetProperties();

            foreach (var property in properties)
            {
                //check if the parameter has a map

                DbType? dbType = mapper.GetTypeFromCLR(property.PropertyType);
                if (dbType == null)
                {
                    //when the type does not have a map to DbType is not gonna be used
                    //TODO: Add some warning in the log
                    continue;
                }


                string parameterName = $"{prefixVariableName}{property.Name}";


                object? value = property.GetValue(parameters);

                command.AddParameter(parameterName, value, dbType.Value, direction);


            }
        }


        public static void ReadOutputParameters(this IDbCommand command, object outputParams)
        {
            var properties = outputParams.GetType().GetProperties();
            foreach (IDbDataParameter param in command.Parameters)
            {
                if (param.Direction == ParameterDirection.Output)
                {
                    PropertyInfo? property = properties.FirstOrDefault(p => p.Name.ToLower().Equals(param.ParameterName.ToLower()));

                    if (property != null)
                    {
                        try
                        {
                            property.SetValue(outputParams, param.Value, null);
                        }
                        catch (Exception ex)
                        {
                            //TODO add exception here
                        }
                    }

                }
            }
        }

    }
}