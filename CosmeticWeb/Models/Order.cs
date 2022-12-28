using System.ComponentModel.DataAnnotations;

namespace CosmeticWeb.Models
{
    public class Order
    {
        #region id e Order
        [Key]
        public Guid Id { get; set; }
        #endregion

        #region Emri i klientit 
        [Display(Name = "Full name")]
        [StringLength(20, MinimumLength = 4)]
        [Required(ErrorMessage = "Customer name is required!")]
        public string? CustomerName { get; set; }
        #endregion

        #region Numri i telefonit te klientit
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone number is required!")]
        public string? CustomerPhone { get; set; }
        #endregion

        #region Emaili i klientit
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required!")]
        public string? CustomerEmail { get; set; }
        #endregion

        #region Adressa e klientit
        [Display(Name = "Adress")]
        [StringLength(20, MinimumLength = 4)]
        [Required(ErrorMessage = "Address is required!")]
        public string? CustomerAddress { get; set; }
        #endregion

        #region Muaji kur kjo porosi eshte krijuar
        public string CreatedDate { get; set; } = DateTime.Now.Month.ToString();
        #endregion

        #region Lidhja me tabelen e OrderItem 1:M
        public IList<OrderItem>? Details { get; set; }
        #endregion
    }
}