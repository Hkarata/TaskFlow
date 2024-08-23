namespace TaskFlow.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public ItemPriority Priority { get; set; }
        public ItemFlag Flag { get; set; }

        // Navigation properties
        public Guid TodoListId { get; set; }
        public TodoList? TodoList { get; set; }
    }

    public enum ItemPriority
    {
        Low,
        Medium,
        High
    }

    public enum ItemFlag
    {
        Completed,
        Deleted,
        InProgress,
        Discarded,
        NotStarted
    }
}
