using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class HouseAddGetModel
    {
        public RegionDTO[] Regions { get; set; }

        public IdNameDTO[] RoomTypes { get; set; }

        public IdNameDTO[] HouseStates { get; set; }

        public IdNameDTO[] HouseDecorateStates { get; set; }

        public IdNameDTO[] HouseType { get; set; }

        public CommunityDTO[] CommunityIds { get; set; }

        public AttachmentDTO[] Attachments { get; set; }

    }
}