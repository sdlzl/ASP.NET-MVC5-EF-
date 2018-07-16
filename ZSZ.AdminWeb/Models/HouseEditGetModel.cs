using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class HouseEditGetModel
    {
       public HouseDTO House { get; set; }

        public RegionDTO[] Regions { get; set; }

        public AttachmentDTO[] Attachments { get; set; }

        public IdNameDTO[] RoomTypes { get; set; }

      

        public IdNameDTO[] HouseStates { get; set; }

  

        public IdNameDTO[] DecorateStates { get; set; }

        //public string Id { get; set; }
        //public long RegionId { get; set; }
        //public long HouseTypeId { get; set; }

        //public string Address { get; set; }

        //public double MonthRent { get; set; }
        //public long HouseStateId { get; set; }

        //public double Area { get; set; }
        //public long HouseDecorationId { get; set; }

        //public int FloorIndex { get; set; }

        //public int TotalFloor { get; set; }

        //public string Direction { get; set; }

        //public string LookableDateTime { get; set; }

        //public string CheckInDateTime { get; set; }

        //public string OwnerName { get; set; }

        //public string OwnerPhoneNum { get; set; }

        //public string Description { get; set; }

        //public long[] AttachmentIds { get; set; }






    }
}