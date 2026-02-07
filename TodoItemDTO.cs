//Data Transfer Object.
//Handles client->model input.
//Gets to decide the properties the client is allowed to modify.
//Map endpoint delegates have the final say during Todo object interation.
public class TodoItemDTO
{
    public int Id { get; set; } //ignored. In fact I should delete it.
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    public TodoItemDTO(){}
    public TodoItemDTO(Todo todoItem)=>
    (Id, Name, IsComplete) = (todoItem.Id, todoItem.Name, todoItem.IsComplete);

}