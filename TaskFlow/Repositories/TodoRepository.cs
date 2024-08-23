using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Repositories
{
    public class TodoRepository(AppDbContext context) : ITodoRepository
    {
        public async Task CreateOrUpdateToDo(TodoItem toDo)
        {
            var todo = await GetToDo(toDo.Id);

            if (todo is null)
                await CreateToDo(toDo);

            else
            {
                todo.Description = toDo.Description;
                todo.Priority = toDo.Priority;
                todo.Flag = toDo.Flag;
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteTodo(Guid todoId)
        {
            var todo = await GetToDo(todoId);

            if (todo is null)
                return;

            context.TodoItems.Remove(todo);
            await context.SaveChangesAsync();
        }

        public async Task<TodoItem?> GetToDo(Guid todoId)
        {
            var todo = await context.TodoItems
                .FindAsync(todoId);

            return todo ?? null;
        }

        public async Task UpdateToDoFlag(Guid todoId, ItemFlag flag)
        {
            var todo = await GetToDo(todoId);

            if (todo is null)
            {
                return;
            }

            todo.Flag = flag;

            await context.SaveChangesAsync();
        }

        public async Task UpdateToDoPriority(Guid todoId, ItemPriority priority)
        {
            var todo = await GetToDo(todoId);

            if (todo is null)
            {
                return;
            }

            todo.Priority = priority;

            await context.SaveChangesAsync();
        }

        private async Task CreateToDo(TodoItem toDo) =>
            await context.TodoItems.AddAsync(toDo);
    }
}
