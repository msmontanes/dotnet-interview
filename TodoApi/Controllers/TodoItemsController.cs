using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/todolists/{todoListId}/todos")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/todolists/5/todos/1
        [HttpGet("{todoItemId}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long todoItemId)
        {
            var todoItem = await _context.TodoItem.FindAsync(todoItemId);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }

        // GET: api/todolists/5/todos
        [HttpGet]
        public async Task<ActionResult<IList<TodoItem>>> GetTodoItems(long todoListId)
        {
            var todoItems = await _context.TodoItem.Where(item => item.TodoListId == todoListId).ToListAsync();

            if (todoItems == null)
            {
                return NotFound();
            }

            return Ok(todoItems);
        }
        
        // POST: api/todolists/5/todos
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItems(long todoListId, [FromBody] CreateTodoItem payload) 
        {
        
            var todoList = await _context.TodoList.FindAsync(todoListId);
            
            if (todoList == null)
            {
                return NotFound(); 
            }
            
            var todoItem = new TodoItem
            {
                Description = payload.Description,
                Completed = payload.Completed,
                TodoListId = todoList.Id,
                TodoList = todoList
            };

            _context.Add(todoItem);

            todoList.ToDoItems.Add(todoItem);

            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetTodoItem), new { todoListId = todoList.Id, todoItemId = todoItem.Id }, todoItem);
        }

        // PUT: api/todolists/5/todos/1
        [HttpPut("{todoItemId}")]
        public async Task<ActionResult> PutTodoItem(long todoItemId, UpdateTodoItem payload)
        {
           var todoItem = await _context.TodoItem.FindAsync(todoItemId);

            if (todoItem == null)
            {
                return NotFound(); 
            }

            if (payload.Description != null)
            { 
                todoItem.Description = payload.Description;
            }

            if (payload.Completed.HasValue)
            {
                todoItem.Completed = (bool) payload.Completed;
            }
            
            await _context.SaveChangesAsync();

            return Ok(todoItem);
        }

        // DELETE: api/todolists/5/todos/1
        [HttpDelete("{todoItemId}")]
        public async Task<ActionResult> DeleteTodoItem(long todoListId, long todoItemId)
        {

            var todoItem = await _context.TodoItem.FindAsync(todoItemId);
        
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItem.Remove(todoItem);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
