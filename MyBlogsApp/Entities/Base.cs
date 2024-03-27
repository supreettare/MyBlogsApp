﻿namespace MyBlogsApp.Entities
{
    public class Base
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 
    }
}
