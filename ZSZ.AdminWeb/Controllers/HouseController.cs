using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;
using ZSZ.DTO;
using ZSZ.AdminWeb.Models;
using ZSZ.CommonMVC;
using ZSZ.AdminWeb.App_Start;
using System.IO;
using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Filters.Watermarks;

namespace ZSZ.AdminWeb.Controllers
{
    public class HouseController : Controller
    {
        public IRegionService RegionService { get; set; }
        public ICommunityService CommunityService { get; set; }
        public IIdNameService IdNameService { get; set; }
        public IAttachmentService AttachmentService { get; set; }
        public IAdminUserService AdminUserService { get; set; }
        public IHouseService HouseService { get; set; }
        public IHousePicService HousePicService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId">房源类型:整租、合租</param>
        /// <returns></returns>
        // GET: House

        [CheckPermission("House.List")]
        public ActionResult List(long typeId,int pageIndex=1)
        {
            //加了权限验证之后userid一定不为空
            long? AdminUserId = AdminHelper.GetUserId(HttpContext);
            long? CityId = AdminUserService.GetById(AdminUserId.Value).CityId;
            if (CityId == null)
            {
                return View("Error", (object)"总部员工不能管理房源");
            }


            ViewBag.pageIndex = pageIndex;
            ViewBag.totalCount = HouseService.GetTotalCount(CityId.Value, typeId);
            ViewBag.typeId = typeId;     
            //获取所有的房源
            HouseDTO[] houses = HouseService.GetPagedData(CityId.Value,typeId,10,(pageIndex-1)*10);
            return View(houses);
        }

