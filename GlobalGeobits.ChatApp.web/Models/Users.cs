using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlobalGeobits.ChatApp.web.Models
{

    public enum Gender { 
    Mael, Femael
    }

    public class Users
    {
        [Key]
        public int UserID { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
          ErrorMessageResourceName = "Validation_reqired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "Validation_maxlength")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_fn")]
        public string UserFristName { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "Validation_reqired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "Validation_maxlength")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_ln")]
        public string UserLastName { get; set; }


        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_dn")]
        [RegularExpression(@"^[a-zA-Z0-9_]{1,3}[a-zA-Z0-9 @#$-]*$",ErrorMessage ="Writ a name or keep blank")]
        public string UserDisplayName { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
          ErrorMessageResourceName = "Validation_reqired")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_gender")]
        public Gender UserGender { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "Validation_reqired")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_dob")]
        [DataType(DataType.Date)]
        [Plus18(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "Validation_birthdate")]
        public DateTime UserDateOfBirth { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Validation_reqired")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Validation_invalid_mail")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_mail")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Validation_reqired")]
        [RegularExpression(@"^((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})$", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Validation_invalid_pass")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_pass")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "Validation_reqired")]
        [Compare("UserPassword", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Validation_Confirmpass")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_confirm_pass")]
        [DataType(DataType.Password)]
        public string UserConfirmPassword { get; set; }

        public int Status { get; set; }

        // the list of all sent messages by a spasific account
        public virtual ICollection<Messages> SentMessages { get; set; }

        // the list of all received messages by a spasific account
        public virtual ICollection<Messages> ReceivedMessages { get; set; }


    }

    /// <summary>
    /// a custom validation attribute that validate that account age is plus 18 years
    /// </summary>
 
    public  class Plus18 : ValidationAttribute
    {

        /// <summary>
        /// overrided IsValid method that check the validation 
        /// So, if account age is plus 18 so it valid and return true
        /// else return false is no Date of Birhth is enterd or age is less than 18
        /// </summary>
        /// <param name="value"> Value is the Date of Birth of the account </param>
        /// <returns>
        /// the Isvalid method return True is account age is equal or plus 18 Years,
        /// return false otherwise
        /// 
        /// </returns>
        public override bool IsValid(object value)
        {

            var DOB = value as DateTime?;

            if (DOB == null) return false;
            if (DOB != null && DOB.HasValue) {

                int ageYears = DateTime.Now.Year - DOB.Value.Year;

                if (ageYears >= 18)
                    return true;
            
            }
            return false;
        }
    }
}