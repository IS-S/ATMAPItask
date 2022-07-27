using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMAPI.Core
{
    public interface IBanknoteCounter
    {
        public List<PayloadModel> countSingleDenominations(List<PayloadModel> payload);
    }
}
