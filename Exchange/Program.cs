using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Exchange
{
    class Program
    {
        private const int REGISTER = 1;
        private const int BUY_SHARES_REQUEST_A_QUOTE = 2;
        private const int REQUEST_INVENTORY_REPORT = 3;
        private const int REQUEST_FINANCIAL_REPORT = 4;
        private const int REQUEST_TRANSACTION_LOG = 5;
        private const int REQUEST_BANK_INVOICE = 6;
        private const int EXIT = 7;

        private static BankUI bkUI;
        private static RegulatorUI reUI;

        public static void Main(string[] args)
        {
            // initialise test data
            InitialiseData();

            // display menu
            DisplayMenu();

            // get menu choice from user
            int choice = GetMenuChoice();


            // loop: (choice not EXIT)
            while (choice != EXIT)
            {
                //   do user choice
                switch (choice)
                {
                    case REGISTER:
                        Register();
                        break;
                    case BUY_SHARES_REQUEST_A_QUOTE:
                        BuyShares_RequestAQuote();
                        break;
                    case REQUEST_INVENTORY_REPORT:
                        RequestInventoryReport();
                        break;
                    case REQUEST_FINANCIAL_REPORT:
                        RequestFinancialReport();
                        break;
                    case REQUEST_TRANSACTION_LOG:
                        RequestTransactionLog();
                        break;
                    case REQUEST_BANK_INVOICE:
                        RequestBankInvoice();
                        break;
                        //    default:
                        // error
                        //       break;
                }

                //   display menu
                DisplayMenu();
                //   get menu choice from user
                choice = GetMenuChoice();

            }
        }

        private static void InitialiseData()
        {

            BankManager bankM = new BankManager();
            ShareManager shareM = new ShareManager();
            RecordManager recordM = new RecordManager();

            bkUI = new BankUI(bankM, shareM, recordM);
            reUI = new RegulatorUI(bankM, shareM, recordM);

            //reset Database for testing
            File.WriteAllText("Banks.txt", "");
            File.WriteAllText("Shares.txt", "");
            File.WriteAllText("Records.txt", "");

            //bank for initial test
            bankM.AddBank("A", "Banks.txt");

            //shares for initial test
            shareM.AddShare(new Share("FLC", 13.45, false), "Shares.txt");
            shareM.AddShare(new Share("FLC", 13.45, false), "Shares.txt");
            shareM.AddShare(new Share("FLC", 14.45, false), "Shares.txt");
            shareM.AddShare(new Share("FLC", 15.00, false), "Shares.txt");
            shareM.AddShare(new Share("VIC", 14.45), "Shares.txt");
            shareM.AddShare(new Share("VIC", 14.45), "Shares.txt");
            shareM.AddShare(new Share("VIC", 14.45), "Shares.txt");
            shareM.AddShare(new Share("VIC", 15.00), "Shares.txt");
            shareM.AddShare(new Share("VIC", 15.30), "Shares.txt");
            shareM.AddShare(new Share("VIC", 15.30), "Shares.txt");
            shareM.AddShare(new Share("VIC", 15.75), "Shares.txt");
            shareM.AddShare(new Share("VIC", 15.75), "Shares.txt");
            shareM.AddShare(new Share("VNM", 15.30), "Shares.txt");
            shareM.AddShare(new Share("VNM", 15.56), "Shares.txt");
            shareM.AddShare(new Share("VNM", 16.00), "Shares.txt");
            shareM.AddShare(new Share("VNM", 16.75), "Shares.txt");

        }


        private static void DisplayMenu()
        {
            Console.WriteLine("\n--------------------------------------------------------------------------------");
            Console.WriteLine("If you haven't got an account, please enter 1 to register for an exchange account.");
            Console.WriteLine("\n{0}. {1}", REGISTER, "Register for an exchange account");
            Console.WriteLine("{0}. {1}", BUY_SHARES_REQUEST_A_QUOTE, "Buy shares or request for a quote");
            Console.WriteLine("{0}. {1}", REQUEST_INVENTORY_REPORT, "Request an inventory report");
            Console.WriteLine("{0}. {1}", REQUEST_FINANCIAL_REPORT, "Request a financial report");
            Console.WriteLine("{0}. {1}", REQUEST_TRANSACTION_LOG, "Request a transaction log");
            Console.WriteLine("{0}. {1}", REQUEST_BANK_INVOICE, "Request a bank's invoice");
            Console.WriteLine("{0}. {1}", EXIT, "Exit");
        }

        private static int GetMenuChoice()
        {
            int option = ReadInteger("\nOption");

            while (option < 1 || option > 7)
            {
                Console.WriteLine("\nChoice not recognised. Please try again");
                option = ReadInteger("\nOption");
            }

            return option;
        }

        private static int ReadInteger(string prompt)
        {
            try
            {
                Console.Write(prompt + ": > ");
                return Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static string ReadString(string prompt)
        {
            try
            {
                Console.Write(prompt + ": > ");
                return Console.ReadLine().Trim();
            }
            catch (Exception)
            {
                return "e";
            }
        }

        private static double ReadDouble(string prompt)
        {

            try
            {
                Console.Write(prompt + ": > ");
                return Math.Round(Convert.ToDouble(Console.ReadLine()), 2);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static void Register()
        {
            string bankName = ReadString("Enter bank name");

            try
            {
                int id = bkUI.Register(bankName,"Banks.txt");
                Console.WriteLine("Bank registered");
                Console.WriteLine($"Your ID is {id}");
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }

        }

        private static void BuyShares_RequestAQuote()
        {
            Console.WriteLine("If you want to request for a quote, please enter 0 as the volume of share to buy.\n");
            int bankID = ReadInteger("Enter bank ID");
            string shareCode = ReadString("Enter share code");
            int vol = ReadInteger("Enter the volume of share you want to buy");
            while(vol<0)
            {
                vol = ReadInteger("Enter the volume of share you want to buy");
            }
            if (vol > 0)
            {
                double price = ReadDouble("Enter price ($) per share");
                Console.WriteLine("\nNOTE");
                //Console.WriteLine("\nIf the quote volume is -1, it means that there are no shares with higher price than your request price.");
                Console.WriteLine("\nIf the quote price is -1, it means either that all shares of request code are sold or there are no available shares with higher price than your request price.\n");
                try
                {
                    List<Response> responses = bkUI.BuyShares(bankID, shareCode, vol, price, "Banks.txt", "Shares.txt", "Records.txt");
                    foreach (Response r in responses)
                    {
                        Console.WriteLine(r.toString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n" + e.Message);
                }
            }
            else
            {
                Console.WriteLine("\nNOTE");
                Console.WriteLine("\nIf the quote price is -1, it means that all shares of request code are sold.\n");
                try
                {
                    List<Response> responses = bkUI.RequestAQuote(bankID, shareCode,"Records.txt");
                    foreach (Response r in responses)
                    {
                        Console.WriteLine(r.toString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n" + e.Message);
                }
            }

        }

        private static void RequestInventoryReport()
        {
            try
            {
                Dictionary<Tuple<string, double>, Tuple<int,int>> shares = reUI.GetInventoryReport();
                Console.WriteLine("INVENTORY REPORT:\n");
                Console.WriteLine("\t{0,-20} {1} {2,20} {3,20} {4,20}",
                        "Share code",
                        "Price($)/Share",
                        "Total volume",
                        "Sold volume",
                        "Available volume");
                foreach (Tuple<string, double> s in shares.Keys)
                {
                    Console.WriteLine("\t{0,-20} {1:0.00} {2,18} {3,20} {4,16}",
                        s.Item1,
                        s.Item2,
                        shares[s].Item1,
                        shares[s].Item2,
                        shares[s].Item1 - shares[s].Item2);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        private static void RequestFinancialReport()
        {
            try
            {
                Dictionary<int, Bank> banks = reUI.GetFinancialReport();
                Console.WriteLine("FINANCIAL REPORT:\n");
                Console.WriteLine("\t{0, -20} {1, -20} {2}",
                        "Bank's ID",
                        "Bank's name",
                        "Amount of money owed to the exchange ($)");
                double total_money_owed = 0;
                foreach (int bankID in banks.Keys)
                {
                    Console.WriteLine("\t{0, -20} {1, -20} {2}",
                        bankID,
                        banks[bankID].name,
                        banks[bankID].getMoneyOwed());
                    total_money_owed += banks[bankID].getMoneyOwed();
                }
                double fee = total_money_owed * 5 / 100;
                Console.WriteLine("The total amount of processing fee is ${0}", Math.Round(fee, 2));
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        private static void RequestTransactionLog()
        {
            Console.WriteLine("TRANSACTION LOG:\n");
            List<Request> requests = reUI.GetTransactionLog();
            foreach (Request r in requests)
            {
                Console.WriteLine(r.toString());
                List<Response> responses = r.getResponses();
                foreach (Response response in responses)
                {
                    Console.WriteLine(response.toString());
                }
            }
        }

        private static void RequestBankInvoice()
        {
            int bankID = ReadInteger("Enter bank ID");
            Console.WriteLine($"BANK NO.{bankID}'S INVOICE:\n");
            try
            {
                List<Response> responses = reUI.GetBankInvoice(bankID);
                Console.WriteLine("\t{0, -40} {1, -20} {2, -20} {3}",
                   "Time",
                   "Share code",
                   "Price ($)/share",
                   "Volume");
                foreach (Response r in responses)
                {
                    Console.WriteLine("\t{0, -40} {1, -20} {2, -20} {3}",
                       r.getTime().ToString("yyyymmdd:HH.mm.ss.fffff"),
                       r.share.code,
                       r.share.price,
                       r.volume);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

    }
    
}
