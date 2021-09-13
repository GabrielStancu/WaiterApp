namespace Infrastructure.Helpers.Specification
{
    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }
}
