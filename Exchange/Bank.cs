using System;

namespace Exchange
{
    public class Bank
    {
        public int id { get; }
        public string name { get;  }
        public double moneyOwed { get; set; }

        public Bank(int id, string name, double moneyOwed)
        {
            this.id = id;
            this.name = name;
            this.moneyOwed = moneyOwed;
        }


        public Bank(int id, string name)
        {
            this.id = id;
            this.name = name;
            this.moneyOwed = 0;
        }

        public int getId() => this.id;
        public string getName() => this.name;
        public double getMoneyOwed() => this.moneyOwed;

        public void setMoneyOwed(double money)
        {
            this.moneyOwed += money;
        }
    }
}
