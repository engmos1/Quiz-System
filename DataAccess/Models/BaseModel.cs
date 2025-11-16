namespace ExaminationSystem.Models
{
    public class BaseModel
    {
        public int ID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }


    }
}