        [CheckPermission("House.Add")]
        [HttpGet]
        public ActionResult Add()
        {

            //获取所有的区域
            long? loginid = AdminHelper.GetUserId(HttpContext);
            if (loginid == null)
            {
                return View("Error", (object)"总部员工不能管理房源");
            }
            //查出该登录者所在的城市Id
            var adminUser = AdminUserService.GetById(loginid.Value);
            //可空类型的转换
            long cityid = adminUser.CityId.Value;
            RegionDTO[] regions = RegionService.GetAll(cityid);

            //获取所有的小区
            //选了区域再刷新

            //获取所有的房型
            IdNameDTO[] RoomType = IdNameService.GetByTypeName("户型");

            IdNameDTO[] HouseType = IdNameService.GetByTypeName("房屋类别");
            //获取所有的房屋状态
            IdNameDTO[] HouseState = IdNameService.GetByTypeName("房屋状态");
            //获取所有的装修状态
            IdNameDTO[] HouseDecorateState = IdNameService.GetByTypeName("装修");
            //获取所有的房屋配置信息
            AttachmentDTO[] Attachments = AttachmentService.GetAll();

            HouseAddGetModel model = new HouseAddGetModel();
            model.Regions = regions;
            model.HouseDecorateStates = HouseDecorateState;
            model.HouseStates = HouseState;
            model.RoomTypes = RoomType;
            model.Attachments = Attachments;
            model.HouseType = HouseType;
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(HouseAddPostModel model)
        {
            HouseAddNewDTO dto = new HouseAddNewDTO();
            dto.Address = model.Address;
            dto.Area = Convert.ToDecimal(model.Area);
            dto.CommunityId = model.CommunityId;
            dto.RoomTypeId = model.RoomTypeId;
            dto.AttachmentIds = model.AttachmentIds;
            dto.CheckInDateTime = model.CheckInDateTime;
            dto.MonthRent = model.MonthRent;
            dto.DecorateStatusId = model.HouseDecorationId;
            dto.StatusId = model.HouseStateId;
            dto.FloorIndex = model.FloorIndex;
            dto.TotalFloorCount = model.TotalFloor;
            dto.Direction = model.Direction;
            dto.LookableDateTime = model.LookableDateTime;
            dto.OwnerName = model.OwnerName;
            dto.OwnerPhoneNum = model.OwnerPhoneNum;
            dto.Description = model.Description;
            dto.TypeId = 12;
            HouseService.AddNew(dto);
            return Json(new AjaxResult() { Status = "ok" });
        }

        public ActionResult LoadCommunities(long regionId)
        {
            CommunityDTO[] model = CommunityService.GetByRegionId(regionId);
            return Json(new AjaxResult() { Status = "ok", Data = model });

        }

        [CheckPermission("House.Delete")]
        public ActionResult Delete(long id)
        {
            HouseService.MarkDeleted(id);
            return Json(new AjaxResult() { Status = "ok" });
        }

        [CheckPermission("House.Delete")]
        public ActionResult BatchDel(long[] houseIds)
        {
            foreach (var s in  houseIds)
            {
                HouseService.MarkDeleted(s);
            }
            return Json(new AjaxResult() { Status = "ok" });

        }


        [CheckPermission("House.Edit")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            //获取所有的区域
            long? loginid = AdminHelper.GetUserId(HttpContext);
            if (loginid == null)
            {
                return View("Error", (object)"总部员工不能管理房源");
            }
            //查出该登录者所在的城市Id
            var adminUser = AdminUserService.GetById(loginid.Value);
            //可空类型的转换
            long cityid = adminUser.CityId.Value;
            RegionDTO[] regions = RegionService.GetAll(cityid);


            //获取所有的房型
            IdNameDTO[] RoomType = IdNameService.GetByTypeName("户型");

            IdNameDTO[] HouseType = IdNameService.GetByTypeName("房屋类别");
            //获取所有的房屋状态
            IdNameDTO[] HouseState = IdNameService.GetByTypeName("房屋状态");
            //获取所有的装修状态
            IdNameDTO[] HouseDecorateState = IdNameService.GetByTypeName("装修");

            //获取所有的房屋配置信息
            AttachmentDTO[] Attachments = AttachmentService.GetAll();

            HouseDTO house = HouseService.GetById(id);

            HouseEditGetModel model = new HouseEditGetModel();
            model.DecorateStates = HouseDecorateState;
            model.House = house;
            model.HouseStates = HouseState;
            model.RoomTypes = RoomType;
            model.Regions = regions;
            model.Attachments = Attachments;
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(HouseEditPostModel model)
        {

            HouseDTO dto = new HouseDTO();
            dto.TypeId = 11;
            dto.Id = model.Id;
            dto.Address = model.Address;
            dto.Area = model.Area;
            dto.AttachmentIds = model.AttachmentIds;
            dto.CheckInDateTime = Convert.ToDateTime(model.CheckInDateTime);
            dto.CityId = Convert.ToInt64(model.Hidden_CityId);
            dto.CommunityId = model.CommunityId;
            dto.CreateDateTime = Convert.ToDateTime(model.Hidden_CreateTime);
            dto.DecorateStatusId = model.HouseDecorationId;
            dto.Description = model.Description;
            dto.Direction = model.Direction;
            dto.FloorIndex = model.FloorIndex;
            dto.LookableDateTime = Convert.ToDateTime(model.LookableDateTime);
            dto.MonthRent = model.MonthRent;
            dto.OwnerName = model.OwnerName;
            dto.OwnerPhoneNum = model.OwnerPhoneNum;
            dto.RegionId = model.RegionId;
            dto.RoomTypeId = model.RoomTypeId;
            dto.StatusId = model.HouseStateId;
            dto.TypeId = 12;
            
            HouseService.Update(dto);
            return Json(new AjaxResult() { Status="ok"});

        }

        /// <summary>
        /// 针对某个房源进行新增
        /// </summary>
        /// <returns></returns>
        public ActionResult PicUpload(long houseId)
        {
            
            return View(houseId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="file">上传表单的名字</param>
        /// <returns></returns>
        public ActionResult UploadPic(long houseId,HttpPostedFileBase file)
        {
            //将文件上传流转化为md5 相同率很低,、
            //算流的md5 从到到尾遍历视频流 转化完成后 指针最后，在保存的时候可能出现保存的文件长度为空
            //解决方法为 将指针的位置复位
            string md5 = Common.CommonHelper.CalcMD5(file.InputStream);
            file.InputStream.Position = 0;//指针复位
            string path ="/upload/"+ DateTime.Now.ToString("yyyy/MM/dd")+"/"+md5+ Path.GetExtension(file.FileName);

            //拿到物理路径
            string fullpath = HttpContext.Server.MapPath("~" + path);


            //路径可能不存在 先创建,这句话意思是 先找到这个路径的文件夹 如果不存在就创建
            new FileInfo(fullpath).Directory.Create();


            //添加水印
            ImageWatermark imgWatermark = new ImageWatermark(HttpContext.Server.MapPath("~/images/watermark.jpg"));
            imgWatermark.ContentAlignment = System.Drawing.ContentAlignment.BottomRight;//水印位置
            imgWatermark.Alpha = 50;//透明度，需要水印图片是背景透明的 png 图片
            ImageProcessingJob jobNormal = new ImageProcessingJob();
            jobNormal.Filters.Add(imgWatermark);//添加水印
            jobNormal.Filters.Add(new FixedResizeConstraint(600, 600));
            jobNormal.SaveProcessedImageToFileSystem(file.InputStream, fullpath);

            file.InputStream.Position = 0;

            //生成缩略图
            ImageProcessingJob jobThumb = new ImageProcessingJob();
            jobThumb.Filters.Add(new FixedResizeConstraint(200, 200));//缩略图尺寸 200*200
            string ThumbUrl = "/upload/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + md5 + "_ThumbUrl" + Path.GetExtension(file.FileName);
            string FullThumbPath = HttpContext.Server.MapPath("~/" + ThumbUrl);
            jobThumb.SaveProcessedImageToFileSystem(file.InputStream, FullThumbPath);


            HouseService.AddNewHousePic(new HousePicDTO() {  HouseId=houseId, Url=path, CreateDateTime=DateTime.Now, ThumbUrl= ThumbUrl });
          // file.SaveAs(fullpath);直接保存成指定规格带水印的图片，原图就不保存了
            return Json(new AjaxResult() { Status="ok"});
        }

        public ActionResult PicList(long houseId)
        {
            var housePic = HousePicService.GetByHouseId(houseId, 0, 10);
            return View(housePic);
        }

        public ActionResult PicBatchDelete(long[] selectedPicIds)
        {
            foreach (var s in selectedPicIds)
            {

                HousePicService.MarkDelete(s);
            }
            return Json(new AjaxResult() { Status="ok"});
        }
    }
}