using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZService
{
    public  class MyDbContent:DbContext
    {
        private static ILog log = LogManager.GetLogger(typeof(MyDbContent));
        public MyDbContent() : base("name=conStr")
        {
           Database.SetInitializer<MyDbContent>(null);
            this.Database.Log = (sql) =>
            {
                log.DebugFormat("EF执行的sql为:{0}", sql);
            };
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<AdminUserEntity> AdminUsers { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<CommunityEntity> Communities { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RegionEntity> Regions { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
        public DbSet<AttachmentEntity> Attachments { get; set; }
        public DbSet<HouseEntity> Houses { get; set; }
        public DbSet<HouseAppointmentEntity> HouseAppointments { get; set; }
        public DbSet<IdNameEntity> IdNames { get; set; }
        public DbSet<HousePicEntity> HousePics { get; set; }
        public DbSet<AdminLogEntity> AdminUserLogs { get; set; }


    }
}
