using System;
using MachineACafe.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace MachineACafe
{
    class Machine
    {
        private List<Drink> drinks = new List<Drink>();
        private string filePathDrink;

        public Machine(string pathDrink)
        {
            this.filePathDrink = pathDrink;
            using (StreamReader streamReader = new StreamReader(pathDrink,true))
            {
                JsonSerializer serializer = new JsonSerializer();
                this.drinks = (List<Drink>)serializer.Deserialize(streamReader,typeof(List<Drink>));
            }
            if(drinks == null)
            {
                drinks = new List<Drink>();
            }
            drinks.OrderBy(r => r.date);
        }

        public void Start()
        {
            Console.WriteLine("Avez vous un badge? (O/N puis appuyer sur Entrée)");
            string rep = Console.ReadLine().ToLowerInvariant();
            while (rep != "o" && rep != "n")
            {
                Console.WriteLine("Avez vous un badge? (O/N puis appuyer sur Entrée)");
            }
            if (rep == "o")
            {
                int numBadge = GetNumBadge();
                if (numBadge != 0)
                    GetNewCommande(numBadge);
            }
            else
            {
                GetNewCommande();
            }
        }

        static int GetNumBadge()
        {
            int numBadge = 0;
            Console.WriteLine("Quel est son numéro? ");
            numBadge = Convert.ToInt32(Console.ReadLine());
            return numBadge;
        }


        private void GetNewCommande(int numBadge = 0)
        {
            if  (!PreviousCommande(numBadge))
            {
                string beverageType = ChooseBeverage();
                int sugarLevel = ChooseSugarLevel();
                bool bWithMug = WithMug();
                PrepareCommande(beverageType, sugarLevel, bWithMug, numBadge);
            }
        }

        private void PrepareCommande(string beverageType, int sugarLevel, bool bWithMug, int numBadge)
        {
            Console.WriteLine("Commande en préparation");
            Drink drink = new Drink(new Beverage(beverageType), sugarLevel, bWithMug, numBadge);
            drinks.Add(drink);
            Console.WriteLine("Commande terminée");
        }

        private void PrepareCommande(Drink drink)
        {
            Console.WriteLine("Commande en préparation");
            drink.date = DateTime.Now;
            drinks.Add(drink);
            Console.WriteLine("Commande terminée");
        }

        private bool PreviousCommande(int numBadge)
        {
            bool bPreviousCommande = false;
            if (numBadge == 0)
                return false;
            Drink drink = GetLatestDrink(numBadge);
            if(drink !=null)
            {
                string commande = string.Format("Souhaitez-vous la commande suivante? {0} (O/N puis appuyer sur Entrée)", drink.ToString());
                Console.WriteLine(commande);
                string rep = Console.ReadLine().ToLowerInvariant();
                while (rep != "o" && rep != "n")
                {
                    Console.WriteLine("Format de réponse incorrect. Veuillez recommencer la saisie");
                    PreviousCommande(numBadge);
                }
                if (rep == "o")
                {
                    bPreviousCommande = true;
                    PrepareCommande(drink);
                }
            }
            return bPreviousCommande;
        }

        private Drink GetLatestDrink(int numBadge)
        {
            if (drinks == null || drinks.Count == 0 )
                return null;
            if (drinks.Where(r => r.idBadge == numBadge).Count() == 0)
                return null;
            return drinks.Where(r => r.idBadge == numBadge).Last();
        }
 

        static string ChooseBeverage()
        {
            string beverageType = string.Empty;
            Console.WriteLine("Choisissez une boisson parmi les suivantes: cafe,the et chocolat et appuyer sur Entrée");
            switch (Console.ReadLine().ToLowerInvariant())
            {
                case "cafe":
                    beverageType = "cafe";
                    break;
                case "the":
                    beverageType = "the";
                    break;
                case "chocolat":
                    beverageType = "chocolat";
                    break;
                default:
                    Console.WriteLine("Mauvais choix de boisson");
                    ChooseBeverage();
                    break;
            }

            return beverageType;
        }
        static int ChooseSugarLevel()
        {
            int sugarLevel = 0;
            try
            {
                Console.WriteLine("Choisissez un niveau de sucre de 0 à 5 puis appuyer sur Entrée");
                sugarLevel = Convert.ToInt32(Console.ReadLine());
                if (sugarLevel > 5)
                {
                    Console.WriteLine("Niveau de sucre non valide");
                    ChooseSugarLevel();
                }
            }
            catch
            {
                Console.WriteLine("Niveau de sucre non valide");
                ChooseSugarLevel();
            }
            return sugarLevel;
        }

        static bool WithMug()
        {

            bool bWithMug = false;
            try
            {
                Console.WriteLine("Avez vous un mug? (O/N puis appuyer sur Entrée)");
                string rep = Console.ReadLine().ToLowerInvariant();
                while (rep != "o" && rep != "n")
                {
                    Console.WriteLine("Format de réponse incorrect. Veuillez recommencer la saisie");
                    WithMug();
                }
                if (rep == "o")
                {
                    bWithMug = true;
                }
                else
                {
                    bWithMug = false;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return bWithMug;
        }

        public void End()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(filePathDrink))
            using (JsonWriter writer = new JsonTextWriter(sw))
            { serializer.Serialize(writer, this.drinks); };
        }


    }
}
