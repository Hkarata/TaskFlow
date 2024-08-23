using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Repositories
{
    public class TodoListRepository(AppDbContext context) : ITodoListRepository
    {
        public async Task CreateOrUpdateToDoList(string title, Guid userId)
        {
            var todoList = await context.TodoLists
            .Where(tl => tl.Title == title)
            .FirstOrDefaultAsync();

            if (todoList is null)
                await CreateToDoList(title, userId);
            else
                todoList.Title = title;

            await context.SaveChangesAsync();
        }

        public async Task DeleteToDoList(Guid listId)
        {
            var list = await GetToDoList(listId);

            if (list is null)
                return;

            context.TodoLists.Remove(list);
            await context.SaveChangesAsync();
        }

        public async Task<TodoList?> GetToDoList(Guid listId)
        {
            var todoList = await context.TodoLists
                .FindAsync(listId);

            return todoList ?? null;
        }

        public async Task<List<TodoList>?> GetToDoLists(Guid userId)
        {
            var todoLists = await context.TodoLists
                .Where(tl => tl.UserId == userId)
                .ToListAsync();

            return todoLists;
        }

        public async Task<TodoList?> GetToDoListWithItems(Guid listId)
        {
            var lists = await context.TodoLists
                .AsNoTracking()
                .Include(tl => tl.Items)
                .Where(tl => tl.Id == listId)
                .FirstOrDefaultAsync();

            return lists ?? null;
        }

        private Task CreateToDoList(string title, Guid userId)
        {
            var todoList = new TodoList
            {
                Title = title,
                UserId = userId
            };

            context.TodoLists.Add(todoList);

            return Task.CompletedTask;
        }
    }
}
