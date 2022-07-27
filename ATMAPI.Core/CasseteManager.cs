namespace ATMAPI.Core
{
    public class CasseteManager : ICassetteManager
    {
        public List<PayloadModel> ChooseWorkingProperly(List<PayloadModel> payload)
        {

            List<PayloadModel> payloadFiltered = new List<PayloadModel>();

            foreach (var cassete in payload)
            {
                if(cassete.worksProperly == true)
                {
                    payloadFiltered.Add(cassete);
                }
            }

            if (payloadFiltered.Count == 0)
            {
                throw new Exception("No working cassettes");
            }

            return payloadFiltered;
        }


    }
}