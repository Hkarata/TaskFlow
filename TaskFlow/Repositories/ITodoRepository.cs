using TaskFlow.Models;

namespace TaskFlow.Repositories
{
    public interface ITodoRepository
    {
        Task CreateOrUpdateToDo(TodoItem toDo);
        Task DeleteTodo(Guid todoId);
        Task<TodoItem?> GetToDo(Guid todoId);
        Task UpdateToDoFlag(Guid todoId, ItemFlag flag);
        Task UpdateToDoPriority(Guid todoId, ItemPriority priority);
    }
}
