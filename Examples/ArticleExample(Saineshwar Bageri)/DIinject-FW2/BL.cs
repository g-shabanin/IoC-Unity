namespace DIinject_FW2
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





