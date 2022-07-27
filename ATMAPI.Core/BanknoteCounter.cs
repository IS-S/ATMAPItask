namespace ATMAPI.Core
{
    public class BanknoteCounter : IBanknoteCounter
    {
        private readonly ICassetteManager _cassettemanager;

        public BanknoteCounter(ICassetteManager cassettemanager)
        {
            _cassettemanager = cassettemanager;
        }

        public List<PayloadModel> countSingleDenominations(List<PayloadModel> payload)
        {
            List<PayloadModel>? filteredPayload = new();

            try
            {
                filteredPayload = _cassettemanager.ChooseWorkingProperly(payload);
            }
            catch(Exception ex)
            {
                if (ex.Message == "No working cassettes")
                {
                    throw;
                }
            }

            List<PayloadModel> finalPayload = new();

            bool found = false;

            foreach(var cassette in filteredPayload)
            {
                found = false;
                foreach(var filteredCassete in finalPayload)
                {
                    if(cassette.denomination == filteredCassete.denomination)
                    {
                        found = true;
                    }
                    if (found)
                    {
                        filteredCassete.banknotesLeft += cassette.banknotesLeft;
                    }
                }
                if (!found)
                {
                    finalPayload.Add(cassette);
                }
            }
            finalPayload.Sort(delegate(PayloadModel x, PayloadModel y)
            {
                return y.denomination.CompareTo(x.denomination);
            });

            if(finalPayload.Count == 1 && filteredPayload[0].denomination == 0)
            {
                throw new Exception("All working cassetes has '0'(ZERO) denomination. Please check!");
            }

            return finalPayload;

        }
    }
}
