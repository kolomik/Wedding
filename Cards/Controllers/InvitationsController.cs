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
    public class InvitationsController : Controller
    {
        private WeddingContext db = new WeddingContext();

        // GET: Invitations
        public async Task<ActionResult> Index ()
        {
            return View( await db.Invitations.Include( x => x.Persons ).ToListAsync() );
        }

        // GET: Invitations/Details/5
        public async Task<ActionResult> Details ( Guid? id )
        {
            if ( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Invitation invitation = await db.Invitations.FindAsync( id );
            if ( invitation == null )
            {
                return HttpNotFound();
            }
            return View( invitation );
        }

        // GET: Invitations/Create
        public ActionResult Create ()
        {
            return View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create ( [Bind( Include = "ID,FriendlyName,IsAccepted" )] Invitation invitation, List<int> Persons )
        {
            if ( ModelState.IsValid )
            {
                invitation.ID = Guid.NewGuid();
                UpdatePersons( invitation, Persons );
                //l.ForEach( x => db.Entry( x ).State = EntityState.Modified );
                db.Invitations.Add( invitation );
                await db.SaveChangesAsync();
                return RedirectToAction( "Index" );
            }

            return View( invitation );
        }

        // GET: Invitations/Edit/5
        public async Task<ActionResult> Edit ( Guid? id )
        {
            if ( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Invitation invitation = await db.Invitations.FindAsync( id );
            if ( invitation == null )
            {
                return HttpNotFound();
            }
            return View( invitation );
        }

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit ( [Bind( Include = "ID,FriendlyName,IsAccepted" )] Invitation invitation, List<int> Persons )
        {
            if ( ModelState.IsValid )
            {
                var invite = db.Invitations.Include( x => x.Persons ).FirstOrDefault( x => x.ID == invitation.ID );
                if ( invite == null )
                    return View( invitation );
                invite.FriendlyName = invitation.FriendlyName;
                UpdatePersons( invite, Persons );
                //l.ForEach( x => db.Entry( x ).State = EntityState.Modified );
                //db.Entry( invite ).State = EntityState.Modified;
                var i = await db.SaveChangesAsync();
                return RedirectToAction( "Index" );
            }
            return View( invitation );
        }

        private void UpdatePersons ( Invitation invitation, List<int> Persons )
        {
            invitation.Persons = new List<Person>();
            if ( Persons == null || Persons.Count == 0 )
                return;
            var r = db.Persons.Where( x => Persons.Contains( x.ID ) ).ToList();
            invitation.Persons = r;
        }

        // GET: Invitations/Delete/5
        public async Task<ActionResult> Delete ( Guid? id )
        {
            if ( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Invitation invitation = await db.Invitations.FindAsync( id );
            if ( invitation == null )
            {
                return HttpNotFound();
            }
            return View( invitation );
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed ( Guid id )
        {
            Invitation invitation = await db.Invitations.Include( x => x.Persons ).FirstOrDefaultAsync( x => x.ID == id );
            invitation.Persons.Clear();
            await db.SaveChangesAsync();
            db.Invitations.Remove( invitation );
            await db.SaveChangesAsync();
            return RedirectToAction( "Index" );
        }

        [AllowAnonymous]
        public async Task<ActionResult> Show ( string id )
        {
            var cookieName = "securityTokenExtended";
            var guid = Guid.Empty;
            Invitation invite = null;

            if ( string.IsNullOrEmpty( id ) )
            {
                var cc = Request.Cookies.Get( cookieName );
                if ( cc != null )
                    id = cc.Value;
            }
            if ( Guid.TryParse( id, out guid ) )
            {
                invite = await db.Invitations.Include( x => x.Persons ).FirstOrDefaultAsync( x => x.ID == guid );
            }
            else
            {
                invite = await db.Invitations.Include( x => x.Persons ).FirstOrDefaultAsync( x => x.FriendlyName == id );
            }
            if ( invite == null )
            {
                return View( "Nothing" );
            }
            Response.Cookies.Add( new HttpCookie( cookieName, invite.ID.ToString() )
            {
                Shareable = false,
                Expires = DateTime.Now.AddMonths( 6 )
            } );
            return View( invite );
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
