using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using ZSZService;

namespace ZSZ.Service
{
    public class IdNameService : IIdNameService
    {
        public long AddNew(string typeName, string name)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                IdNameEntity idName =
                    new IdNameEntity { Name = name, TypeName = typeName };

                //todo:检查重复性
                ctx.IdNames.Add(idName);
                ctx.SaveChanges();
                return idName.Id;
            }
        }

        private IdNameDTO ToDTO(IdNameEntity entity)
        {
            IdNameDTO dto = new IdNameDTO();
            dto.CreateDateTime = entity.CreateDateTime;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            dto.TypeName = entity.TypeName;
            return dto;
        }

        public IdNameDTO[] GetAll(string typeName)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<IdNameEntity> bs 
                    = new BaseService<IdNameEntity>(ctx);
                return bs.GetAll().Where(e => e.TypeName == typeName)
                    .ToList().Select(e=>ToDTO(e)).ToArray();
            }
        }

        public IdNameDTO GetById(long id)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<IdNameEntity> bs
                    = new BaseService<IdNameEntity>(ctx);
                return ToDTO(bs.GetById(id));
            }
        }

        public IdNameDTO[] GetByTypeName(string typeName)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<IdNameEntity> bs
                    = new BaseService<IdNameEntity>(ctx);
                return bs.GetAll().Where(e=>e.TypeName==typeName).ToList().Select(e=>ToDTO(e)).ToArray();
            }

        }
    }
}
