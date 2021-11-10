using System;

namespace Exchange
{
    public abstract class Record
    {
        public int id { get; }
        public DateTime time { get; }
        public Share share { get; }
        public int volume { get; }

        public Record(int id, DateTime time, Share share, int volume)
        {
            this.id = id;
            this.time = time;
            this.share = share;
            this.volume = volume;
        }

        public int getId() => this.id;
        public DateTime getTime() => this.time;
        public Share getShare() => this.share;
        public int getVolume() => this.volume;

        public abstract string toString();
    }
}
