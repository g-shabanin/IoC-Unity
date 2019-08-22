namespace UnityTesting
{
    using System;


    public class Player : IPlayer
    {
        public Player()
        {
        }

        string name;
        string teamName;

        public string PlayerName
        {
            get
            {
                return name;

            }
            set
            {
                name = value;
            }

        }
        public string TeamName
        {
            get
            {
                return teamName;
            }
            set
            {
                teamName = value;
            }
        }
        public void DisplayDetails()
        {
            Console.WriteLine("Player Name = \t" + name + " \tPlayer Team Name = \t" + teamName);
            //Console.WriteLine(p.PlayerName);
        }


    }
}
