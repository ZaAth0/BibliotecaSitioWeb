// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bibliotecaaa.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // Propiedad para controlar si estamos en modo de edición
        public bool IsEditMode { get; set; }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

            [Required]
            [Display(Name = "Nombre")] 
            public string FirstName { get; set; }
            
            [Required]
            [Display(Name = "Apellido")] 
            public string LastName { get; set; }
            
            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Fecha de Nacimiento")]
            public DateTime? BirthDate { get; set; }
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            var claims = await _userManager.GetClaimsAsync(user); 
            var firstNameClaim = claims.FirstOrDefault(c => c.Type == "FirstName"); 
            var lastNameClaim = claims.FirstOrDefault(c => c.Type == "LastName"); 
            var birthDateClaim = claims.FirstOrDefault(c => c.Type == "BirthDate");

           

            Input = new InputModel
            {
                FirstName = firstNameClaim?.Value,
                LastName = lastNameClaim?.Value,
                BirthDate = birthDateClaim != null ? DateTime.Parse(birthDateClaim.Value) : (DateTime?)null
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se puede cargar al usuario con ID '{_userManager.GetUserId(User)}'.");
            }

            // Si la página se carga por primera vez, no está en modo de edición
            IsEditMode = false;
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se puede cargar al usuario con ID '{_userManager.GetUserId(User)}'.");
            }

            // Cambiar a modo edición si la acción es "Edit"
            if (action == "Edit")
            {
                IsEditMode = true;
                await LoadAsync(user);
                return Page();
            }

            // Guardar los cambios si el formulario es válido
            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }


            var claims = await _userManager.GetClaimsAsync(user);
            var firstNameClaim = claims.FirstOrDefault(c => c.Type == "FirstName");
            var lastNameClaim = claims.FirstOrDefault(c => c.Type == "LastName"); 
            var birthDateClaim = claims.FirstOrDefault(c => c.Type == "BirthDate");

            if (firstNameClaim == null) 
            { 
                await _userManager.AddClaimAsync(user, new Claim("FirstName", Input.FirstName)); 
            } 
            else if (Input.FirstName != firstNameClaim.Value) 
            { 
                await _userManager.ReplaceClaimAsync(user, firstNameClaim, new Claim("FirstName", Input.FirstName)); 
            }

            if (lastNameClaim == null) 
            { 
                await _userManager.AddClaimAsync(user, new Claim("LastName", Input.LastName)); 
            } 
            else if (Input.LastName != lastNameClaim.Value) 
            { 
                await _userManager.ReplaceClaimAsync(user, lastNameClaim, new Claim("LastName", Input.LastName)); 
            }

            if (birthDateClaim == null) 
            {
                await _userManager.AddClaimAsync(user, new Claim("BirthDate", Input.BirthDate.HasValue ? Input.BirthDate.Value.ToString("yyyy-MM-dd") : string.Empty));
            } 
            else if (Input.BirthDate != DateTime.Parse(birthDateClaim.Value)) 
            {
                await _userManager.ReplaceClaimAsync(user, birthDateClaim, new Claim("BirthDate", Input.BirthDate.Value.ToString("yyyy-MM-dd")));
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Tu perfil ha sido actualizado";
            return RedirectToPage();
        }
    }
}
