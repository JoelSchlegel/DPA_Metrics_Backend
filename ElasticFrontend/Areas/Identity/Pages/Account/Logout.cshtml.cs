// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using App.Metrics;
using ElasticFrontend.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElasticFrontend.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<SampleUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        private readonly IMetrics _metrics;

        public LogoutModel(SignInManager<SampleUser> signInManager, ILogger<LogoutModel> logger, IMetrics metrics)
        {
            _signInManager = signInManager;
            _logger = logger;
            _metrics = metrics;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
