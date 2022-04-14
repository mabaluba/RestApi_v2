using System;
using System.ComponentModel.DataAnnotations;

namespace M10_RestApi.ModelsDto
{
    public class LecturePostDto
    {
        public string Topic { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int TeacherId { get; set; }
    }
}