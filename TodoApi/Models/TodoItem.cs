using System.Text.Json.Serialization;

namespace TodoApi.Models;

public class TodoItem
{
    public long Id { get; set; }

    public required string Description { get; set; }

    public Boolean Completed { get; set; }

    public long TodoListId { get; set; }
    
    [JsonIgnore]
    public TodoList TodoList { get; set; } = null!;

}
