using System;
using System.Collections.Generic;

namespace Exchange
{
    public class Request:Record
    {
        public Bank bank { get; }
        private List<Response> responses = new List<Response>();

        public Request(int id, DateTime time, Share share, int vol, Bank bank): base(id, time, share, vol)
        {
            this.bank = bank;
        }

        public Bank getBank() => this.bank;
        public List<Response> getResponses() => this.responses;
        public void setResponse(Response r) => this.responses.Add(r);
        public override string toString()
        {
            string t = time.ToString("yyyymmdd:HH.mm.ss.fffff");
            string rel = $"{bank.getName()}@{t}: buy {this.volume} {share.getCode()} at ${share.getPrice()}";
            return rel; throw new NotImplementedException();
        }
    }
}
