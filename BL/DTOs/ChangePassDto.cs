﻿namespace BL.DTOs
{
    public class ChangePassDto
    {
        public string userId { get; set; }
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
    }
}
