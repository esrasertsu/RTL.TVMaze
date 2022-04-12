using MassTransit;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TVMaze.API.Controllers;
using TVMaze.Core;

namespace TVMaze.API.Test
{
    public class TvShowControllerTest
    {
        public class DerivedClass : Response<TvShowWithCast>
        {
            public TvShowWithCast Message => new TvShowWithCast()
            {
                Id = 1,
                Name="testing ",
                Cast = new System.Collections.Generic.List<CastMember> { new CastMember() { 
                  Id = 2,
                  Name = "Test Cast",
                  Birthday =  DateTime.UtcNow
                } }
            };

            public Guid? MessageId => throw new NotImplementedException();

            public Guid? RequestId => throw new NotImplementedException();

            public Guid? CorrelationId => throw new NotImplementedException();

            public Guid? ConversationId => throw new NotImplementedException();

            public Guid? InitiatorId => throw new NotImplementedException();

            public DateTime? ExpirationTime => throw new NotImplementedException();

            public Uri? SourceAddress => throw new NotImplementedException();

            public Uri? DestinationAddress => throw new NotImplementedException();

            public Uri? ResponseAddress => throw new NotImplementedException();

            public Uri? FaultAddress => throw new NotImplementedException();

            public DateTime? SentTime => throw new NotImplementedException();

            public Headers Headers => throw new NotImplementedException();

            public HostInfo Host => throw new NotImplementedException();

            object Response.Message => throw new NotImplementedException();
        }

        TvShowController controller = null;
        [SetUp]
        public void Setup()
        {

            controller = new TvShowController(null);
        }


        [Test]
        public async Task GetTvShows()
        {
            var expectedProduct = new List<TvShowWithCast>()
            {
                new TvShowWithCast()
                {
                    Id = 1,
                    Name = "testing",
                    Cast = new System.Collections.Generic.List<CastMember> { new CastMember() {
                          Id = 2,
                          Name = "Test Cast",
                          Birthday =  DateTime.UtcNow
                        }
                    }
                }
                
            };

            var result = await controller.GetAllTvShows(1);

            Assert.IsTrue((result as IEnumerable<TvShowWithCast>) != null);
            var recievedProduct = (result as IEnumerable<TvShowWithCast>);
            Assert.IsTrue(recievedProduct == expectedProduct);
        }
    }
}