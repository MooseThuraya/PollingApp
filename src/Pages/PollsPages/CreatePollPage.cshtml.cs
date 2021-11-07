using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages.PollsPages
{
    /// <summary>
    /// CreatePollPage Model Class
    /// </summary>
    public class CreatePollPageModel : PageModel
    {
        /// <summary>
        /// Message To Display 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Log Category for CreatePollPageModel
        /// </summary>
        private readonly ILogger<CreatePollPageModel> _logger;

        /// <summary>
        /// JsonFileUserService to access, get, set User Services
        /// </summary>
        public JsonFileUserService UserServices { get; set; }

        /// <summary>
        /// JsonFilePollService to access, get, set Poll Services
        /// </summary>
        public JsonFilePollService PollService { get; set; }

        /// <summary>
        /// String to Hold Cookie Value acquired from User Services
        /// </summary>
        public string CookieNameValue { get; set; }

        /// <summary>
        /// Model for Holding Attributes/ variable to Create a Poll
        /// </summary>
        [BindProperty]
        public CreatePollModel CreatePoll { get; set; }

        /// <summary>
        /// Constructor for the CreatePollPage 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userService"></param>
        /// <param name="pollService"></param>
        public CreatePollPageModel(ILogger<CreatePollPageModel> logger,
            JsonFileUserService userService, JsonFilePollService pollService)
        {
            // Initialize attributes with default Values
            _logger = logger;
            UserServices = userService;
            PollService = pollService;
            CookieNameValue = UserServices.GetCookieValue("nameCookie");
        }

        /// <summary>
        /// OnGet Method, for CreatePollPage
        /// </summary>
        public void OnGet()
        {
            // Message to Diplay in Html
            Message = $"Welcome {CookieNameValue}:  Create Your Amazing Poll";
        }

        /// <summary>
        /// OnPost Method, Used to Create a Poll based on CreatePollModel
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            // Get User based on Name (Cookie Value)
            UserModel getUser = UserServices.GetUser(CookieNameValue);

            // Check if User Exists
            if(getUser == null)
            {
                // Redirect to Page
                return RedirectToPage("PollsPage");
            }

            // Pass CreatePollModel and UserID to CreatePoll Service and Fetch the Result
            PollModel pollCreationStatus = PollService.CreatePoll(CreatePoll, getUser.UserID);

            // Clear the Model State
            ModelState.Clear();

            // Reset CreatePollModel attributes before redirecing back to the Page
            CreatePoll.CreateTitle = String.Empty;
            CreatePoll.CreateDescription = String.Empty;
            CreatePoll.CreateOpinionOne = String.Empty;
            CreatePoll.CreateOpinionTwo = String.Empty;

            // Check if Poll Creation and Addition to Poll DataSet was succesful
            if(pollCreationStatus == null)
            {
                // Wasn't Successful , Display Error Message
                Message = $"Something Went Wrong Try Again";

                // Redirect To This Page
                return Page();
            }

            // Was Succesfull, Print A Message of Success
            Message = $"Awesome {pollCreationStatus.Title} Created: Make Another Poll";

            // Redirect To This Page
            return Page();
        }
    }
}
