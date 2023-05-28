namespace P.FGSP
{
    [Flags]
    public enum QueryBuilderIgnore
    {
        Filtering = 1,
        Grouping = 2,
        Sorting = 4,
        Paging = 8,
    }
}
