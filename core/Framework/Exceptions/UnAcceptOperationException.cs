using System;
using System.Runtime.Serialization;

namespace LComplete.Framework.Exceptions
{
    /// <summary>
    /// 不可用的操作异常
    /// </summary>
    [Serializable]
    public class UnAcceptOperationException : System.Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public int ErrorCode { get; private set; }

        public UnAcceptOperationException()
        {
        }

        public UnAcceptOperationException(int errorCode,string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public UnAcceptOperationException(int errorCode, string message, System.Exception inner)
            : base(message, inner)
        {
            ErrorCode = errorCode;
        }

        protected UnAcceptOperationException(
            int errorCode,
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            ErrorCode = errorCode;
        }
    }
}
