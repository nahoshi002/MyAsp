namespace SV20T1080031.Web.Models
{
    public class Person //Access Modifier: phạm vi truy cập của class
    {
        public int PersonId {  get; set; }
        public string? Name { get; set; }
        public string? LivePlace { get; set; }
        public string? Email { get; set; }
    }
}