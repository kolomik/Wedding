using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cards.Context;
using Cards.Context.Models;

namespace Cards.Controllers
{
    [Authorize]
    [RequireHttps]
    public class PeopleController : Controller
    {
        private WeddingContext db = new WeddingContext();

        // GET: People
        public async Task<ActionResult> Index ()
        {
            return View( await db.Persons.ToListAsync() );
        }

        // GET: People/Details/5
        public async Task<ActionResult> Details ( int? id )
        {
            if ( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Person person = await db.Persons.FindAsync( id );
            if ( person == null )
            {
                return HttpNotFound();
            }
            return View( person );
        }

        // GET: People/Create
        public ActionResult Create ()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create ( [Bind( Include = "ID,ShortName,FullName" )] Person person )
        {
            if ( ModelState.IsValid )
            {
                db.Persons.Add( person );
                await db.SaveChangesAsync();
                return RedirectToAction( "Index" );
            }

            return View( person );
        }

        // GET: People/Edit/5
        public async Task<ActionResult> Edit ( int? id )
        {
            if ( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Person person = await db.Persons.FindAsync( id );
            if ( person == null )
            {
                return HttpNotFound();
            }
            return View( person );
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit ( [Bind( Include = "ID,ShortName,FullName" )] Person person )
        {
            if ( ModelState.IsValid )
            {
                db.Entry( person ).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction( "Index" );
            }
            return View( person );
        }

        // GET: People/Delete/5
        public async Task<ActionResult> Delete ( int? id )
        {
            if ( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Person person = await db.Persons.FindAsync( id );
            if ( person == null )
            {
                return HttpNotFound();
            }
            return View( person );
        }

        // POST: People/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed ( int id )
        {
            Person person = await db.Persons.FindAsync( id );
            db.Persons.Remove( person );
            await db.SaveChangesAsync();
            return RedirectToAction( "Index" );
        }

        protected override void Dispose ( bool disposing )
        {
            if ( disposing )
            {
                db.Dispose();
            }
            base.Dispose( disposing );
        }
    }
}
