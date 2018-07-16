using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.Service;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZService
{
    public class HousePicService : IHousePicService
    {
        public void AddNew(long houseid, string url, string ThumbUrl)
        {
            using (MyDbContent ctx=new MyDbContent())
            {
                HousePicEntity picentity = new HousePicEntity();
                picentity.HouseId = houseid;
                picentity.Url = url;
                picentity.ThumbUrl = ThumbUrl;
                ctx.HousePics.Add(picentity);
                ctx.SaveChanges();
            }
        }

        public HousePicDTO[] GetByHouseId(long houseId, int startIndex, int pageSize)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<HousePicEntity> housepicService = new BaseService<HousePicEntity>(ctx);
               var result= housepicService.GetAll().Where(e => e.HouseId == houseId).OrderByDescending(e => e.CreateDateTime).
                    Skip(startIndex).Take(pageSize);
                List<HousePicDTO> list = new List<HousePicDTO>();
                foreach (var a in result)
                {
                    HousePicDTO dto = new HousePicDTO();
                    dto.CreateDateTime = a.CreateDateTime;
                    dto.HouseId = a.HouseId;
                    dto.Id = a.Id;
                    dto.ThumbUrl = a.ThumbUrl;
                    dto.Url = a.Url;
                    list.Add(dto);
                }
                return list.ToArray();
            }
        }

        public void MarkDelete(long HousePicId)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<HousePicEntity> housepicService = new BaseService<HousePicEntity>(ctx);
                housepicService.MarkDeleted(HousePicId);
            }

        }

        public void Update(long houseid, string url, string ThumbUrl,long id)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
               var result= ctx.HousePics.SingleOrDefault(e => e.Id == id);
                if (result!=null)
                {
                    result.HouseId = houseid;
                    result.Url = url;
                    result.ThumbUrl = ThumbUrl;
                    ctx.SaveChanges();
                }

            }
        }
    }
}
