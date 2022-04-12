using System.Net;


namespace TVMaze.Core
{
    public class ApiResponse
    {
        public ApiResponse(HttpStatusCode status, string json)
        {
            this.Status = status;
            this.Json = json;
        }

        public HttpStatusCode Status { get; set; }
        public string Json { get; set; }

        public void Deconstruct(out HttpStatusCode status, out string json)
        {
            status = this.Status;
            json = this.Json;
        }

    }
}
