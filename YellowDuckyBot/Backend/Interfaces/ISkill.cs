namespace YellowDuckyBot.Backend.Interfaces
{
    public interface ISkill<T> where T : class 
    {
        bool Give(T obj);

        T Take(string filter);
    }
}