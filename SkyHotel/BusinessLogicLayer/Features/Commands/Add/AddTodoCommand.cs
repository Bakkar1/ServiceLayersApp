using DataAccessLayer.Model;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Features.Commands.Add
{
    public class AddTodoCommand : IRequest<bool>
    {
        [Required]
        public TodoItem? TodoItem { get; set; }
    }
}
