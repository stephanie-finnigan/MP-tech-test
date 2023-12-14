namespace Moonpig.PostOffice.Model.Dto
{
    public class OrderRequestDto
    {
        public List<int> ProductIds { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
