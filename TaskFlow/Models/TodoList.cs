namespace TaskFlow.Models
{
    public class TodoList
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<TodoItem>? Items { get; set; }

        // Navigation properties
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
