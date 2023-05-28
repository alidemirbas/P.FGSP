namespace P.FGSP
{
    public class Pager
    {
        public Pager(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;

            if (pageNumber < 1)
                pageNumber = 1;

            Skip = (pageNumber - 1) * pageSize;
            Take = pageSize;
        }

        public int PageNumber { get; }
        public int PageSize { get; }
        public int Skip { get; }
        public int Take { get; }
    }
}
