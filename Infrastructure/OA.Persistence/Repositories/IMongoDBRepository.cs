namespace OA.Persistence.Repositories
{
    public interface IMongoDBRepository<T>
    {
        Task InsertAsync(string document);
    }
}
