using Microsoft.AspNetCore.Identity;
using System;

namespace VZ.MoneyFlow.Entities.DbSet
{
    public class RefreshToken
    {
        public int Id { get; set; }        
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime ExpiryTime { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
