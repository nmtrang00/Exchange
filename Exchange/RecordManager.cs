using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
namespace Exchange
{
    public class RecordManager
    {
        private Dictionary<int, Record> records = new Dictionary<int, Record>();
        private static int recordID = 0;

        public int CreateRequest(DateTime t, Share s, int vol, Bank b, string database)
        {
            records.Add(recordID, new Request(recordID,t,s,vol,b));
            //Add to Transaction Database
            //string fileName = "Transactions.txt";
            string jsonString = JsonSerializer.Serialize(new Request(recordID, t, s, vol, b));
            using (StreamWriter sw = File.AppendText(database))
            {
                sw.WriteLine(jsonString);
            };
            recordID++;
            return recordID-1;
        }

        public int CreateResponse(DateTime t, Share s, int vol, string type, string database)
        {
            records.Add(recordID, new Response(recordID, t, s, vol, type));
            //Add to Transaction Database
            //string fileName = "Transactions.txt";
            string jsonString = JsonSerializer.Serialize(new Response(recordID, t, s, vol, type));
            using (StreamWriter sw = File.AppendText(database))
            {
                sw.WriteLine(jsonString);
            };
            recordID++;
            return recordID-1;
        }

        public Response GetResponse(int resID)
        {
            try
            {
                Response response = (Response)records[resID];
                return response;
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    throw new KeyNotFoundException("ERROR: Response not found");
                }
                else 
                {
                    //(ex is InvalidCastException)
                    throw new InvalidCastException("ERROR: The given ID belongs to a request");
                }
            }
        }

        public void SetResponseToARequest(int reqID, int resID)
        {
            // InvalidCastException: Can't conver Response to request and vice versa
            try
            { 
                Request request = (Request)records[reqID];
                try
                {
                    Response response = (Response)records[resID];
                    request.setResponse(response);
                }
                catch (Exception ex)
                {
                    if (ex is KeyNotFoundException)
                    {
                        throw new KeyNotFoundException("ERROR: Response not found");
                    }
                    else
                    {
                        //(ex is InvalidCastException)
                        throw new InvalidCastException("ERROR: The given ID belongs to a request");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    throw new KeyNotFoundException("ERROR: Response not found");
                }
                else
                {
                    //(ex is InvalidCastException)
                    throw new InvalidCastException("ERROR: The given ID belongs to a request");
                }
            }
        }

        public List<Request> GetAllRequest()
        {
            List<Request> requests = new List<Request>();
            for (int i = 0; i < recordID; i++)
            {
                if (records[i] is Request)
                {
                    requests.Add((Request)records[i]);
                }
            }
            return requests;
        }

        public List<Response> GetGetBankSuccessfulTransactions(int bankID)
        {
            List<Response> found_responses = new List<Response>();
            for (int i = 0; i< recordID; i++ )
            {
                if (records[i] is Request)
                {
                    Request r = (Request)records[i];
                    if (r.bank.id == bankID && r.getResponses()[0].typeOfResponse == "bought")
                    {
                        found_responses.Add(r.getResponses()[0]);
                    }
                }
            }
            return found_responses;
        }

        public void resetRecordID()
        {
            //This is only used for unit testing the functions of the recored manager
            recordID = 0;
        }
    }
}
