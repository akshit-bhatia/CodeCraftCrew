namespace Test1.Data;
using System;
using System.ComponentModel.DataAnnotations;

public class User
{
	[Key]
	public Guid Id { get; set; }
	public string? Name { get; set; }
	public string? GeneratedHTML { get; set; }
	public string? Email { get; set; }
	public int? ViewersCount { get; set; }
	public string? ImageURL { get; set; }
}