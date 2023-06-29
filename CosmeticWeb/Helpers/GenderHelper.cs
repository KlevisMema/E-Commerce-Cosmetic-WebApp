using Microsoft.AspNetCore.Mvc.Rendering;

namespace CosmeticWeb.Helpers
{
    /// <summary>
    ///     Nje klase ndihmuese.
    /// </summary>
    public class GenderHelper
    {
        /// <summary>
        ///     Nje metode qe kthen nje selected liste me strings 
        ///     ku do te perdoret tek  forma e rregjstrimit tek
        ///     drop downi.
        /// </summary>
        /// <returns> Nje liste me string. </returns>
        public static SelectList GetGender()
        {
            List<string> genders = new List<string>()
            {
                "Male",
                "Female"
            };

            return new SelectList(genders);

        }
    }
}
