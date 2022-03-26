using System.Data;
using System.Reflection;

namespace HmgTools.DataBase
{
    public static class ReaderUtils
    {
        public static List<T> ConvertReaderToList<T>(this IDataReader reader) where T : new()
        {

            if (reader.IsClosed)
            {
                throw new Exception("Datareader is closed, to be able to convert the reader to a List needs to be opened");
            }

            List<T> list = new List<T>();

            // ordinal to property Name
            Dictionary<int, PropertyInfo> columnsMap = new Dictionary<int, PropertyInfo>();

            var properties = typeof(T).GetProperties();


            for (int i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);

                PropertyInfo? property = properties.FirstOrDefault(p => p.Name.ToLower().Equals(name.ToLower()));

                if (property != null)
                {
                    columnsMap.Add(i, property);
                }
            }


            while (reader.Read())
            {
                T item = new T();
                foreach (var column in columnsMap)
                {
                    object value = reader.GetValue(column.Key);
                    try
                    {
                        column.Value.SetValue(item, value);
                        
                    }
                    catch(Exception ex)
                    {
                        //TODO: add error/log,etc..
                    }

                }

                list.Add(item);
               
            }



            return list;
        }
    }
}
