﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Jasper.Messaging.Model;
using Jasper.Messaging.Runtime;
using Jasper.Messaging.Sagas;
using Jasper.Messaging.Tracking;
using Jasper.Messaging.Transports;
using Xunit;


namespace Jasper.Testing.Messaging.Sagas
{
    public abstract class SagaTestHarness<TSagaHandler, TSagaState> : IDisposable
        where TSagaHandler : StatefulSagaOf<TSagaState> where TSagaState : class
    {
        private MessageHistory _history;
        private JasperRuntime _runtime;

        protected async Task withApplication()
        {
            _runtime = await JasperRuntime.ForAsync(_ =>
            {
                _.Handlers.DisableConventionalDiscovery().IncludeType<TSagaHandler>();

                _.Include<MessageTrackingExtension>();

                _.Publish.AllMessagesTo(TransportConstants.LoopbackUri);

                configure(_);
            });

            _history = _runtime.Get<MessageHistory>();
        }


        protected string codeFor<T>()
        {

            return _runtime.Get<HandlerGraph>().HandlerFor<T>().Chain.SourceCode;
        }

        public void Dispose()
        {
#pragma warning disable 4014
            _runtime?.Shutdown();
#pragma warning restore 4014
        }

        protected virtual void configure(JasperRegistry registry)
        {
            // nothing
        }

        protected async Task invoke<T>(T message)
        {
            if (_history == null)
            {
                await withApplication();
            }

            await _runtime.Messaging.Invoke(message);
        }

        protected async Task send<T>(T message)
        {
            if (_history == null)
            {
                await withApplication();
            }

            await _history.WatchAsync(() => _runtime.Messaging.Send(message), 10000);
        }

        protected Task send<T>(T message, object sagaId)
        {
            return _history.WatchAsync(() => _runtime.Messaging.Send(message, e => e.SagaId = sagaId.ToString()), 10000);
        }

        protected TSagaState LoadState(object id)
        {
            return _runtime.Get<InMemorySagaPersistor>().Load<TSagaState>(id);
        }
    }
}
