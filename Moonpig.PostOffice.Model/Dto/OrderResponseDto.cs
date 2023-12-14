namespace Moonpig.PostOffice.Model.Dto
{
    using System;

    public class OrderResponseDto : ErrorResponseDto
    {
        public DateTime Date { get; set; }
    }
}