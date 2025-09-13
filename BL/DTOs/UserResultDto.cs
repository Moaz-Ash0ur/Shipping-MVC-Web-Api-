namespace BL.DTOs
{
    public class UserResultDto
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Message { get; set; }

    }


}
