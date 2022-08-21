using Domains.DTOs;
using Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines_logic.Interface
{
    public interface ILinkService
    {
        Task<bool> AddLink(LinkDTO link);
        Task<bool> DeleteLink(int id);
        Task<Link> GetLink(int id);
        Task<IEnumerable<Link>> GetLinks(LinkFilteringModel filteringModel);
    }
}
