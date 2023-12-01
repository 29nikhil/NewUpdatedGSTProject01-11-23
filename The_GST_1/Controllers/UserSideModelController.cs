using Data_Access_Layer.Db_Context;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository_Logic.Dto;
using Repository_Logic.FileUploads.Interface;
using Repository_Logic.ModelView;
using Repository_Logic.ReturnFile.Interface;
using Repository_Logic.UserOtherDatails.Interface;
using Repository_Logic.ViewFilledGSt.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace The_GST_1.Controllers
{
    [Authorize(Roles = "User")]
    public class UserSideModelController : Controller
    {
        private readonly IExtraDetails extraDetails;
        private readonly Application_Db_Context _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFileRepository _fileRepository;
        private readonly IReturnFile _returnFile;
        private readonly IViewGSTFilledGst _viewGSTFilledGst;
        private readonly IConverter _pdfConverter;

        public UserSideModelController(IExtraDetails extraDetails,
            Application_Db_Context context, UserManager<IdentityUser> userManager,
            IFileRepository fileRepository, IReturnFile returnFile,
            IViewGSTFilledGst viewGSTFilledGst, IConverter pdfConverter)
        {
            this.extraDetails = extraDetails;
            _context = context;
            _userManager = userManager;
            _fileRepository = fileRepository;
            _returnFile = returnFile;
            _viewGSTFilledGst = viewGSTFilledGst;
            _pdfConverter = pdfConverter;
        }

        public IActionResult ViewProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserData = extraDetails.GetUser(userId);
            return View(UserData);
        }

        public IActionResult UpdateProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserData = extraDetails.GetUser(userId);
            return View(UserData);
        }

        [HttpPost]
        public IActionResult UpdateProfileSave(JoinUserTable_Dto userModelView)
        {
            try
            {
                var useremailcheck = extraDetails.GetUser(userModelView.Id);

                if (useremailcheck.Email != userModelView.Email)
                {
                    useremailcheck.Confirm = false;
                    extraDetails.UpdateUser(userModelView);
                    extraDetails.UpdateEmailConfirmation(userModelView.Id);
                    TempData["UpdateUserModel"] = "Update User and Send Confirmation Link to Your Email: " + userModelView.Email;
                    return RedirectToAction("UpdateEmail_Confirmation", "EmailSending", new { Email = userModelView.Email, UserId = userModelView.Id, UserRole = "User" });
                }
                else
                {
                    extraDetails.UpdateUser(userModelView);
                    TempData["UpdateUserProfile"] = "Update User Record: " + userModelView.FirstName;
                    return RedirectToAction("ViewProfile");
                }
            }
            catch
            {
                return View();
            }
        }

        public IActionResult ShowPartialView()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var data = _context.UserDetails.Where(x => x.UserId == userId).FirstOrDefault();
            return RedirectToAction("DocumentsView", "UserDetails", new { Id = userId });

            // Add your logic for displaying the partial view here
        }

        public IActionResult TaxCalculator()
        {
            // Add your logic for the Tax Calculator page here
            return View();
        }

        public async Task<IActionResult> ViewFiledGst()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserData = extraDetails.GetUser(userId);

            var user = await _userManager.FindByIdAsync(userId);
            var isInRole = await _userManager.IsInRoleAsync(user, "User");
            List<ViewFilleGSt_Dto> ReturnFileData;

            if (isInRole)
            {
                ReturnFileData = await _viewGSTFilledGst.GetReturnedFilesDataForUser(userId);
            }
            else
            {
                // Handle cases when the user is not in the "User" role
                // You may want to return an error or handle it accordingly
                ReturnFileData = new List<ViewFilleGSt_Dto>();
            }

            ViewBag.Name = UserData.FirstName + " " + UserData.MiddleName + " " + UserData.LastName;
            ViewBag.Email = UserData.Email;
            ViewBag.AdharNo = UserData.AdharNo;
            TempData["Name"] = UserData.Email;

            return View(ReturnFileData);
        }

        public async Task<JsonResult> ViewFiledGstDataDataTable()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var dataTable_ = new DataTable_Dto
            {
                Draw = Request.Form["draw"].FirstOrDefault(),
                sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(),
                sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault(),
                SearchValue = Request.Form["search[value]"].FirstOrDefault(),
                PageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0"),
                Skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0")
            };
            var data = await _viewGSTFilledGst.ViewReturnFilesDataTableUser(dataTable_, userId);

            var totalRecords = data.Count;
            var filterRecord = data.Count;
            var filteredData = data.Skip(dataTable_.Skip).Take(dataTable_.PageSize).ToList();

            var result = new
            {
                draw = dataTable_.Draw,
                recordsTotal = totalRecords,
                recordsFiltered = filterRecord,
                data = filteredData
            };

            return Json(result);
        }

        public IActionResult GeneratePdf()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserData = extraDetails.GetUser(userId);

            // Check if UserData is null before using it
            var userName = UserData != null ? $"{UserData.FirstName} {UserData.MiddleName} {UserData.LastName}" : "N/A";

            // Fetch the data you want to display in the PDF
            var returnFileData = _viewGSTFilledGst.GetReturnedFilesDataForUser(userId).Result;
            var htmlContent = "<html><body><table class=\"table table-dark\">";

            htmlContent += "<tr>";
            htmlContent += $"<td>Name: {userName}</td>";
            htmlContent += "</tr>";
            htmlContent += "</table>";

            htmlContent += "<table>";

            // Define the table header
         
            // Check if returnFileData is not null and contains data
            if (returnFileData != null && returnFileData.Any())
            {
                // Populate the table with data
                 htmlContent += @"

   <style>
        table {
            width: 100%;
            border-collapse: collapse;
        }
        th, td {
            border: 1px solid #000;
            padding: 8px;
            text-align: left;
        }
        th {
            background-color: #f2f2f2;
        }
    </style>
</head>
<body>
    <table>
        <tr>
            <th>GSTNo</th>
            <th>GSTType</th>
            <th>Date</th>
            <th>FiledBy</th>
            <th>FileName</th>
            <th>Year</th>
            <th>Tax Period</th>
        </tr>";

                // Check if returnFileData is not null and contains data
                if (returnFileData != null && returnFileData.Any())
                {
                    // Populate the table with data
                    foreach (var item in returnFileData)
                    {
                        htmlContent += $@"
        <tr>
            <td>{item.GSTNo}</td>
            <td>{item.GSTType}</td>
            <td>{item.Date}</td>
            <td>{item.FiledBy}</td>
            <td>{item.FileName}</td>
            <td>{item.Year}</td>
            <td>{item.TaxPeriod}</td>
        </tr>";
                    }
                }

                htmlContent += @"
    </table>
</body>
</html>";

            }
            else
            {
                htmlContent += "<tr><td colspan='8'>No data available</td></tr>";
            }

            htmlContent += "</table></body></html>";

            if (!string.IsNullOrWhiteSpace(htmlContent))
            {

                var doc = new HtmlToPdfDocument
                {
                    GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Landscape,
                        PaperSize = PaperKind.A4,
                    },
                    Objects = {
                        new ObjectSettings
                        {
                            HtmlContent = htmlContent,
                        }
                    }
                };

                var pdfBytes = _pdfConverter.Convert(doc);
                return File(pdfBytes, "application/pdf", "Products.pdf");
            }
            else
            {
                // Handle the case where htmlContent is empty
                // You may want to log an error or return an error response.
                // The handling depends on your application's requirements.
                return File(new byte[0], "application/pdf", "Products.pdf");
            }
        }
    }
}
