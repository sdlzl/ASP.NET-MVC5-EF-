using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IHousePicService:IServiceSupport
    {
        void AddNew(long houseid,string url,string ThumbUrl);

        void MarkDelete(long HousePicId);

        HousePicDTO[] GetByHouseId(long houseId,int startIndex,int pageSize);

        void Update(long houseid, string url, string ThumbUrl,long id);
    }
}
