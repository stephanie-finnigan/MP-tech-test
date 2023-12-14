namespace Moonpig.PostOffice.Model.Dto
{
    using System;

    public class DespatchDateResponseDto : ErrorResponseDto
    {
        public DateTime Date { get; set; }
    }
}