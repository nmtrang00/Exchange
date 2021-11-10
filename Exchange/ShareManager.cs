using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Exchange
{
    public class ShareManager
    {
        private List<Share> allShares = new List<Share>();

        public void AddShare(Share s, string database)
        {
            allShares.Add(s);
            //Add to Share Database
            string jsonString = JsonSerializer.Serialize(s);
            using (StreamWriter sw = File.AppendText(database))
            {
                sw.WriteLine(jsonString);
            };
        }

        public List<Share> GetAllShares() => this.allShares;

        public List<Share> FindShare(string shareCode)
        {
            //Get share with matching share code
            List<Share> found_shares = new List<Share>();
            foreach (Share s in this.allShares)
            {
                if (s.code == shareCode)
                {
                    found_shares.Add(s);
                }
            }
            return found_shares;
            
        }

        public double GetQuotePrice(List<Share> shares, double basePrice)
        {
            //Get quote price
            double price_dif = 1000;
            foreach (Share s in shares)
            {

                if (s.price - basePrice > 0 && s.price - basePrice < price_dif)
                {
                    price_dif = s.price - basePrice;
                }
            }
            if (basePrice + price_dif > 1000)
            {
                return -1;
            }
            else
            {
                return basePrice + price_dif;
            }
        }

        public List<Share> GetSharesToBuy(List<Share> shares, double price)
        {
            //Get all shares with price lower or equal to the request price
            List<Share> found_shares = new List<Share>();
            foreach (Share s in shares)
            {
                if (s.price <= price)
                {
                    found_shares.Add(s);
                }
            }
            return found_shares;
        }

        public List<Share> GetSharesAtAPrice(List<Share> shares, double price)
        {
            //Get all shares at the request price
            List<Share> found_shares = new List<Share>();
            foreach (Share s in shares)
            {
                if (s.price == price)
                {
                    found_shares.Add(s);
                }
            }
            return found_shares;
        }

        public List<Share> GetAvailableShares(List<Share> shares)
        {
            //Get all share that are available to buy
            List<Share> found_shares = new List<Share>();
            foreach (Share s in shares)
            {
                if (s.getState() == true)
                {
                    found_shares.Add(s);
                }
            }
            return found_shares;
        }

        public void updateShareDB(string database)
        {
            //Update shares state after a successful transaction 
            //string fileName = "Shares.txt";
            File.WriteAllText(database, "");
            foreach (Share s in this.allShares)
            {
                string jsonString = JsonSerializer.Serialize(s);
                using (StreamWriter sw = File.AppendText(database))
                {
                    sw.WriteLine(jsonString);
                };
            }
        }
    }
}
