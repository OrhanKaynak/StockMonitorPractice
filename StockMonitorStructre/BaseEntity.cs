using System;
using System.Collections.Generic;
using System.Text;

namespace StockMonitorStructre
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
