namespace Domains;

public class TbRefreshToken : BaseTable
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; }
}


