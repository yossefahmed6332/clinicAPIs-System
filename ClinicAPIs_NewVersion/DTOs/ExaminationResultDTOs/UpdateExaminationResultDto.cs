using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.ExaminationResultDTOs
{
    public class UpdateExaminationResultDto
    {
        [Required, MaxLength(500)]
        public string TestType { get;  set; } = null!;
        [Required, MaxLength(50)]
        public string ResultValue { get;  set; }= null!;
        [Required, MaxLength(50)]
        public string   Unit { get;  set; } = null!;   
        [Required, MaxLength(100)]
        public string NormalRange { get;  set; }=null!;
        [Required, MaxLength(1000)]
        public string Note { get;  set; } = null!;




    }
}
