
namespace AMT.GenericRepository
{

    public class BaseModel<Tkey>
    {
        public Tkey Id { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? DeletedOnUtc { get; set; }
    }
}
