using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using DejtApplication10._0.Models;

namespace DejtApplication10._0.Controllers
{

    public class AnvändareController : Controller
    {
        private AnvändareDbContext db = new AnvändareDbContext();
        //Get Loginsida
        public ActionResult Login()
        {
            return View();
        }


        //Kontrollerar om Användarefinns
        [HttpPost]
        public ActionResult ValidateLogin(AnvändareModel model)
        {
            bool resultat = false;
            var ctx = new AnvändareDbContext();
            foreach (var användare in ctx.användare)
            {
                if (användare.AnvändarNamn.Equals(model.AnvändarNamn) && användare.Lössenord.Equals(model.Lössenord))
                {
                    resultat = true;
                }
            }
            if (resultat == true)
            {
                Console.WriteLine("testa1");
                return RedirectToAction("UserPage", model);

            }
            else
            {
                Console.WriteLine("testa2");
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult UserPage(AnvändareModel model)
        {

            var ctx = new AnvändareDbContext();
            var view = new AnvändareIndexViewModel();

            view.Användare = ctx.användare.ToList();

            var username = model.AnvändarNamn;
            var nyview = new AnvändareModel();

            foreach (var item in view.Användare)
            {
                if (item.AnvändarNamn.Equals(username))
                {
                    nyview.AnvändarNamn = item.AnvändarNamn;
                    nyview.Efternamn = item.Efternamn;
                    nyview.Födelsedatum = item.Födelsedatum;
                    nyview.Epost = item.Epost;
                    nyview.Förnamn = item.Förnamn;
                    nyview.IsActive = item.IsActive;
                    nyview.Lössenord = item.Lössenord;

                    return View(nyview);
                };

            }

            return View(model);

        }


        //Denna metod körs när man ska registrera sig den retunerar rätt GUI 
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //När man registrerat en användare så körs denn metod för att spara användaren och visa Att registreringen lyckades
        [HttpPost]
        public ActionResult addAnvändare(AnvändareModel model, HttpPostedFileBase File1)
        {
            var ctx = new AnvändareDbContext();

            if (File1 != null && File1.ContentLength > 0)
            {
                model.Profilbild = new byte[File1.ContentLength]; // Gör om filen man valt till En byte[]
                File1.InputStream.Read(model.Profilbild, 0, File1.ContentLength);
                ctx.användare.Add(model);
                ctx.SaveChanges();

                var nyaanvändarenMedID = new AnvändareModel();
                foreach (var användare in ctx.användare)
                {
                    if (användare.AnvändarNamn.Equals(model.AnvändarNamn))
                    {
                        nyaanvändarenMedID = användare;
                    }
                }
                TempData["NyAnvändare"] = nyaanvändarenMedID;

                return RedirectToAction("Bekräftelse", "Användare");
            }
            return RedirectToAction("Bekräftelse", "Användare");


        }

        //När man registrerat en användare så körs denn metod för att spara användaren och visa Att registreringen lyckades

        [HttpGet]
        public ActionResult Bekräftelse()
        {
            var view = new AnvändareModel();
            view = TempData["NyAnvändare"] as AnvändareModel;
            return View(view);
        }

        [HttpGet]
        public ActionResult EditUser(string username)
        {

            var view = new AnvändareIndexViewModel();
            view.Användare = db.användare.ToList();
            var users = view.Användare.Where(s => s.AnvändarNamn == username).FirstOrDefault();


            return View(users);
        }
        [HttpPost]
        public ActionResult EditUser(AnvändareModel model)
        {
            if (ModelState.IsValid)
            {

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserPage", model);

            }
            return View(model);
        }

        // Denna metod körs när man söker efter något på söksidan, samt sorterar sökningen.
        [HttpPost]
        public ActionResult Sök(string id)
        {
            string searchString = id;
            var ctx = new AnvändareDbContext();
            var view = new AnvändareIndexViewModel();


            if (!String.IsNullOrEmpty(searchString) && searchString != "" && searchString.Contains(" "))
            {
                string[] deladSök = searchString.Split(null);
                string förnamnet1 = deladSök[0];
                string förnamnet = förnamnet1.Substring(0, 1).ToUpper() + förnamnet1.Substring(1);
                string efternamnet1 = deladSök[1];
                string efternamnet = efternamnet1.Substring(0, 1).ToUpper() + efternamnet1.Substring(1);
                foreach (var användare in ctx.användare)
                {
                    if (användare.Förnamn.Contains(förnamnet) && användare.Efternamn.Contains(efternamnet))
                    {
                        view.Användare.Add(användare);
                    }
                }
                return View(view);
            }

            return RedirectToAction("Sök");
        }

        // Metoden körs när man går in på söksidan
        [HttpGet]
        public ActionResult Sök()
        {
            var ctx = new AnvändareDbContext();
            var veiw = new AnvändareIndexViewModel();
            veiw.Användare = ctx.användare.ToList();
            return View(veiw);
        }

    }
}