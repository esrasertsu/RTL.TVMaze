# RTL.TVMaze
TVMaze Scrapper Operations<br>
with Docker, Clean Architecture and Repository pattern implementing. <br>
<p>&nbsp;</p>

# Built With
<ul>
<li>Microsoft Visual Studio 2022</li>
<li>ASP.NET Core 6  Application Project - The project that includes Rest API. References .NET 6 </li>
<li>MongoDb and Redis Cache: Containerized TvMazeApi with Redis and Mongodb</li>
<li>RabbitMq for example using</li>
<li>Polly for Resilience: Circuit Breaker Pattern with CircuitBreackerPolicy</li>
</ul>

# Example Response
[
  {
    "id": 4,
    "name": "Arrow",
    "cast": [
      {
        "id": 271,
        "name": "Stephen Amell",
        "birthday": "1981-05-08T00:00:00Z"
      },
      {
        "id": 274,
        "name": "David Ramsey",
        "birthday": "1971-11-17T00:00:00Z"
      },
      {
        "id": 276,
        "name": "Emily Bett Rickards",
        "birthday": "1991-07-24T00:00:00Z"
      },
    ]
  },
  {
    "id": 3,
    "name": "Bitten",
    "cast": [
      {
        "id": 172,
        "name": "Laura Vandervoort",
        "birthday": "1984-09-22T00:00:00Z"
      },
      {
        "id": 173,
        "name": "Greyston Holt",
        "birthday": "1985-09-30T00:00:00Z"
      }
    ]
  }
  ....
  }
]
