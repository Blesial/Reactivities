namespace Application.Core
{
    public class AppException
    {


        public AppException(int statusCode, string exceptionMessage, string details = null)
        {
            this.StatusCode = statusCode;
            this.ExceptionMessage = exceptionMessage;
            this.Details = details;

        }
        public int StatusCode { get; set; }

        public string ExceptionMessage { get; set; }

        public string Details { get; set; } // we dont want to show this in production mode
    }
}