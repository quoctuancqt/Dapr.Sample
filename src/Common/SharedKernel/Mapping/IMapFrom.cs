using AutoMapper;

namespace SharedKernel.Mapping
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }
}
