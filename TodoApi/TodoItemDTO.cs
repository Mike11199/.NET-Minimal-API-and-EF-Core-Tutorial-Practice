namespace TodoApi
{
    public class TodoItemDTO
    {

        /*          
         
        Currently the sample app exposes the entire Todo object. 
        Production apps typically limit the data that's input and returned using a subset of the model. 
        There are multiple reasons behind this and security is a major one. 
        The subset of a model is usually referred to as a Data Transfer Object (DTO), input model, or view model. DTO is used in this article.

        A DTO may be used to:

            -Prevent over-posting.
            -Hide properties that clients are not supposed to view.
            -Omit some properties in order to reduce payload size.
            -Flatten object graphs that contain nested objects. Flattened object graphs can be more convenient for clients.
         */

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }

        public TodoItemDTO() { }
        public TodoItemDTO(Todo todoItem) =>
        (Id, Name, IsComplete) = (todoItem.Id, todoItem.Name, todoItem.IsComplete);
    }
}
