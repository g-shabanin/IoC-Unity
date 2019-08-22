namespace UnityTesting
{

   public  interface IPlayer
    {
       string PlayerName
       {
           get;
           set;
       }
       string TeamName
       {
           get;
           set;
       }

         void DisplayDetails();
    }
}
