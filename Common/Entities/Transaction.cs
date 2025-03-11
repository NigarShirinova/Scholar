using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Constants;

namespace Common.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public  decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public Guid PaymentToken { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
