using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using TodoMCPServer.Dtos;

namespace TodoMCPServer.Tools
{
    [McpServerToolType]
    public static class TodoListTools
    {
        [McpServerTool, Description("Devolver todas las listas.")]
        public static async Task<string> GetTodoLists(HttpClient client)
        {
            var jsonElement = await client.GetFromJsonAsync<JsonElement>("/api/todolists");

            var todoLists = jsonElement.EnumerateArray();

            if (!todoLists.Any())
            {
                return "No hay listas.";
            }

            string ret = "";

            foreach (var list in todoLists)
            {
                ret = ret + list.GetProperty("name").ToString() + "\n";
            }

            return ret;
        }

        [McpServerTool, Description("Crear lista con nombre 'Nombre de la lista'.")]
        public static async Task<string> PostTodoList(
            HttpClient client,
            [Description("Nombre de la lista")] string name)
        {
            var dto = new CreateTodoList { Name = name };

            var response = await client.PostAsJsonAsync("/api/todolists", dto);
            var jsonElement = await response.Content.ReadFromJsonAsync<JsonElement>();

            return "Se ha creado la lista.";
        }

        [McpServerTool, Description("Cambiar nombre de lista 'Nombre de lista' por 'Nuevo nombre de lista'")]
        public static async Task<string> UpdateTodoList(
            HttpClient client,
            [Description("Nombre de lista")] string name,
            [Description("Nuevo nombre de lista")] string newName)
        {

            var id = await findTodoListID(client, name);
            if (id == null)
            {
                return "No existe la lista.";
            }

            var dto = new UpdateTodoList { Name = name };

            var jsonElement = await client.PutAsJsonAsync($"/api/todolists/{id}", dto);

            return "Se ha actualizado correrctamente el nomnre de la lista.";

        }

        [McpServerTool, Description("Eliminar lista 'Nombre de la lista'.")]
        public static async Task<string> DeleteTodoList(
            HttpClient client,
            [Description("Nombre de la lista")] string name)
        {

            var id = await findTodoListID(client, name);
            if (id == null)
            {
                return "No existe la lista.";
            }

            var response = await client.DeleteAsync($"/api/todolists/{id}");

            return "Se ha eliminado la lista correctamente.";
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

    }

        
}
