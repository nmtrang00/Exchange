using System;

namespace Exchange
{
    public class Share
    {
        public string code { get; }
        public double price { get; }
        public bool state { get; set; }
        //available = true, not available = false
      
        public Share(string code, double price, bool state)
        {
            this.code = code;
            this.price = price;
            this.state = state;
        }
    
        public Share(string code, double price)
        {
            this.code = code;
            this.price = price;
            this.state = true;
        }
        public string getCode() => this.code;
        public double getPrice() => this.price;
        public bool getState() => this.state;

        public void setState(bool b) => this.state=b;

    }
}
