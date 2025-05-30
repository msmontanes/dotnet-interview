namespace TodoApi.Models;

public class TodoList
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public List<TodoItem> ToDoItems { get; set; } = new List<TodoItem>();

}
