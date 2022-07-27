using Microsoft.AspNetCore.Mvc;
using ATMAPI.Core;

namespace ATMAPItask.Controllers
{
    [ApiController]
    public class Controller : ControllerBase
    {

        private readonly IAdvEvaluator _evaluateadvance;
        public Controller(IAdvEvaluator evaluateadvance)
        {
            _evaluateadvance = evaluateadvance;
        }
        [HttpPost]
        [Route("get_money")]
        public ActionResult Post(DataInModel Data)
        {

            if (!ModelState.IsValid || Data.amount == 0)
            {
                return BadRequest("Incorrect amount");
            }

            var result = _evaluateadvance.Evaluate(Data);

            return Ok(result);

        }
    }
}