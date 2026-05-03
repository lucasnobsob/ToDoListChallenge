using AutoMapper;
using ToDoListChallenge.Application.ViewModels;
using ToDoListChallenge.Domain.Commands;

namespace ToDoListChallenge.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<CreateTaskItemViewModel, RegisterNewTaskCommand>()
                .ConstructUsing(src => new RegisterNewTaskCommand(src.Title, src.Description, src.DueDate, (ToDoListAPI.Domain.Enums.TaskItemStatus)src.Status));

            CreateMap<UpdateTaskItemViewModel, UpdateTaskCommand>()
                .ConstructUsing(src => new UpdateTaskCommand(src.Id, src.Title, src.Description, src.DueDate, (ToDoListAPI.Domain.Enums.TaskItemStatus)src.Status));
        }
    }
}
