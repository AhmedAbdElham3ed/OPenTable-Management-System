using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTable
{
    class log_in
    {
        public static bool logged_in = false;
       public static string User_Email;
        public static string password;
        public static string Location;
        public static string firstname;
        public static string lastname;
        public static Dictionary<string, int> occasions=new Dictionary<string, int>();
        public static Dictionary<string, int> available_time = new Dictionary<string, int>();
        public static Dictionary<string, int> cuisine = new Dictionary<string, int>();
        public static Dictionary<string, int> locations = new Dictionary<string, int>();
        public static void store()
        {
            occasions.Add("Birthday", 0);
            occasions.Add("Anniversary", 1);
            occasions.Add("Date night", 2);
            occasions.Add("Business Meal", 3);
            occasions.Add("Celebration", 4);

            available_time.Add("no specifications", 0);
            available_time.Add("01:00", 1);
            available_time.Add("01:30", 2);
            available_time.Add("02:00", 3);
            available_time.Add("02:30", 4);
            available_time.Add("03:00", 5);
            available_time.Add("03:30", 6);
            available_time.Add("04:00", 7);
            available_time.Add("04:30", 8);
            available_time.Add("05:00", 9);
            available_time.Add("05:30", 10);
            available_time.Add("06:00", 11);
            available_time.Add("06:30", 12);
            available_time.Add("07:00", 13);
            available_time.Add("07:30", 14);
            available_time.Add("08:00", 15);
            available_time.Add("08:30", 16);
            available_time.Add("09:00", 17);
            available_time.Add("09:30", 18);
            available_time.Add("10:00", 19);
            available_time.Add("10:30", 20);
            available_time.Add("11:00", 21);
            available_time.Add("11:30", 22);
            available_time.Add("12:00", 23);
            available_time.Add("12:30", 24);
            available_time.Add("13:00", 25);
            available_time.Add("13:30", 26);
            available_time.Add("14:00", 27);
            available_time.Add("14:30", 28);
            available_time.Add("15:00", 29);
            available_time.Add("15:30", 30);
            available_time.Add("16:00", 31);
            available_time.Add("16:30", 32);
            available_time.Add("17:00", 33);
            available_time.Add("17:30", 34);
            available_time.Add("18:00", 35);
            available_time.Add("18:30", 36);
            available_time.Add("19:00", 37);
            available_time.Add("19:30", 38);
            available_time.Add("20:00", 39);
            available_time.Add("20:30", 40);
            available_time.Add("21:00", 41);
            available_time.Add("21:30", 42);
            available_time.Add("22:00", 43);
            available_time.Add("22:30", 44);
            available_time.Add("23:00", 45);
            available_time.Add("23:30", 46);

            cuisine.Add("no specifications", 1);
            cuisine.Add("American", 1);
            cuisine.Add("Italian", 2);
            cuisine.Add("French", 3);
            cuisine.Add("Indian", 4);
            cuisine.Add("Japanese", 5);
            cuisine.Add("Chinese", 6);
            cuisine.Add("Grill", 7);
            cuisine.Add("Turkish", 8);
            cuisine.Add("Lebanese", 9);
            cuisine.Add("Egyptian", 10);
            cuisine.Add("Syrian", 11);
            cuisine.Add("Asian", 12);
            cuisine.Add("International",12);
            cuisine.Add("Middle Eastern", 13);

            locations.Add("no specifications", 1);
            locations.Add("Seoul", 1);
            locations.Add("Tokyo", 2);
            locations.Add("Istanbul", 3);
            locations.Add("Kuala Lumpur", 4);
            locations.Add("New York", 5);
            locations.Add("Dubai", 6);
            locations.Add("Paris", 7);
            locations.Add("London", 8);
            locations.Add("Bangkok", 9);
            locations.Add("Cairo", 10);
            

        }
    }
}
