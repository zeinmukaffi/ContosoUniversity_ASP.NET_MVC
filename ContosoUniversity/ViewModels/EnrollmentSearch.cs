using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContosoUniversity.ViewModels
{
    public class EnrollmentSearch : IValidatableObject
    {
        [Display]
        public string Nama { get; set; }

        [Display]
        public string Title { get; set; }

        [Display]
        public string Nilai { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(Nama))
            {
                yield return new ValidationResult("Masukkan Nama Mahasiswa!");
                //Memberi validation jika sebuah kolom pencarian tidak diisi!
            }

            if (Title == null)
            {
                yield return new ValidationResult("Masukkan Nama Matkul!");
                //Memberi validation ke variable yang dituju!
            }
            if (Nilai == null)
            {
                yield return new ValidationResult("Masukkan Nilai!");
                //Memberi validation ke variable yang dituju!
            }
        }
    }
}