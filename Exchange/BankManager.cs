using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Exchange
{
    public class BankManager
    {
        private Dictionary<int, Bank> banks = new Dictionary<int, Bank>();
        private static int bankIDtoAssign = 0;

        public int AddBank(string bankName, string database)
        {
            banks.Add(bankIDtoAssign, new Bank(bankIDtoAssign, bankName));
            //Add to Bank Database
            //string fileName = "Banks.txt";
            string jsonString = JsonSerializer.Serialize(new Bank(bankIDtoAssign, bankName));
            using (StreamWriter sw = File.AppendText(database))
            {
                sw.WriteLine(jsonString);
            };
            bankIDtoAssign++;
            return bankIDtoAssign - 1;
        }

        public Bank FindBank(int bankId)
        {
            try
            {
                return banks[bankId];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("ERROR: Bank not found. Register to request for a quote or buy shares.");
            }
        }

        public Dictionary<int, Bank> GetAllBanks()
        {
            return banks;
        }

        public void SetMoneyOwed(int bankId, double money)
        {
            double old_value = banks[bankId].getMoneyOwed();
            banks[bankId].setMoneyOwed(old_value+money);
        }

        public void updateBankDB(string database)
        {
            //string fileName = "Banks.txt";
            File.WriteAllText(database, "");
            foreach (KeyValuePair<int, Bank> b in banks)
            {
                string jsonString = JsonSerializer.Serialize(b.Value);
                using (StreamWriter sw = File.AppendText(database))
                {
                    sw.WriteLine(jsonString);
                };
            }
        }

        public void resetIDtoAssign()
        {
            //this function is only used to support unit testing
            bankIDtoAssign = 0;
        }
    }
}
