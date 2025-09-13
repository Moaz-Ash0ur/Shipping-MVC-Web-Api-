namespace BL.Contracts
{
    public  interface IBaseService<T,DTO>
    {
        IEnumerable<DTO> GetAll();
        DTO GetByID(Guid Id);
        bool Insert(DTO entity);
        bool Insert(DTO entity, out Guid Id);
        bool ChangeStatus(Guid ID, Guid userId, int status = 1);
        bool Update(DTO entity);
    }



}
