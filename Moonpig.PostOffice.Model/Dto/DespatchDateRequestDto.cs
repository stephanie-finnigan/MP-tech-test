namespace Moonpig.PostOffice.Model.Dto
{
    public class DespatchDateRequestDto
    {
        public List<int> ProductIds { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
