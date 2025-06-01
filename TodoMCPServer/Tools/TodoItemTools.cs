using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using TodoMCPServer.Dtos;

namespace TodoMCPServer.Tools
{
    [McpServerToolType]
    public static class TodoItemTools
    {

        [McpServerTool, Description("Crear un ítem en la lista 'Nombre de lista' con la descripción ‘Descripcion de item'")]
        public static async Task<string> PostTodoItem(
            HttpClient client,
            [Description("Nombre de la lista")] string name,
            [Description("Descripcion del item")] string description)
        {

            var id = await findTodoListID(client, name);
            if (id == null)
            {
                return "No existe la lista.";
            }

            var dto = new CreateTodoItem { Description = description, Completed = false };
            var response = await client.PostAsJsonAsync($"/api/todolists/{id}/todos", dto);

            return "Se ha creado el item correctamente.";
        }

        [McpServerTool, Description("Devolver los ítems de la lista 'Nombre de lista'.")]
        public static async Task<string> GetTodoItems(
            HttpClient client,
            [Description("Nombre de la lista")] string name)
        {

            var id = await findTodoListID(client, name);
            if (id == null)
            {
                return "No existe la lista.";
            }

            var jsonElement = await client.GetFromJsonAsync<JsonElement>($"/api/todolists/{id}/todos");

            var todoItems = jsonElement.EnumerateArray();

            if (!todoItems.Any())
            {
                return "No existe el item.";
            }

            string ret = "";

            foreach (var item in todoItems)
            {
                ret = ret + item.ToString() + "\n";
            }

            return ret;

        }

        [McpServerTool, Description("Completar un ítem en la lista 'Nombre de lista' con la descripción ‘descripcion de item’, o marcar como finalizado.")]
        public static async Task<string> CompleteTodoItem(
            HttpClient client,
            [Description("Nombre de lista")] string name,
            [Description("Descripcion de item")] string description)
        {

            var listId = await findTodoListID(client, name);
            if (listId == null)
            {
                return "No existe la lista.";
            }

            var itemId = await findTodoItemID(client, (long) listId, description);
            if (itemId == null)
            {
                return "No existe el item.";
            }

            var dto = new UpdateTodoItem { Description = null, Completed = true };

            var jsonElement = await client.PutAsJsonAsync($"/api/todolists/{listId}/todos/{itemId}", dto);

            return "Se ha actualizado correrctamente el item.";

        }

        [McpServerTool, Description("Cambiar descripcion del item de la lista 'Nombre lista' con descripcion 'descripcion original' por 'nueva descripcion")]
        public static async Task<string> UpdateDescriptionTodoItem(
            HttpClient client,
            [Description("Nombre de lista")] string name,
            [Description("Descripcion original")] string description,
            [Description("Descripcion nueva")] string newDescription)
        {

            var listId = await findTodoListID(client, name);
            if (listId == null)
            {
                return "No existe la lista.";
            }

            var itemId = await findTodoItemID(client, (long) listId, description);
            if (itemId == null)
            {
                return "No existe el item.";
            }

            var dto = new UpdateTodoItem { Description = newDescription };

            var jsonElement = await client.PutAsJsonAsync($"/api/todolists/{listId}/todos/{itemId}", dto);

            return "Se ha actualizado correrctamente el item.";

        }

        [McpServerTool, Description("Eliminar ítem en la lista 'nombre de lista' con la descripción ‘descripcion de item’")]
        public static async Task<string> DeleteTodoItem(
            HttpClient client,
            [Description("Nombre de lista")] string name,
            [Description("Descripcion de item")] string description)
        {

            var listId = await findTodoListID(client, name);
            if (listId == null)
            {
                return "No existe la lista.";
            }

            var itemId = await findTodoItemID(client, (long) listId, description);
            if (itemId == null)
            {
                return "No existe el item.";
            }

            var jsonElement = await client.DeleteAsync($"/api/todolists/{listId}/todos/{itemId}");

            return "Se ha eliminado correrctamente el item.";

        }
        public static async Task<long?> findTodoListID(HttpClient client, string name)
        {
            long? id = null;

            var jsonElement = await client.GetFromJsonAsync<JsonElement>("/api/todolists");
            var todolists = jsonElement.EnumerateArray();

            if (!todolists.Any())
            {
                return id;
            }

            foreach (var list in todolists)
            {
                if (name == list.GetProperty("name").GetString())
                {
                    id = list.GetProperty("id").GetInt64();
                    break;
                }
            }
            return id;
        }
    
    public static async Task<long?> findTodoItemID(HttpClient client, long listId, string description)
    {
        long? itemId = null;

        var jsonElement = await client.GetFromJsonAsync<JsonElement>($"/api/todolists/{listId}/todos");
        var todoItems = jsonElement.EnumerateArray();

        if (!todoItems.Any())
        {
            return itemId;
        }

        foreach (var item in todoItems)
        {
            if (description == item.GetProperty("description").GetString())
            {
                itemId = item.GetProperty("id").GetInt64();
                break;
            }
        }

        return itemId;
    }

    }
    

}


