namespace DIinject_FW2
{

    using Microsoft.Practices.Unity;
    using System;

    class Program
    {
        static void Main(string[] args)
        {

            /* Creating microsoft unity Container*/
            UnityContainer IU = new UnityContainer();

            /* Register a type*/
              IU.RegisterType<BL>();
              IU.RegisterType<DL>();
            /* Register a type with specific members to be injected. */

            IU.RegisterType<IProduct, DL>();

            BL objDL = IU.Resolve<BL>(); 
            objDL.Insert(); // BL Method
            Console.ReadLine();
        }
    }
}
