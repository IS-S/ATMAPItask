using System.Text.Json;

namespace ATMAPI.Core
{
    public class AdvProcessor : IAdvProcessor
    {
        private readonly IBanknoteCounter _banknotecounter;

        public AdvProcessor(IBanknoteCounter banknotecounter)
        {
            _banknotecounter = banknotecounter;
        }
        public List<PayloadResult>? CalculateAdvance(DataInModel data)
        {

            List<PayloadModel> banknotesReady = new();

            try
            {
                banknotesReady = _banknotecounter.countSingleDenominations(data.payload);
            }
            catch(Exception ex)
            {
                if (ex.Message == "All working cassetes have '0'(ZERO) denomination. Please check!"
                    || ex.Message == "No working cassettes")
                {
                    throw;
                }
            }

            List<int?> needCoins = new();
            double[] minBankn = new double[data.amount+1];

            List<PayloadResult> payloadRes = new();
            PayloadResult newItem = new();

            minBankn[0] = 0;
            int sum, banknote, oldBanknote = 0;
            bool found, banknoteChanged;

            for (sum = 1; sum <= data.amount; sum++)
            {
                minBankn[sum] = double.PositiveInfinity;

                for (banknote = 0; banknote < banknotesReady.Count(); banknote++)
                {
                    if(sum >= banknotesReady[banknote].denomination &&
                        minBankn[sum] > minBankn[sum - banknotesReady[banknote].denomination] + 1)
                    {
                        minBankn[sum] = minBankn[sum - banknotesReady[banknote].denomination] + 1;
                    }
                }
            }

            if (minBankn[data.amount] == double.PositiveInfinity)
            {
                return null;
            }

            

            sum = data.amount;
            while (sum > 0)
            {
                int cursSum = sum;
                for ( banknote = 0; banknote < banknotesReady.Count; banknote++)
                {

                    banknoteChanged = false;


                    if (banknote != oldBanknote && banknote!=0)
                    {
                        banknoteChanged = true;
                    }

                    oldBanknote = banknote;

                    bool banknoteExists = banknotesReady[banknote].banknotesLeft > 0;
                    if (banknoteExists && sum >= banknotesReady[banknote].denomination
                        && (minBankn[sum] == minBankn[sum - banknotesReady[banknote].denomination] + 1
                        || minBankn[sum] == minBankn[sum - banknotesReady[banknote].denomination] || banknoteChanged))
                    {
                        while(needCoins.Count() <= banknote)
                        {
                            needCoins.Add(null);
                        }
                        if (needCoins[banknote] == null)
                        {
                            needCoins[banknote] = 0;
                        }
                        needCoins[banknote]++;
                        sum -= banknotesReady[banknote].denomination;
                        banknotesReady[banknote].banknotesLeft -= 1;

                        found = false;
                        foreach (var item in payloadRes)
                        {
                            if (item.denomination == banknotesReady[banknote].denomination)
                            {
                                found = true;
                                item.banknotesNum++;
                            }
                        }
                        if (!found)
                        {
                            payloadRes.Add(JsonSerializer.Deserialize<PayloadResult>("{\"denomination\":" + banknotesReady[banknote].denomination + ",\"banknotesNum\":" + 1 + "}"));
                        }
                        break;
                    }
                }

                if (cursSum == sum)
                {
                    return null;
                }
            }

            return payloadRes;
        }
    }
}
