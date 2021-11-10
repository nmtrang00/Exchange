using System;
using System.Collections.Generic;

namespace Exchange
{
    public class RegulatorUI
    {
        private BankManager bankM;
        private ShareManager shareM;
        private RecordManager recordM;

        public RegulatorUI(BankManager bank, ShareManager share, RecordManager record)
        {
            this.bankM = bank;
            this.shareM = share;
            this.recordM = record;
        }

        public Dictionary<Tuple<string, double>, Tuple<int, int>> GetInventoryReport()
        {
            Dictionary<Tuple<string, double>, Tuple<int,int>> share_dict = new Dictionary<Tuple<string, double>, Tuple<int,int>>();
            List<Share> shares = shareM.GetAllShares();
            foreach (Share s in shares)
            {
                List<Tuple<string, double>> share_key = new List<Tuple<string,double>>(share_dict.Keys);
                if (share_key.Contains(new Tuple<string, double>(s.code,s.price)))
                {
                    int total = share_dict[new Tuple<string, double>(s.code, s.price)].Item1;
                    int sold = share_dict[new Tuple<string, double>(s.code, s.price)].Item2;
                    total++;
                    if (s.getState()==false)
                    {
                        sold++;
                        share_dict[new Tuple<string, double>(s.code, s.price)] = new Tuple<int, int>(total, sold);
                    }
                    else
                    {
                        share_dict[new Tuple<string, double>(s.code, s.price)] = new Tuple<int, int>(total, sold);
                    }
                    
                }
                else
                {
                    if (s.getState() == false)
                    {
                        share_dict[new Tuple<string, double>(s.code, s.price)] = new Tuple<int, int>(1, 1);
                    }
                    else
                    {
                        share_dict[new Tuple<string, double>(s.code, s.price)] = new Tuple<int, int>(1, 0);
                    }
                }
            }
            return share_dict;
        }

        public Dictionary<int,Bank> GetFinancialReport()
        {
            return bankM.GetAllBanks();
        }

        public List<Request> GetTransactionLog()
        {
            return recordM.GetAllRequest();
        }

        public List<Response> GetBankInvoice(int bankID)
        {
            return recordM.GetGetBankSuccessfulTransactions(bankID);
        }
    }
}
