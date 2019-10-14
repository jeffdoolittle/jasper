﻿using System.Collections.Generic;
using Jasper.Conneg;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;

namespace JasperHttp
{
    public class HttpSerializationGraph : SerializationGraph<IRequestReader, IResponseWriter>
    {
        public HttpSerializationGraph(IEnumerable<ISerializerFactory<IRequestReader, IResponseWriter>> serializers, IEnumerable<IRequestReader> readers, IEnumerable<IResponseWriter> writers) : base(serializers, readers, writers)
        {
        }
    }
}