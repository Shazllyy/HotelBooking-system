﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBookingClient.Models
{
   
    public  class room 
    {
        [Required]
        [Key]
        [DisplayName("Room ID")]
        public int room_id { get; set; }
        [Required]
        [DisplayName("Room Capacity")]
        public int room_capacity { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Room Status")]        
        public string room_status { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Room Type")]
        public string room_type { get; set; }
        [Required]
        [DisplayName("Room Price")]
        public decimal room_price { get; set; }
        [Required]
        //[Remote(action: "VerifyRoomRoom", controller: "Room",ErrorMessage ="Number already exists in this branch")]
        [DisplayName("Room Number")]
        public int room_number { get; set; }
        [Required]
        [DisplayName("Branch ID")]
        public int branch_id { get; set; }
        
        [DisplayName("Room Residents Count")]
        public int room_residents_count { get; set; } = 0;
       
    }
}