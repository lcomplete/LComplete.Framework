using System;
using System.Collections.Generic;

namespace LComplete.Framework.Http
{
    internal class RequestParameterComparer:IComparer<RequestParameter>
    {
        public int Compare(RequestParameter x, RequestParameter y)
        {
            return StringComparer.CurrentCulture.Compare(x.Name, y.Name);
        }
    }
}
