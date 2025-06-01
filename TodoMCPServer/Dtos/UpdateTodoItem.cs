namespace TodoMCPServer.Dtos;

public class UpdateTodoItem
{
    public required string? Description { get; set; }
    public Boolean? Completed { get; set; }
}
