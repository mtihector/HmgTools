using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HmgTools.DataBase.MySql.Tests.ReadersTests
{
    [TestFixture]
    public class ReadersBaseTests
    {

        public IConfiguration GetConfiguration()
        {

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();

        }


        [Test]
        public void ReaderTest1()
        {
            IConfiguration? configuration = GetConfiguration();

            string? connectionString = configuration.GetConnectionString(ConnectionConst.SecurityConnection);



            List<ObjectValueObject> result = MySqlDbUtils.ExecuteReader<ObjectValueObject>(connectionString, "select Id, Description from Objects",
                null, CommandType.Text);


            foreach (ObjectValueObject item in result)
            {
                Console.WriteLine(item.ToString());
            }

        }




        [Test]
        public void ReaderTest2()
        {
            IConfiguration? configuration = GetConfiguration();

            string? connectionString = configuration.GetConnectionString(ConnectionConst.SecurityConnection);



            List<ObjectValueObject> result = MySqlDbUtils.ExecuteReader<ObjectValueObject>(connectionString, "select Id, Description from Objects where id = @Id",
                new
                {
                    iD = new Guid("9ca68e5a-ad3b-11ec-8f4b-28ee521c19e9")
                }, CommandType.Text, null, "@");


            foreach (ObjectValueObject item in result)
            {
                Console.WriteLine(item.ToString());
            }

        }


        [Test]
        public void StoredProcedureTest1()
        {
            IConfiguration? configuration = GetConfiguration();

            string? connectionString = configuration.GetConnectionString(ConnectionConst.ClassicModelsConnection);



            var result = MySqlDbUtils.ExecuteReader<CustomerValueObject>(connectionString, "GetCustomers",
                new
                {
                    filter = 'a'
                });


            foreach (var customer in result)
            {
                
                    Console.WriteLine(customer);
                

            }

        }

        [Test]
        public void StoredProcedureTest2 ()
        {
            IConfiguration? configuration = GetConfiguration();

            string? connectionString = configuration.GetConnectionString(ConnectionConst.ClassicModelsConnection);



            List<DataTable> result = MySqlDbUtils.ExecuteReaderDataTable(connectionString, "GetCustomer",
                new
                {
                    idCustomer = 112
                });


            foreach (var  table in result)
            {
                Console.WriteLine(table.TableName);

                foreach (DataRow row in table.Rows)                    
                {
                    
                    Console.WriteLine(row[0]);
                }
                
            }

        }


        [Test]
        public void StoredProcedureTest3()
        {
            IConfiguration? configuration = GetConfiguration();

            string? connectionString = configuration.GetConnectionString(ConnectionConst.ClassicModelsConnection);



            List<DataTable> result = MySqlDbUtils.ExecuteReaderDataTable(connectionString, "GetCustomer",
                new
                {
                    idCustomer = 112
                });


            foreach (var table in result)
            {
                Console.WriteLine(table.TableName);

                foreach (DataRow row in table.Rows)
                {

                    Console.WriteLine(row[0]);
                }

            }


            string data = JsonConvert.SerializeObject(result);

            Console.WriteLine(data);

        }


    }

   
}



