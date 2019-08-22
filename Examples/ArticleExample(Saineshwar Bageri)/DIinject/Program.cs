using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace DIinject
{
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
