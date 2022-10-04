using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContosoUniversity.ViewModels
{
    public class StudentSearch : IValidatableObject
    {
        [Display]
        public string LastName { get; set; }

        [Display]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [Display]
        public DateTime? EnrollmentDateFrom { get; set; } // ? (NULLABLE)

        [DataType(DataType.Date)]
        [Display]
        public DateTime? EnrollmentDateUntil { get; set; } // ? (NULLABLE)

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(LastName) && String.IsNullOrEmpty(FirstMidName))
            {
                yield return new ValidationResult("Masukkan minimal satu kolom pencarian!");
                //Memberi validation jika sebuah kolom pencarian tidak diisi!
            }

            if (EnrollmentDateFrom == null)
            {
                yield return new ValidationResult("Masukkan Kolom Tanggal Dari ", new[] { "EnrollmentDateFrom" });
                //Memberi validation ke variable yang dituju!
            }
            if (EnrollmentDateUntil == null)
            {
                yield return new ValidationResult("Masukkan Kolom Tanggal Sampai", new[] { "EnrollmentDateUntil" });
                //Memberi validation ke variable yang dituju!
            }
        }
    }
}