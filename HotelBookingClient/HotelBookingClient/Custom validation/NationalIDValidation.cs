using System.ComponentModel.DataAnnotations;

namespace HotelBookingClient.Custom_validation
{
    public class NationalIDValidation : ValidationAttribute
    {
        
        //public override bool IsValid(object value)
        //{
        //    if (value.ToString().Length != 14)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }
}
