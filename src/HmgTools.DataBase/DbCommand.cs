using System.Data;

namespace HmgTools.DataBase
{
    public static class DbCommand
    {

        public static void AddParameter(this IDbCommand command, string parameterName, object? value, DbType dbType)
        {
            var param = command.CreateParameter();
            param.ParameterName = parameterName;
            param.Value = value;
            param.DbType = dbType;

            command.Parameters.Add(param);
        }



        public static void AddParametersFromObject(this IDbCommand command, object parameters)
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
                if (dbType != null)
                {
                    //when the type does not have a map to DbType is not gonna be used
                    //TODO: Add some warning in the log
                    continue;
                }


                string parameterName = property.Name;
                object? value = property.GetValue(parameters);

                command.AddParameter(parameterName, value, dbType!.Value);


            }
        }

    }
}