using System;
using System.Collections.Generic;

namespace Exchange
{
    public class BankUI
    {
        private BankManager bankM;
        private ShareManager shareM;
        private RecordManager recordM;

        public BankUI(BankManager bank, ShareManager share, RecordManager record)
        {
            this.bankM = bank;
            this.shareM = share;
            this.recordM = record;
        }

        public int Register(string bankName, string database)
        {
            return bankM.AddBank(bankName, database);
        }

        public List<Response> BuyShares(int bankID, string sharecode, int vol, double price, string bankDB, string shareDB, string recordDB)
        {
            List<Response> response = new List<Response>();
            Bank b = bankM.FindBank(bankID);
            if (b != null)
            {
                int reqID = recordM.CreateRequest(DateTime.Now, new Share(sharecode, price), vol, b, recordDB);
                List<Share> shares_matching_code = shareM.FindShare(sharecode);
                if (shares_matching_code.Count < 1)
                {
                    int resID = recordM.CreateResponse(DateTime.Now, new Share("Unknown share code", 0), 0, "quote", recordDB);
                    recordM.SetResponseToARequest(reqID, resID);
                    response.Add(recordM.GetResponse(resID));
                }
                else
                {
                    List<Share> shares_matched_available = shareM.GetAvailableShares(shares_matching_code);
                    if (shares_matched_available.Count < 1)
                    {
                        int resID = recordM.CreateResponse(DateTime.Now, new Share(sharecode, -1), 0, "quote", recordDB);
                        recordM.SetResponseToARequest(reqID, resID);
                        response.Add(recordM.GetResponse(resID));
                    }
                    else
                    {
                        List<Share> shares_all_matched = shareM.GetSharesToBuy(shares_matched_available, price);

                        int available_vol = shares_all_matched.Count;
                        if (vol <= available_vol)
                        {
                            double price_to_pay = vol * price;
                            b.setMoneyOwed(price_to_pay);
                            //Update bank database
                            bankM.updateBankDB(bankDB);
                            //Shares at lower price are sold first
                            for (int i = 0; i < vol; i++)
                            {
                                shares_all_matched[i].setState(false);
                            }
                            //Update share database
                            shareM.updateShareDB(shareDB);
                            int resID = recordM.CreateResponse(DateTime.Now, new Share(sharecode, price), vol, "bought", recordDB);
                            recordM.SetResponseToARequest(reqID, resID);
                            response.Add(recordM.GetResponse(resID));
                        }
                        else
                        {
                            int resID_only = recordM.CreateResponse(DateTime.Now, new Share(sharecode, price), available_vol, "only", recordDB);
                            recordM.SetResponseToARequest(reqID, resID_only);
                            response.Add(recordM.GetResponse(resID_only));

                            double newPrice = shareM.GetQuotePrice(shares_matched_available, price);
                            List<Share> shares_at_newPrice = shareM.GetSharesAtAPrice(shares_matched_available, newPrice);
                            int resID_quote = recordM.CreateResponse(DateTime.Now, new Share(sharecode, newPrice), shares_at_newPrice.Count, "quote", recordDB);
                            recordM.SetResponseToARequest(reqID, resID_quote);
                            response.Add(recordM.GetResponse(resID_quote));

                        }
                    }
                }
            }
            return response;
        }

        public List<Response> RequestAQuote(int bankID, string sharecode, string recordDB)
        {
            List<Response> response = new List<Response>();
            Bank b = bankM.FindBank(bankID);
            if (b != null)
            {
                int reqID = recordM.CreateRequest(DateTime.Now, new Share(sharecode, 0), 0, b, recordDB);
                List<Share> shares_matching_code = shareM.FindShare(sharecode);
                if (shares_matching_code.Count < 1)
                {
                    int resID = recordM.CreateResponse(DateTime.Now, new Share("Unknown share code", 0), 0, "quote", recordDB);
                    recordM.SetResponseToARequest(reqID, resID);
                    response.Add(recordM.GetResponse(resID));
                }
                else
                {
                    List<Share> shares_matched_available = shareM.GetAvailableShares(shares_matching_code);
                    if (shares_matched_available.Count < 1)
                    {
                        int resID = recordM.CreateResponse(DateTime.Now, new Share(sharecode, -1), 0, "quote", recordDB);
                        recordM.SetResponseToARequest(reqID, resID);
                        response.Add(recordM.GetResponse(resID));
                    }
                    else
                    {
                        double newPrice = shareM.GetQuotePrice(shares_matched_available, 0);
                        List<Share> shares_at_newPrice = shareM.GetSharesAtAPrice(shares_matched_available, newPrice);
                        int resID_quote = recordM.CreateResponse(DateTime.Now, shares_at_newPrice[0], shares_at_newPrice.Count, "quote", recordDB);
                        recordM.SetResponseToARequest(reqID, resID_quote);
                        response.Add(recordM.GetResponse(resID_quote));
                    }
                }
            }
            return response;
        }     

    }
}


