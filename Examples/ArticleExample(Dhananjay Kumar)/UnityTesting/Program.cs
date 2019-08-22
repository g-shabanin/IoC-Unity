using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration; 

namespace UnityTesting
{
    
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
