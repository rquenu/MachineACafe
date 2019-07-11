using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineACafe.Data
{
    class Drink
    {
        public Beverage beverage;
        public int sugarQuantity;
        public bool bUseMug;
        public int idBadge;
        public DateTime date;

        public Drink(Beverage beverage, int sugarQuantity, bool bUseMug, int idBadge)
        {
            this.beverage = beverage;
            this.sugarQuantity = sugarQuantity;
            this.bUseMug = bUseMug;
            this.idBadge = idBadge;
            this.date = DateTime.Now;
        }

        public override string ToString()
        {
            string WithMug;
            if (bUseMug)
                WithMug = "avec";
            else
                WithMug = "sans";
            return string.Format("{0} avec une quantité de sucre de {1} {2} mug.", beverage.name, sugarQuantity, WithMug);
        }
    }
}
