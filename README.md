# dotnet-interview 

## Instrucciones

### 1. Compilar TodoApi y TodoMCPServer

`dotnet build`

### 2. Ejecutar TodoApi

`dotnet run --project TodoApi`

### 3. Configurar IA
Para probarlo en Claude Desktop se debe editar el archivo 'claude_desktop_config.json'.
Para más información, véase sección "Testing your server with Claude for Desktop" del siguiente [Link](https://modelcontextprotocol.io/quickstart/server#c).

Ejemplo configuración en Windows:
    ```json
        "mcpServers": {
            "todo": {
                "command": "dotnet",
                "args": [
                    "run",
                    "--project",
                    "C:\\ABSOLUTE\\PATH\\TO\\PROJECT",
                    "--no-build"
                ]
            }
        }
    ```
La ruta será algo como: C:\\...\\dotnet-interview\\TodoMCPServer

> [!IMPORTANT]
>- Para la base datos se utiliza devcontainers.
>- Si la API no corre en localhost:5083, se debe editar Program.cs en TodoMPCServer y cambiar la URL manualmente.

## Explicación

### TodoApi
Se agregan/modifican los siguientes archivos:
- En Models: Se crea TodoItem.cs con atributos necesarios para modelar un Item.
- En Controllers: Se agrega TodoItemsController.cs para manejar los nuevos Endpoints.
- En Dtos: Se agrega CreateTodoItem.cs y UpdateTodoList.cs.
- Data (TodoContext.cs) y Migrations: Se actualizan para soportar la persistencia de Items.

> [!NOTE]
> Se toma como referencia los casos de pruebas propuestos en (https://github.com/crunchloop/interview-tests) para determinar el comportamiento de los nuevos endpoints.

### TodoMPCServer

Se exponen los siguientes tools para listas (TodoList.cs) y items (TodoItemTools.cs):
- GetTodoLists: Devuelve todas las listas en el sistema.
- PostTodoList: Crea una lista. Requiere el nombre de la lista.
- UpdateTodoList: Cambiar nombre de una lista. Requiere nombre actual de la lista y nuevo nombre.
- DeleteTodoList: Elimina una lista. Requiere nombre de la lista.

- PostTodoItem: Crear un ítem. Requiere nombre de lista y descripción del item.
- GetTodoItems: Devuelve items de una lista. Requiere nombre de la lista.
- CompleteTodoItem: Marca como finalizado un item. Requiere nombre de lista y descripción del item.
- UpdateDescriptionTodoItem: Cambia descripción de un item. Requiere nombre de lista, descripción actual del item y nueva descripción.
- DeleteTodoItem: Elimina un item. Requiere nombre de lista y descripción de item.

## Pruebas
A modo de ejemplo, se pueden usar los siguientes prompts para probar el comportamiento de los nuevos endpoints:

1. Crear lista con nombre 'Trabajo'. 
2. Crear un ítem en la lista ‘Trabajo’ con la descripción ‘Terminar informe’
3. Devuelve todos los items de la lista 'Trabajo'.
4. Cambiar descripcion por 'Terminar presentacion'.
5. Marcar como finalizado 'Terminar presentacion'.
6. Devolver items de 'Trabajo'.

A su vez, se pueden hacer consultas que utilicen más de un tool. Algunos ejemplos:
1. Consultar estado de un item en cierta lista.
2. Listar los items de todas las listas.
3. Devolver items no completados (o completados) de cierta lista. 
4. Devolver items no completados (o completados) de todas las listas.

## Oportunidades de mejora
A continuación se listan posibles mejoras:
1. Evitar que se creen listas o ítems con el mismo nombre. 
2. Agregar restricciones más fuertes de seguridad ante errores que pueda reportar la API. 
3. Agregar 'timeout' para cuando la API no responde.
