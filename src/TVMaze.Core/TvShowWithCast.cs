
namespace TVMaze.Core
{
    using MongoDB.Bson.Serialization.Attributes;
    using Newtonsoft.Json.Converters;
    using System.Linq;

    public class TvShowWithCast
    {
        [BsonElement("_id")]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<CastMember> Cast { get; set; } = new List<CastMember>();
        public TvShowWithCast OrderByBirthday()
        {
            Cast = Cast.OrderByDescending(c => c.Birthday).ToList();
            return this;
        }
    }


    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }


    
}