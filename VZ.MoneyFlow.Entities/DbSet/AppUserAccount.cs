using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VZ.MoneyFlow.Entities.DbSet
{
    public class AppUserAccount
    {
        public int AffiliateAccountId { get; set; }
        public Account AffiliateAccount { get; set; }

        public string AffiliateUserId { get; set; }
        public AppUser AffiliateUser { get; set; }
    }
}
