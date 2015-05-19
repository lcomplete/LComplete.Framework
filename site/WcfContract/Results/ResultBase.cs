using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfContract.Results
{
    public class ResultBase
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}
