using System;
using System.Collections.Generic;
using System.Text;

namespace StockMaster.Application.DTOs
{
    public abstract class BaseDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
