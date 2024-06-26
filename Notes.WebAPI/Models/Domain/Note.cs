﻿namespace Notes.WebAPI.Models.Domain;

public class Note
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsLiked { get; set; }
    public bool IsDone { get; set; }
    public string CreatedAt { get; set; } = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

    public string UserId { get; set; } 
    public ApplicationUser User { get; set; }
}
