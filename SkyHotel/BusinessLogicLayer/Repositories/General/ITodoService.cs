using DataAccessLayer.Model;

namespace BusinessLogicLayer.Repositories.General
{
    public interface ITodoService
    {
        Task<List<TodoItem>> GetTodos();
        Task<bool> CompleteTodo(int todoId);
        Task<bool> AddTodo(TodoItem todoItem);
    }
}
