namespace MySql.DataLayer.Core.Mapper
{
    using AgileObjects.AgileMapper.Extensions;
    using AgileObjects.AgileMapper;
    public class Map
    {
        /// <summary>
        /// It returns a DTO object from IDataEntity.
        /// </summary>
        /// <param name="dtoRecord"></param>
        /// <param name="entity"></param>
        /// <typeparam name="DTORecord"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns>DTO mapped</returns>
        public static DTORecord FromDataEntity<DTORecord, TEntity>(TEntity entity)
        where DTORecord : class
        where TEntity : IDataEntity
        {
           return entity.Map().ToANew<DTORecord>();
        }

        /// <summary>
        /// It returns a IDataEntity from DTO object.
        /// </summary>
        /// <param name="record"></param>
        /// <typeparam name="DTORecord"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns>IDataEntity mapped.</returns>
        public static TEntity ToEntity<DTORecord, TEntity>(DTORecord dtoRecord)
        where TEntity: IDataEntity
        where DTORecord : class
        {
            return dtoRecord.Map().ToANew<TEntity>();
        }
    }
}