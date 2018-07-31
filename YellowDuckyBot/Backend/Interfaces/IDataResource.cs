using System.Collections.Generic;

namespace YellowDuckyBot.Backend.Interfaces
{
    /// <summary>
    /// Represents common interface for all data resources, which should be used as common base for all resource classes.
    /// </summary>
    /// <typeparam name="T">wanted entry class</typeparam>
    public interface IDataResource<T> where T : class
    {
        List<T> GetAll();

        List<T> GetSome(string filter);

        T Get(string field, string value);

        bool Add(T item);

        bool AddMany(List<T> items);

        int Count();
    }
}