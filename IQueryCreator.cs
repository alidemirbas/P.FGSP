
namespace P.FGSP
{
    public interface IQueryCreator
    {
        IQuery Create<T>(System.Collections.Specialized.NameValueCollection form);
    }
}
