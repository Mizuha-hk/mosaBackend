﻿using System.ComponentModel.DataAnnotations;

namespace mosaCupBackend.Models.DbModels
{
    public class Follow
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Uid { get; set; }
        [Required]
        public string FollowedUid { get; set; }
    }
}
