
namespace lab2
{

    public abstract class MyAbsClass : IExample
    {
        public int GetHash()
        {
            Random rnd = new Random();
            return rnd.Next();
        }

        public String Name = "";

        public abstract void PrintName();

        public virtual void ChangeName(String newName)
          {
              Name = Name + newName;
          }

    }

}