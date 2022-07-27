using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMAPI.Core
{
    public interface IAdvProcessor
    {
        public List<PayloadResult>? CalculateAdvance(DataInModel data);
    }
}
