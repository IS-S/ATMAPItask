using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ATMAPI.Core
{
    public class PayloadResult
    {
        public int? denomination { get; set; }
        public int? banknotesNum { get; set; }
    }
    public class Result
    {
        public List<PayloadResult> payload { get; set; }
        public string result { get; set; }
        public string timeElapsed  { get; set; }
    }

    public class PayloadModel
    {
        [PosNumber(ErrorMessage = "Should be positive number")]
        public int denomination { get; set; }
        public bool worksProperly { get; set; }
        [PosNumber(ErrorMessage = "Should be positive number")]
        public int banknotesLeft { get; set; }
    }
    public class DataInModel
    {
        [Required]
        public List<PayloadModel> payload { get; set; }

        [PosNumber(ErrorMessage = "Should be positive number")]
        [AtmAmtCheck(ErrorMessage = "Should be a multiple of 100")]
        [FromBody]
        [Required] // this dnw
        public int amount { get; set; }
    }
    public class PosNumber : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            int getal;
            if (int.TryParse(value.ToString(), out getal))
            {
                if (getal >= 0)
                    return true;
            }
            return false;

        }
    }
    public class AtmAmtCheck : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            int getal;
            if (int.TryParse(value.ToString(), out getal))
            {
                if (getal%100==0)
                    return true;
            }
            return false;

        }
    }


}

