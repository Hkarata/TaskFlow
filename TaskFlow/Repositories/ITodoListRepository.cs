using TaskFlow.Models;

namespace TaskFlow.Repositories
{
    public interface ITodoListRepository
    {
        Task CreateOrUpdateToDoList(string title, Guid userId);
        Task DeleteToDoList(Guid listId);
        Task<TodoList?> GetToDoList(Guid listId);
        Task<TodoList?> GetToDoListWithItems(Guid listId);
        Task<List<TodoList>?> GetToDoLists(Guid userId);
    }
}
