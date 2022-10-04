using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContosoUniversity.ViewModels
{
    public class CourseSearch : IValidatableObject
    {
    [Display]
        public string CourseID { get; set; }

        [Display]
        public string Title { get; set; }
    
        [Display]
        public string Credit { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(Title))
            {
                yield return new ValidationResult("Masukkan minimal satu kolom pencarian!");
                //Memberi validation jika sebuah kolom pencarian tidak diisi!
            }

            if (CourseID == null)
            {
                yield return new ValidationResult("Masukkan ID Matkul! ");
                //Memberi validation ke variable yang dituju!
            }

            if (Credit == null)
            {
                yield return new ValidationResult("Masukan SKS");
                //Memberi validation ke variable yang dituju!
            }
        }
    }
    
}