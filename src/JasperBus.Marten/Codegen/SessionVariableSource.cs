﻿using System;
using Jasper.Codegen;
using Marten;

namespace JasperBus.Marten.Codegen
{
    public class SessionVariableSource : IVariableSource
    {
        public bool Matches(Type type)
        {
            return type == typeof(IQuerySession) || type == typeof(IDocumentSession);
        }

        public Variable Create(Type type)
        {
            if (type == typeof(IQuerySession))
            {
                return new QuerySessionFrame().Session;
            }

            if (type == typeof(IDocumentSession))
            {
                return new SessionFrame().Session;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}