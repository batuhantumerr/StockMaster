using System;
using System.Collections.Generic;
using System.Text;

namespace StockMaster.Core.DTOs
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
