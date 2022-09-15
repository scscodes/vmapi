namespace vmapi.Models
{
    public class Denomination
    {
        public int Hundreds { get; }
        public int Fifties { get; }
        public int Twenties { get; }
        public int Tens { get; }
        public int Fives { get; }
        public int Ones { get; }
        public int Quarters { get; }
        public int Dimes { get; }
        public int Nickles { get; }
        public int Pennies { get; }

        public Denomination(decimal price)
        {
            Hundreds = (int)(price / 100);
            price %= 100;
            Fifties = (int)(price / 50);
            price %= 50;
            Twenties = (int)(price / 20);
            price %= 20;
            Tens = (int)(price / 10);
            price %= 10;
            Fives = (int)(price / 5);
            price %= 5;
            Ones = (int)(price / 1);
            price %= 1;
            Quarters = (int)(price / .25m);
            price %= .25m;
            Dimes = (int)(price / .10m);
            price %= .10m;
            Nickles = (int)(price / .05m);
            price %= .05m;
            Pennies = (int)(price / .01m);
        }
    }
}
