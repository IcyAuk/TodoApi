using System.ComponentModel.DataAnnotations;
using Microsoft.Net.Http.Headers;

public class Todo
{
    public int Id { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    [MaxLength(200)]
    public string? Secret { get; set; }
}