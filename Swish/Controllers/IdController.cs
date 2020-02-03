using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
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
                var currentUserId = _userManager.GetUserId(User);
                
                var MIds = from m in _context.ManagerIds select m;
                var MId = currentUserId;
                
                var MClaim = (from c in _context.IdClaimManagers select c)
                    .Where(c => c.M.M.Id == MId);

                var isAuthorized = User.IsInRole(Constants.UserAdministratorsRole);
                if (isAuthorized)
                {
                    var verificationProfiles = from c in _context.VerificationProfiles
                        select c;
                }

                if (!isAuthorized)
                {
                    var claims = (from c in MClaim.UIDs select c);

                }

                VerificationProfiles = await verificationProfiles.ToListAsync();

                return View(VerificationProfiles);
            }

            return View();
        }



        // GET: Id/Create
        [HttpGet]
        //[Authorize(Roles="Managers")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Id/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles="Managers, Administrators")]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,FakeImgStr")]
            VerificationProfile verificationProfile)
        {
            if (ModelState.IsValid)
            {
                verificationProfile.MIds.Add(_userManager.GetUserId(User));
                var isAuthorized = await _authorizationService.AuthorizeAsync(User, verificationProfile,
                    UserOperations.Create);

                if (!isAuthorized.Succeeded)
                {
                    return NotFound();
                }

                _context.Add(verificationProfile);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
                //return RedirectToAction("Details", "Id", new { id = verificationProfile.UId });
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
