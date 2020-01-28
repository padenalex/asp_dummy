using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swish.Authorization;
using Swish.Data;
using Swish.Models;

namespace Swish.Controllers
{
    public class IdController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;

        public IdController(ApplicationDbContext context, IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;


        }
        private IList<VerificationProfile> VerificationProfiles { get; set; }
        
        // GET: Index (all allowed profiles)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (ModelState.IsValid)
            {
                var verificationProfiles = from c in _context.VerificationProfiles
                    select c;

                var isAuthorized = User.IsInRole(Constants.UserAdministratorsRole);
                var currentUserId = _userManager.GetUserId(User);

                if (!isAuthorized)
                {
                    verificationProfiles = verificationProfiles.Where(c => c.OwnerId == currentUserId);
                }
                
                VerificationProfiles = await verificationProfiles.ToListAsync();

                return View(VerificationProfiles);
            }

            return View();
        }

        // GET: Id/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (ModelState.IsValid)
            {
                var verificationProfile = await _context.VerificationProfiles
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (verificationProfile == null)
                {
                    return NotFound();
                }

                var isAuthorized = await _authorizationService.AuthorizeAsync(User, verificationProfile,
                    UserOperations.Read);

                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }

                return View(verificationProfile);
            }

            return RedirectToAction(nameof(Index));
        }
        
        // POST: Id/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, VerificationProfileStatus status)
        {
            if (ModelState.IsValid)
            {
                var verificationProfile = await _context.VerificationProfiles.FirstOrDefaultAsync(
                    m => m.Id == id);

                if (verificationProfile == null)
                {
                    return NotFound();
                }

                var userOperation = (status == VerificationProfileStatus.Approved)
                    ? UserOperations.Approve
                    : UserOperations.Reject;

                var isAuthorized = await _authorizationService.AuthorizeAsync(User, verificationProfile, userOperation);

                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }

                verificationProfile.Status = status;
                _context.VerificationProfiles.Update(verificationProfile);
                await _context.SaveChangesAsync();

                return View(verificationProfile);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Id/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Id/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,FakeImgStr")] VerificationProfile verificationProfile)
        {
            if (ModelState.IsValid)
            {
                verificationProfile.OwnerId = _userManager.GetUserId(User);
                var isAuthorized = await _authorizationService.AuthorizeAsync(User, verificationProfile,
                    UserOperations.Create);

                if (!isAuthorized.Succeeded)
                {
                    return NotFound();
                }
                _context.Add(verificationProfile);
                await _context.SaveChangesAsync();
                
                //return RedirectToAction(Details(verificationProfile.Id);
                return RedirectToAction("Details", "Id", new { id = verificationProfile.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Id/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (ModelState.IsValid)
            {
                var verificationProfile = await _context.VerificationProfiles
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (verificationProfile == null)
                {
                    return NotFound();
                }
                var isAuthorized = await _authorizationService.AuthorizeAsync(User, verificationProfile,
                    UserOperations.Update);

                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }
                
                return View(verificationProfile);
            }
            
            return RedirectToAction(nameof(Index));
        }


        // POST: Id/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,FakeImgStr, OwnerId")] VerificationProfile verificationProfile)
        {
            if (id != verificationProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var isAuthorized = await _authorizationService.AuthorizeAsync(User, verificationProfile,
                    UserOperations.Update);

                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }
                try
                {
                    _context.Update(verificationProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VerificationProfileExists(verificationProfile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Id", new { id = verificationProfile.Id });
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Id/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var verificationProfile = await _context.VerificationProfiles
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (verificationProfile == null)
                {
                    return NotFound();
                }

                var isAuthorized = await _authorizationService.AuthorizeAsync(
                    User, verificationProfile,
                    UserOperations.Delete);
                var currentUserId = _userManager.GetUserId(User);

                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }
                
                return View(verificationProfile);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Id/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var verificationProfile = await _context.VerificationProfiles.FindAsync(id);

                if (verificationProfile == null)
                {
                    return NotFound();
                }

                var isAuthorized = await _authorizationService.AuthorizeAsync(User, verificationProfile,
                    UserOperations.Delete);

                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }

                _context.VerificationProfiles.Remove(verificationProfile);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VerificationProfileExists(int id)
        {
            return _context.VerificationProfiles.Any(e => e.Id == id);
        }
    }
}
