
namespace ServiceAppApi.ModelDTO
{
    public class ResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public object Data { get; set; }
        public int TotalCount { get; set; }
    }
}
