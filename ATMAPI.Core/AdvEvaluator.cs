using System.Diagnostics;
using System.Text.Json;

namespace ATMAPI.Core
{
    public class AdvEvaluator : IAdvEvaluator
    {
        private readonly IAdvProcessor _advprocessor;

        public AdvEvaluator(IAdvProcessor advprocessor)
        {
            _advprocessor = advprocessor;
        }
        public Result Evaluate(DataInModel data)
        {


            Result result = new();

            Stopwatch sw = new Stopwatch();

            

            List<PayloadResult>? resultAdv = new();

            try
            {
                sw.Start();
                resultAdv = _advprocessor.CalculateAdvance(data);
                sw.Stop();
            }
            catch (Exception ex)
            {
                sw.Stop();
                result.result = ex.Message;
            }

            string elapsedTime = sw.ElapsedMilliseconds.ToString();

            if (resultAdv == null)
            {
                result.result = "Can't perform cash advance.";
            }
            else
            {
                result.result = "OK";
            }

            result.payload = resultAdv;
            result.timeElapsed = elapsedTime;
            

            return result;

        }
    }
}
