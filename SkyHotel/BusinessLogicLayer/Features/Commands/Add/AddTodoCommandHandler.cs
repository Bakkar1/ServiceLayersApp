using BusinessLogicLayer.Repositories.General;
using MediatR;

namespace BusinessLogicLayer.Features.Commands.Add
{
    public class AddTodoCommandHandler : IRequestHandler<AddTodoCommand, bool>
    {
        private readonly ITodoService _todoService;

        public AddTodoCommandHandler(ITodoService todoService)
        {
            _todoService = todoService;
        }

        public async Task<bool> Handle(AddTodoCommand request, CancellationToken cancellationToken)
        {
            return await _todoService.AddTodo(request.TodoItem);
        }
    }
}
