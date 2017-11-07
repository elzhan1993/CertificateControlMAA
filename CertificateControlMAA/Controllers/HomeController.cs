using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertificateControlMAA.Models;
using System.Web.Security;
using System.Threading;
using Microsoft.AspNet.Identity;
using System.IO;

namespace CertificateControlMAA.Controllers
{

    public class HomeController : Controller
    {
        CertContext db_certs = new CertContext();
        ApplicationDbContext db_user = new ApplicationDbContext();

        [Authorize(Roles = "admin, user")]
        public ActionResult Index()
        {

            var current_user = db_user.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (HttpContext.User.IsInRole("admin"))
            {
                IEnumerable<department> department_list = db_certs.departments;
                ViewBag.departments = department_list;
                return View();
            }
            else if (HttpContext.User.IsInRole("user"))
            {
                List<department> department_list = new List<department>();
                string[] deps = current_user.departmentID.Split(' ');

                foreach (string a in deps)
                {
                    if (a != "")
                    {
                        int b = Convert.ToInt16(a);
                        department des = db_certs.departments.FirstOrDefault(d => d.id == b);
                        department_list.Add(des);

                        ViewBag.departments = department_list;
                    }
                }
                //return RedirectToAction("view_department/"+current_user.departmentID, "Home");
                return View();
            }
            else {
                return RedirectToAction("Login", "Account");
            }
        }



        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult AddDepartment() {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AddDepartment(department dep)
        {
            db_certs.departments.Add(dep);
            db_certs.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult delete_department(int id) {

            var depart = db_certs.departments.First(de => de.id == id);
            db_certs.departments.Remove(depart);
            db_certs.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult view_department(int id)
        {
            var current_user = db_user.Users.First(u => u.UserName == User.Identity.Name);
            if (User.IsInRole("admin"))
            {
                IEnumerable<owner> owners = db_certs.owners.Where(o => o.departmentID == id);
                ViewBag.owners = owners;
                ViewBag.dep_name = db_certs.departments.First(o => o.id == id).name;
                ViewBag.dep_id = id;
                return View();
            }
            else if (current_user.departmentID.Contains(id.ToString()))
            {
                IEnumerable<owner> owners = db_certs.owners.Where(o => o.departmentID == id);
                ViewBag.owners = owners;
                ViewBag.dep_name = db_certs.departments.First(o => o.id == id).name;
                ViewBag.dep_id = id;
                return View();
            }
            else {
                return new HttpStatusCodeResult(403);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult AddOwner(int id)
        {
            ViewBag.dep_id = id;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public ActionResult AddOwner(owner owner)
        {
            db_certs.owners.Add(owner);
            db_certs.SaveChanges();
            return RedirectToAction(@"view_department/" + owner.departmentID, "Home");
        }
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult delete_owner(int id)
        {
            var owner = db_certs.owners.First(de => de.id == id);
            db_certs.owners.Remove(owner);
            db_certs.SaveChanges();
            return RedirectToAction(@"view_department/" + owner.departmentID, "Home");
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult view_certs(int id)
        {
            IEnumerable<certificate> certs = db_certs.certificates.Where(x => x.ownerID == id);
            ViewBag.certs = certs;
            owner owner = db_certs.owners.First(o => o.id == id);
            department department = db_certs.departments.First(d => d.id == owner.departmentID);
            ViewBag.owner = owner;
            ViewBag.sex = owner.sex ? "Male" : "Female";
            ViewBag.department = department;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult add_certs(int id)
        {
            ViewBag.ownerID = id;
            ViewBag.ownerName = db_certs.owners.First(de => de.id == id).name;
            ViewBag.vendors = db_certs.vendors;
            ViewBag.cert_categories = db_certs.categories;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public ActionResult add_certs(certificate cert, IEnumerable<HttpPostedFileBase> file)
        {
            string[] file_names = new string[4];

            foreach (var a in file)
            {
                int i = 0;
                if (a != null)
                {
                    string guid_file = Guid.NewGuid().ToString();
                    file_names[i] = guid_file + Path.GetExtension(a.FileName);
                    a.SaveAs(Server.MapPath("~/Files/" + guid_file + Path.GetExtension(a.FileName)));
                }
            }
            cert.file1_name = file_names[0];
            cert.file2_name = file_names[1];
            cert.file3_name = file_names[2];
            cert.file4_name = file_names[3];


            db_certs.certificates.Add(cert);
            db_certs.SaveChanges();
            return RedirectToAction(@"view_certs/" + cert.ownerID, "Home");
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult delete_certs(int id)
        {
            var cert_del = db_certs.certificates.First(de => de.id == id);
            db_certs.certificates.Remove(cert_del);
            db_certs.SaveChanges();
            return RedirectToAction(@"view_certs/" + cert_del.ownerID, "Home");
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult CertViewFull(int id)
        {
            certificate certificate = db_certs.certificates.First(c => c.id == id);
            owner owner = db_certs.owners.First(o => o.id == certificate.ownerID);
            vendor vendor = db_certs.vendors.First(v => v.id == certificate.vendorID);
            ViewBag.certificate = certificate;
            ViewBag.owner = owner;
            ViewBag.vendor = vendor;
            ViewBag.img1 = @"~/Files/" + certificate.file1_name;
            ViewBag.img2 = @"~/Files/" + certificate.file2_name;
            ViewBag.img3 = @"~/Files/" + certificate.file3_name;
            ViewBag.img4 = @"~/Files/" + certificate.file4_name;

            return View();
        }

        [Authorize(Roles = "admin, user")]
        public ActionResult vendor_management()
        {
            IEnumerable<vendor> vendors = db_certs.vendors;
            ViewBag.vendors = vendors;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult add_vendor()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public ActionResult add_vendor(vendor vendor)
        {
            db_certs.vendors.Add(vendor);
            db_certs.SaveChanges();
            return RedirectToAction("vendor_management", "Home");
        }
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult delete_vendor(int id)
        {
            var ven_del = db_certs.vendors.First(de => de.id == id);
            db_certs.vendors.Remove(ven_del);
            db_certs.SaveChanges();
            return RedirectToAction("vendor_management", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult edit_vendor(int id)
        {
            ViewBag.vendor = db_certs.vendors.First(v => v.id == id);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public ActionResult edit_vendor(vendor vendor)
        {
            var ven = db_certs.vendors.First(v => v.id == vendor.id);
            ven.name = vendor.name;
            ven.foundationDate = vendor.foundationDate;
            ven.address = vendor.address;
            db_certs.SaveChanges();
            return RedirectToAction("vendor_management", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult edit_department(int id)
        {
            ViewBag.department = db_certs.departments.First(d => d.id == id);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult edit_department(department department)
        {
            var dep = db_certs.departments.First(d => d.id == department.id);
            dep.name = department.name;
            db_certs.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult edit_owner(int id)
        {
            ViewBag.owner = db_certs.owners.First(d => d.id == id);
            ViewBag.departments = db_certs.departments;
            ViewBag.current_dep = db_certs.departments.First(d => d.id == id);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public ActionResult edit_owner(owner owner)
        {
            var own = db_certs.owners.First(d => d.id == owner.id);
            own.name = owner.name;
            own.surname = owner.surname;
            own.secondName = owner.secondName;
            own.birthDate = owner.birthDate;
            own.position = owner.position;
            own.departmentID = owner.departmentID;
            own.sex = owner.sex;
            own.email = owner.email;
            own.IIN = owner.IIN;
            own.phone_number = owner.phone_number;
            db_certs.SaveChanges();
            return RedirectToAction(@"view_department/" + owner.departmentID, "Home");
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult edit_cert(int id)
        {

            certificate cert = db_certs.certificates.First(d => d.id == id);
            ViewBag.cert = cert;
            ViewBag.vendors = db_certs.vendors;
            ViewBag.current_ven = db_certs.vendors.First(v => v.id == cert.vendorID);
            ViewBag.categories = db_certs.categories;
            ViewBag.current_cat = db_certs.categories.First(c => c.id == cert.category);
            ViewBag.img1 = @"~/Files/" + cert.file1_name;
            ViewBag.img2 = @"~/Files/" + cert.file2_name;
            ViewBag.img3 = @"`/Files/" + cert.file3_name;
            ViewBag.img4 = @"~/Files/" + cert.file4_name;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public ActionResult edit_cert(certificate certp, IEnumerable<HttpPostedFileBase> file)
        {
            string[] file_names = new string[4];

            foreach (var a in file)
            {
                int i = 0;
                if (a != null)
                {
                    string guid_file = Guid.NewGuid().ToString();
                    file_names[i] = guid_file + Path.GetExtension(a.FileName);
                    a.SaveAs(Server.MapPath("~/Files/" + guid_file + Path.GetExtension(a.FileName)));
                }
            }


            var cert = db_certs.certificates.First(d => d.id == certp.id);
            cert.category = certp.category;
            cert.name = certp.name;
            cert.startDate = certp.startDate;
            cert.endDate = certp.endDate;
            cert.getDate = certp.getDate;
            cert.vendorID = certp.vendorID;
            cert.file1_name = file_names[0];
            cert.file2_name = file_names[1];
            cert.file3_name = file_names[2];
            cert.file4_name = file_names[3];
            db_certs.SaveChanges();
            return RedirectToAction(@"view_certs/" + cert.ownerID, "Home");
        }
        [Authorize(Roles = "admin")]
        public ActionResult category_management()
        {
            IEnumerable<cert_category> categories = db_certs.categories;
            ViewBag.categories = categories;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult add_category()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult add_category(cert_category cat)
        {
            db_certs.categories.Add(cat);
            db_certs.SaveChanges();
            return RedirectToAction("category_management", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult edit_category(int id)
        {
            ViewBag.category = db_certs.categories.First(d => d.id == id);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult edit_category(cert_category category)
        {
            var cat = db_certs.categories.First(d => d.id == category.id);
            cat.name = category.name;
            db_certs.SaveChanges();
            return RedirectToAction("category_management", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult delete_category(int id)
        {

            var cat = db_certs.categories.First(de => de.id == id);
            db_certs.categories.Remove(cat);
            db_certs.SaveChanges();
            return RedirectToAction("category_management", "Home");
        }

        [HttpGet]
        public ActionResult delete_image(string @name, int por, int id)
        {
            certificate cer = db_certs.certificates.FirstOrDefault(c => c.id == id);
            string fn = Path.GetFileName(name);
            if (fn != null)
            {

                System.IO.File.Delete(Server.MapPath("~/Files/" + fn));
                switch (por) {
                    case 1: cer.file1_name = null; break;
                    case 2: cer.file2_name = null; break;
                    case 3: cer.file3_name = null; break;
                    case 4: cer.file4_name = null; break;
                }
                db_certs.SaveChanges();

            }

            return RedirectToAction(@"CertViewFull/" + id, "Home");

        }

        //User mangement methods************************************************************************************************************************************************************

        public ActionResult user_list()
        {
            ViewBag.users_list = db_user.Users.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult edit_user(string id)
        {
            ViewBag.user = db_user.Users.FirstOrDefault(u => u.Id == id);
            return View();
        }

        [HttpPost]
        public ActionResult edit_user(ApplicationUser usr)
        {

            return View();
        }


        //end of user management*****************************************************************************************************************************************************************************



    }
}