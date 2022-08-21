using Bussines_logic.Interface;
using DataAccess.Interfaces;
using Domains.DTOs;
using Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines_logic.Services
{
    public class LinkService : ILinkService
    {
        private ILinkRepository _linkRepository { get; set; }
        public LinkService(ILinkRepository linkRepository)
        {
            _linkRepository = linkRepository;
        }
        public async Task<IEnumerable<Link>> GetLinks(LinkFilteringModel filteringModel)
        {
            filteringModel.SearchTerm ??= "";
            var links = await _linkRepository.GetFiltered(filteringModel,x=>x.FullUrl.Contains(filteringModel.SearchTerm));
            return links;
        }
        public async Task<Link> GetLink(int id)
        {
            var link = await _linkRepository.GetById(id);
            return link;
        }
        private string GetShortLink(string host)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomLetter = new string(Enumerable.Repeat(chars, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return host + "/" + randomLetter;
        }
        public async Task<bool> AddLink(LinkDTO linkDTO)
        {
            var links = await _linkRepository.Get(x => x.FullUrl == linkDTO.Url);
            if (links.Count != 0)
            {
                return false;
            }
            var host = linkDTO.CurrentUrl.Replace(linkDTO.Path, "");
            var shortLink = GetShortLink(host);
            var link = new Link()
            {
                CreationTime = DateTime.Now,
                FullUrl = linkDTO.Url,
                ShortUrl = shortLink,
            };

            await _linkRepository.Insert(link);
            await _linkRepository.Save();
            return true;
        }
        public async Task<bool> DeleteLink(int id)
        {
            var link = await _linkRepository.GetById(id);
            if (link == null)
            {
                return false;
            }

            _linkRepository.Delete(link);
            return true;
        }
    }
}
