using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Data.Repositories.Base;

namespace Data.Repositories.Abstract
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
    }
}
