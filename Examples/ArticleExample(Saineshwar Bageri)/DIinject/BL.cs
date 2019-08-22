using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIinject
{
    public class BL
    {
        private IProduct _objpro;

        public BL (IProduct objpro)
        {
            _objpro = objpro;    
        }

        public void Insert()
        {
            _objpro.Insertdata();
        }

    }
}





