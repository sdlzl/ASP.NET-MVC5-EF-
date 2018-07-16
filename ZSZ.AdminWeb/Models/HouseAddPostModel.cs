using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.Models
{
    public class HouseAddPostModel
    {



        [Required]
        public string RegionId { get; set; }

        [Required]
        public long CommunityId { get; set; }

        [Required]
        public long RoomTypeId { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int MonthRent { get; set; }

        [Required]
        public long HouseStateId { get; set; }

        [Required]
        public double Area { get; set; }

        [Required]
        public long HouseDecorationId { get; set; }

        [Required]
        public int FloorIndex { get; set; }

        [Required]
        public int TotalFloor { get; set; }

        [Required]
        public string Direction { get; set; }

        [Required]
        public DateTime LookableDateTime { get; set; }

        [Required]
        public DateTime CheckInDateTime { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Phone]
        [Required]
        public string OwnerPhoneNum { get; set; }

        [MaxLength(120)]
        public string Description { get; set; }

        [Required]
        public long[] AttachmentIds { get; set; }
    }
}