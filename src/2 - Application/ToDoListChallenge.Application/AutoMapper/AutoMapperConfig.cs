namespace ToDoListChallenge.Application.AutoMapper
{
    public class AutoMapperConfig
    {
        public static Type[] RegisterMappings()
        {
            return
            [
                typeof(DomainToViewModelMappingProfile),
                typeof(ViewModelToDomainMappingProfile)
            ];
        }
    }
}
