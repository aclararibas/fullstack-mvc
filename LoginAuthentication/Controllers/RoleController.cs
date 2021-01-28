using LoginAuthentication.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LoginAuthentication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        // Declair context of identity database
        private ApplicationDbContext context;

        public RoleController() // Constructor
        {
            // Instantiate context
            context = new ApplicationDbContext();
        }

        // GET: Roles/Index
        public ActionResult Index()
        {
            // Convert DB from a list to a view
            var Roles = context.Roles.ToList();
            return View(Roles);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            // Add a new role to the database, save it and redirect it to index view
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Roles/Edit
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Role = context.Roles.Find(id);
            if (Role == null)
            {
                return HttpNotFound();
            }
            return View(Role);
        }

        // POST: Roles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                context.Entry(role).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: Roles/Delete
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Role = context.Roles.Find(id);
            if (Role == null)
            {
                return HttpNotFound();
            }
            return View(Role);
        }

        // POST: Roles/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var Role = context.Roles.Find(id);
            context.Roles.Remove(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
