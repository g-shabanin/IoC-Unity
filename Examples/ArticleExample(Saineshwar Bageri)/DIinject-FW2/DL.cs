namespace DIinject_FW2
{
    using System;

    public class DL : IProduct
    {
        public string Insertdata()
        {
            string val = "Dependency injection injected";

            Console.WriteLine(val);

            return val;
        }
    }
}
