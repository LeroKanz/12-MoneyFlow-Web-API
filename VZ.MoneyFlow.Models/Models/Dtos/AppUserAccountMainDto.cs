using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.Models.Models.Dtos
{
    public class AppUserAccountMainDto
    {
        public int AffiliateAccountId { get; set; }
        public AccountDto AffiliateAccount { get; set; }

        public string AffiliateUserId { get; set; }
        public AppUser AffiliateUser { get; set; }
    }
}
