using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;
using ZSZService;

namespace ZSZ.Service
{
    public class CommunityService : ICommunityService
    {
        /*
        private CommunityDTO ToDTO(CommunityEntity en)
        {
            CommunityDTO dto = new CommunityDTO();
            dto.bui
        }*/

        public CommunityDTO[] GetByRegionId(long regionId)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<CommunityEntity> bs 
                    = new BaseService<CommunityEntity>(ctx);
                var cities = bs.GetAll().AsNoTracking()
                    .Where(c => c.RegionId == regionId);
                return cities.Select(c=>new CommunityDTO { BuiltYear=c.BuiltYear,
                CreateDateTime = c.CreateDateTime,Id=c.Id,Location=c.Location,
                    Name =c.Name,RegionId=c.RegionId,Traffic=c.Traffic}).ToArray();
            }
        }
    }
}
