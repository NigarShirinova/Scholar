using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;

namespace Business.ViewModels.TeacherDashboard
{
    public class BalanceIndexVM
    {
     
        public decimal Balance { get; set; } 
        public List<Transaction> TransactionList { get; set; }
    }
}
