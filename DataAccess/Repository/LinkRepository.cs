using DataAccess.Interfaces;
using Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LinkRepository: GenericRepository<Link>, ILinkRepository
    {   

        public LinkRepository(ApplicationContext context):base(context)
        {
            
        }     
    }
}
