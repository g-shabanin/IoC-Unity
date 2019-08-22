namespace UnityTesting
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            IPlayer obj = Factory.CreateInstance();
            obj.PlayerName = " Sachin Tendulkar ";
            obj.TeamName = " India "; 
            obj.DisplayDetails();
            IPlayer obj1 = Factory.CreateInstance();
            obj.PlayerName = " Shane warne";
            obj.TeamName = " Australia ";
            obj.DisplayDetails();
            Console.Read();
            
        }
    }
}
