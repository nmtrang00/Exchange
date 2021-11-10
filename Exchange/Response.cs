using System;

namespace Exchange
{
    public class Response: Record
    {
        public string typeOfResponse { get; }

        public Response(int id, DateTime time, Share share, int vol, string type) : base(id, time, share, vol)
        {
            this.typeOfResponse = type;
        }

        public string getTypeOfResponse() => this.typeOfResponse;

        public override string toString()
        {
            string t = time.ToString("yyyymmdd:HH.mm.ss.fffff");
            string rel = $"EXCH@{t}: {this.typeOfResponse} {this.volume} {share.getCode()} at ${share.getPrice()}";
            return rel;
        }
    }
}
