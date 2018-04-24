﻿using Jasper.Messaging.Transports.Configuration;

namespace Jasper.SqlServer.Tests.Persistence
{
    public class ItemSender : JasperRegistry
    {
        public ItemSender()
        {
            Settings.PersistMessagesWithSqlServer(ConnectionSource.ConnectionString, "sender");



            Publish.Message<ItemCreated>().To("tcp://localhost:2345/durable");
            Publish.Message<Question>().To("tcp://localhost:2345/durable");

            Transports.LightweightListenerAt(2567);
        }
    }
}
