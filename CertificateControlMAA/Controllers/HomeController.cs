using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertificateControlMAA.Models;

namespace CertificateControlMAA.Controllers
{
    
    public class HomeController : Controller
    {
        CertContext db_certs = new CertContext();
        ApplicationDbContext db_user = new ApplicationDbContext();

       [Authorize(Roles ="admin")]
        public ActionResult Index()
        {
            IEnumerable<department> department_list = db_certs.departments;
            ViewBag.departments = department_list;
            return View();
        }

        [HttpGet]
        public ActionResult AddDepartment() {
            return View();
        }
        [HttpPost]
        public ActionResult AddDepartment(department dep)
        {
            db_certs.departments.Add(dep);
            db_certs.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public ActionResult delete_department(int id) {
            
            var depart = db_certs.departments.First(de => de.id == id);
            db_certs.departments.Remove(depart);
            db_certs.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles ="admin, user")]
        public ActionResult view_department(int id)
        {
           
            IEnumerable<owner> owners = db_certs.owners.Where(o => o.departmentID == id);
            ViewBag.owners = owners;
            ViewBag.dep_name = db_certs.departments.First(o => o.id == id).name;
            ViewBag.dep_id = id;
            return View();
        }

        [HttpGet]
        public ActionResult AddOwner(int id)
        {
            ViewBag.dep_id = id;
            return View();
        }
        [HttpPost]
        public ActionResult AddOwner(owner owner)
        {
            db_certs.owners.Add(owner);
            db_certs.SaveChanges();
            return RedirectToAction(@"view_department/"+owner.departmentID, "Home");
        }
        [HttpGet]
        public ActionResult delete_owner(int id)
        {
            var owner = db_certs.owners.First(de => de.id == id);
            db_certs.owners.Remove(owner);
            db_certs.SaveChanges();
            return RedirectToAction(@"view_department/" + owner.departmentID, "Home");
        }

        [HttpGet]
        public ActionResult view_certs(int id)
        {
            IEnumerable<certificate> certs = db_certs.certificates.Where(x => x.ownerID == id);
            ViewBag.certs = certs;
            owner owner = db_certs.owners.First(o => o.id == id);
            department department = db_certs.departments.First(d => d.id == owner.departmentID);
            ViewBag.owner = owner;
            ViewBag.department = department;
            return View();
        }

        [HttpGet]
        public ActionResult add_certs(int id)
        {
            ViewBag.ownerID = id;
            ViewBag.ownerName = db_certs.owners.First(de => de.id == id).name;
            ViewBag.vendors = db_certs.vendors;
            ViewBag.cert_categories = db_certs.categories;
            return View();
        }

        [HttpPost]
        public ActionResult add_certs(certificate cert)
        {
            db_certs.certificates.Add(cert);
            db_certs.SaveChanges();
            return RedirectToAction(@"view_certs/"+cert.ownerID, "Home");
        }

        [HttpGet]
        public ActionResult delete_certs(int id)
        {
            var cert_del = db_certs.certificates.First(de => de.id == id);
            db_certs.certificates.Remove(cert_del);
            db_certs.SaveChanges();
            return RedirectToAction(@"view_certs/" + cert_del.ownerID, "Home");
        }

        [HttpGet]
        public ActionResult CertViewFull(int id)
        {
            certificate certificate = db_certs.certificates.First(c => c.id == id);
            owner owner = db_certs.owners.First(o => o.id == certificate.ownerID);
            vendor vendor = db_certs.vendors.First(v => v.id == certificate.vendorID);
            ViewBag.certificate = certificate;
            ViewBag.owner = owner;
            ViewBag.vendor = vendor;
            return View();
        }

        public ActionResult vendor_management()
        {
            IEnumerable<vendor> vendors = db_certs.vendors;
            ViewBag.vendors = vendors;
            return View();
        }

        [HttpGet]
        public ActionResult add_vendor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult add_vendor(vendor vendor)
        {
            db_certs.vendors.Add(vendor);
            db_certs.SaveChanges();
            return RedirectToAction("vendor_management", "Home");
        }
        [HttpGet]
        public ActionResult delete_vendor(int id)
        {
            var ven_del = db_certs.vendors.First(de => de.id == id);
            db_certs.vendors.Remove(ven_del);
            db_certs.SaveChanges();
            return RedirectToAction("vendor_management", "Home");
        }

        [HttpGet]
        public ActionResult edit_vendor(int id)
        {
            ViewBag.vendor = db_certs.vendors.First(v => v.id == id);
            return View();
        }

        [HttpPost]
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
        public ActionResult edit_department(int id)
        {
            ViewBag.department = db_certs.departments.First(d => d.id == id);
            return View();
        }

        [HttpPost]
        public ActionResult edit_department(department department)
        {
            var dep = db_certs.departments.First(d => d.id == department.id);
            dep.name = department.name;
            db_certs.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult edit_owner(int id)
        {
            ViewBag.owner = db_certs.owners.First(d => d.id == id);
            ViewBag.departments = db_certs.departments;
            ViewBag.current_dep = db_certs.departments.First(d => d.id == id);
            return View();
        }

        [HttpPost]
        public ActionResult edit_owner(owner owner)
        {
            var own = db_certs.owners.First(d => d.id == owner.id);
            own.name = owner.name;
            own.surname = owner.surname;
            own.secondName = owner.secondName;
            own.birthDate = owner.birthDate;
            own.position = owner.position;
            own.departmentID = owner.departmentID;
            db_certs.SaveChanges();
            return RedirectToAction(@"view_department/" + owner.departmentID, "Home");
        }

        [HttpGet]
        public ActionResult edit_cert(int id)
        {
            ViewBag.cert = db_certs.certificates.First(d => d.id == id);
            ViewBag.vendors = db_certs.vendors;
            ViewBag.current_ven = db_certs.vendors.First(v => v.id == id);
            ViewBag.categories = db_certs.categories;
            ViewBag.current_cat = db_certs.categories.First(c => c.id == id);
            return View();
        }

        [HttpPost]
        public ActionResult edit_cert(certificate certp)
        {
            var cert = db_certs.certificates.First(d => d.id == certp.id);
            cert.category = certp.category;
            cert.name = certp.name;
            cert.startDate = certp.startDate;
            cert.endDate = certp.endDate;
            cert.getDate = certp.getDate;
            cert.vendorID = certp.vendorID;
            db_certs.SaveChanges();
            return RedirectToAction(@"view_certs/" + cert.ownerID, "Home");
        }
        public ActionResult category_management()
        {
            IEnumerable<cert_category> categories = db_certs.categories;
            ViewBag.categories = categories;
            return View();
        }

        [HttpGet]
        public ActionResult add_category()
        {
            return View();
        }
        [HttpPost]
        public ActionResult add_category(cert_category cat)
        {
            db_certs.categories.Add(cat);
            db_certs.SaveChanges();
            return RedirectToAction("category_management", "Home");
        }

        [HttpGet]
        public ActionResult edit_category(int id)
        {
            ViewBag.category = db_certs.categories.First(d => d.id == id);
            return View();
        }

        [HttpPost]
        public ActionResult edit_category(cert_category category)
        {
            var cat = db_certs.categories.First(d => d.id == category.id);
            cat.name = category.name;
            db_certs.SaveChanges();
            return RedirectToAction("category_management", "Home");
        }

        [HttpGet]
        public ActionResult delete_category(int id)
        {

            var cat = db_certs.categories.First(de => de.id == id);
            db_certs.categories.Remove(cat);
            db_certs.SaveChanges();
            return RedirectToAction("category_management", "Home");
        }

    }
}