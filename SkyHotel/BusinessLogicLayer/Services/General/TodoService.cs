using DataAccessLayer.Data;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.General
{
    public class TodoService : ITodoService
    {
        public TodoService(AppDbContext context)
        {
            _context = context;
        }

        public AppDbContext _context { get; set; }

        public async Task<bool> AddTodo(TodoItem todoItem)
        {
            await _context.TodoItems.AddAsync(todoItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteTodo(int todoId)
        {
            var todoItem = _context.TodoItems.FirstOrDefault(x => x.Id == todoId);
            if(todoItem is not null)
            {
                todoItem.IsCompleted = true;
                _context.TodoItems.Update(todoItem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<TodoItem>> GetTodos()
        {
            return await _context.TodoItems.ToListAsync();
        }
    }
}
