namespace Infrastructure.Business.Specification
{
    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }
}
