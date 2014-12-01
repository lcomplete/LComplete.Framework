namespace LComplete.Framework.Data
{
    public class PagingModel
    {
        public int PageSize { get; set; }

        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; }

        public int RecordCount { get; set; }

        public PagingModel(int pageIndex, int pageSize, int recordCount = 0)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            RecordCount = recordCount;
        }
    }
}
