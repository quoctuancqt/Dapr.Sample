using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Intefaces
{
    public interface IBaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }

    public interface IBaseEntity : IBaseEntity<string> { }
}
