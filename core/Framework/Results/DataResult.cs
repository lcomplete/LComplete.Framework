namespace LComplete.Framework.Results
{
    public class DataResult<T> : ResultBase where T:class 
    {
        public T Data { get; set; }
    }
}