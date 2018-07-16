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
    public class PermissionService : IPermissionService
    {
        public long AddPermission(string permName,string description)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<PermissionEntity> permBS = new BaseService<PermissionEntity>(ctx);
                bool exists = permBS.GetAll().Any(p=>p.Name==permName);
                if(exists)
                {
                    throw new ArgumentException("权限项已经存在");
                }
                PermissionEntity perm = new PermissionEntity();
                perm.Description = description;
                perm.Name = permName;
                ctx.Permissions.Add(perm);
                ctx.SaveChanges();
                return perm.Id;
            }
        }

        public void AddPermIds(long roleId, long[] permIds)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<RoleEntity> roleBS 
                    = new BaseService<RoleEntity>(ctx);
                var role = roleBS.GetById(roleId);
                if(role==null)
                {
                    throw new ArgumentException("roleId不存在"+roleId);
                }
                BaseService<PermissionEntity> permBS
                    = new BaseService<PermissionEntity>(ctx);
                var perms = permBS.GetAll()
                    .Where(p => permIds.Contains(p.Id)).ToArray();
                foreach(var perm in perms)
                {
                    role.Permissions.Add(perm);
                }
                ctx.SaveChanges();
            }
        }

        private PermissionDTO ToDTO(PermissionEntity p)
        {
            PermissionDTO dto = new PermissionDTO();
            dto.CreateDateTime = p.CreateDateTime;
            dto.Description = p.Description;
            dto.Id = p.Id;
            dto.Name = p.Name;
            return dto;
        }

        public PermissionDTO[] GetAll()
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<PermissionEntity> bs = new BaseService<PermissionEntity>(ctx);
                return bs.GetAll().ToList().Select(p=>ToDTO(p)).ToArray();
            }
        }

        public PermissionDTO GetById(long id)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<PermissionEntity> bs = new BaseService<PermissionEntity>(ctx);
                var pe = bs.GetById(id);
                return pe == null ? null : ToDTO(pe);
            }
        }

        public PermissionDTO GetByName(string name)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<PermissionEntity> bs = new BaseService<PermissionEntity>(ctx);
                var pe = bs.GetAll().SingleOrDefault(p=>p.Name==name);
                return pe == null ? null : ToDTO(pe);
            }
        }

        public PermissionDTO[] GetByRoleId(long roleId)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<RoleEntity> bs = new BaseService<RoleEntity>(ctx);
                return bs.GetById(roleId).Permissions.ToList().Select(p => ToDTO(p)).ToArray();
            }
        }

        //2,3,4
        //3,4,5
        public void UpdatePermIds(long roleId, long[] permIds)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<RoleEntity> roleBS
                    = new BaseService<RoleEntity>(ctx);
                var role = roleBS.GetById(roleId);
                if (role == null)
                {
                    throw new ArgumentException("roleId不存在" + roleId);
                }
                role.Permissions.Clear();
                BaseService<PermissionEntity> permBS
                    = new BaseService<PermissionEntity>(ctx);
                var perms = permBS.GetAll()
                    .Where(p => permIds.Contains(p.Id)).ToList();
                foreach (var perm in perms)
                {
                    role.Permissions.Add(perm);
                }
                ctx.SaveChanges();
            }
        }

        public void UpdatePermission(long id, string permName, string description)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<PermissionEntity> bs = new BaseService<PermissionEntity>(ctx);
                var result = bs.GetById(id);
                if (result == null)
                {
                    throw new ArgumentException("id：" + id + "的权限项不存在");
                }
                result.Name = permName;
                result.Description = description;
                ctx.SaveChanges();

            }
        }

        public void MarkDeleted(long id)
        {
            using (MyDbContent ctx = new MyDbContent())
            {
                BaseService<PermissionEntity> bs = new BaseService<PermissionEntity>(ctx);
                bs.MarkDeleted(id);
                
            }
        }
    }
}
