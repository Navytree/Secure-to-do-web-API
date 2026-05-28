namespace ToDoUserWebAPI.DTOs
{
    public class ShowDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Done { get; set; }
        public DateOnly DateTo {  get; set; }
    }
}
