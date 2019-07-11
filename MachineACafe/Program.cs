using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MachineACafe
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePathDrinks = System.Configuration.ConfigurationSettings.AppSettings["filePathDrinks"];
            Machine machine = new Machine(filePathDrinks);
            machine.Start();
            machine.End();
        }





    }
}
