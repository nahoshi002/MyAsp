namespace SV20T1080031.Web.Models
{
    public class PersonDAL
    {
        public List<Person> List() 
        { 
            List<Person> list = new List<Person>(); // tên biến camelCase: firstName
            list.Add(new Person()
            {
                PersonId = 1,
                Name = "ATEODINANG",
                LivePlace = "77 Nguyễn Huệ",
                Email = "nahoshi002@gmail.com",
            });
            list.Add(new Person()
            {
                PersonId = 2,
                Name = "ATEODIMUA",
                LivePlace = "16 Nguyễn Sinh Cung",
                Email = "nahoshina002@gmail.com",
            });
            list.Add(new Person()
            {
                PersonId = 3,
                Name = "ATEODIGIO",
                LivePlace = "76 Nguyễn Sinh Cung",
                Email = "akisbird@gmail.com",
            });
            return list;
        }
    }
}
