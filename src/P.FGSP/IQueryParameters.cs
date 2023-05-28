namespace P.FGSP
{
    public interface IQueryParameters
    {
        Pager Pager { get; set; }
        Filter Filter { get; set; }
        List<Sorter> Sorters { get; set; }
        List<Grouper> Groupers { get; set; }
    }
}
