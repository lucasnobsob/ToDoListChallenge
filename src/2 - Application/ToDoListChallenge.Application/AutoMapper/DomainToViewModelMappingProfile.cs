using AutoMapper;
using ToDoListAPI.Domain.Entities;
using ToDoListChallenge.Application.ViewModels;

namespace ToDoListChallenge.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<TaskItem, TaskItemViewModel>().ReverseMap();

            CreateMap<TaskItem, CreateTaskItemViewModel>().ReverseMap();

            CreateMap<TaskItem, UpdateTaskItemViewModel>().ReverseMap();
        }
    }
}
