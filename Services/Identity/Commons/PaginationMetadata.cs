namespace Identity.Commons
{
    public class PaginationMetadata
    {
        /// <summary>
        /// Tổng số lượng mục
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPageCount { get; set; }

        /// <summary>
        /// Kích thước trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int CurrentPage { get; set; }

        public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
        {
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }
    }
}