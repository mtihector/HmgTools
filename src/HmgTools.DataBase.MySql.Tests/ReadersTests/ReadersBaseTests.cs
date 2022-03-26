using Microsoft.Extensions.Configuration;
using MySqlConnector;
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
            var configuration = GetConfiguration();

            var connectionString = configuration.GetConnectionString(ConnectionConst.SecurityConnection);



            List<ObjectValueObject> result = MySqlDbUtils.ExecuteReader<ObjectValueObject>(connectionString, "select Id, Description from Objects",
                null, CommandType.Text);


            foreach(ObjectValueObject item in result)
            {
                Console.WriteLine(item.ToString());
            }

        }

    }
}
