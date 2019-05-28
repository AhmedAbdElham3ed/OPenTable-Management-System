using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTable
{
    public class resturants_info : IComparable<resturants_info>
    {
        public string res_name;
        public string res_street;
        public string res_location;
        public string cusine;
        public int min_prize;
        public int max_prize;
        public int max_party_size;

        public int avg_rate;
        public resturants_info(string res_name, string res_street, string res_location, string cusine, int min_prize, int max_prize, int max_party_size)
        {
            this.res_name = res_name;
            this.res_street = res_street;
            this.res_location = res_location;
            this.cusine = cusine;
            this.min_prize = min_prize;
            this.max_prize = max_prize;
            this.max_party_size = max_party_size;

            this.avg_rate = 0;
        }
        public int CompareTo(resturants_info other)
        {
            // already implemented in integer class 
            // we also able to implement it here 
            return this.avg_rate.CompareTo(other.avg_rate);
        }
    }
}
