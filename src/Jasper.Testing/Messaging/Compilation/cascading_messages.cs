﻿using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Jasper.Testing.Messaging.Compilation
{
    public class source_code_attachment_to_handler_chains : IntegrationContext
    {
        [Fact]
        public async Task should_have_code_on_each_chain()
        {
            await withAllDefaults();

            Handlers.Chains.Any().ShouldBeTrue();

            foreach (var chain in Handlers.Chains)
            {
                // Source generation is lazy now, so you gotta do this
                Handlers.HandlerFor(chain.MessageType);

                chain.SourceCode.ShouldContain(chain.TypeName);

//                Console.WriteLine("Should be " + chain.TypeName);
//                Console.WriteLine("--------------------------------------");
//                Console.WriteLine(chain.SourceCode);
//                Console.WriteLine("--------------------------------------");

            }
        }
    }




}