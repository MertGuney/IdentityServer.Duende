namespace IdentityServer.Shared.Models
{
    public class ErrorModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }

        public ErrorModel(int code, string message, string description)
        {
            Code = code;
            Message = message;
            Description = description;
        }
    }
}
