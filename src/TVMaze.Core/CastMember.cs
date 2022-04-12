using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TVMaze.Core
{
    public class CastMember
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [DisplayFormat(DataFormatString = "yyyy-MM-dd")]
        [BsonDateTimeOptions(DateOnly = true)]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime? Birthday { get; set; }
    }
}
