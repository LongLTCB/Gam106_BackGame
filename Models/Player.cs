using System.ComponentModel.DataAnnotations;

// THÊM DÒNG KHAI BÁO NAMESPACE NÀY:
namespace WebApplication1.Models;

public class Player
{
    public int PlayerId { get; set; }

    [Required(ErrorMessage = "Tên người chơi không được để trống")]
    [StringLength(100)]
    public string Name { get; set; }

    [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ")]
    public string Email { get; set; }

    public int Score { get; set; }
}