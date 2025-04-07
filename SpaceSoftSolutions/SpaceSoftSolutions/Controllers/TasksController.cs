using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaceSoftSolutions.Models;

namespace SpaceSoftSolutions.Controllers
{
    public class TasksController : Controller
    {
            private readonly MyDbContext _context;
            private readonly EmailService _emailService;

            public TasksController(MyDbContext context, EmailService emailService)
            {
                _context = context;
                _emailService = emailService;
            }
            public IActionResult Index()
            {
                return View();
            }

            public IActionResult AssignTasks(int id)
            {
                ViewBag.ManagerId = id;
                ViewBag.Employees = new SelectList(_context.Employees.Where(Employees => Employees.ManagerId == 1).ToList(), "Id", "Name");
                return View();
            }
            [HttpPost]
            public async Task<IActionResult> AssignTasks(SpaceSoftSolutions.Models.Task task)
            {

                //Email = employee.Email;
                task.Id = 0;

                _context.Tasks.Add(task);
                _context.SaveChanges();

                var emp = _context.Employees.Find(task.EmployeeId);
                await _emailService.SendEmailAsync(emp.Email, "New Task", $"<p><b>task title:</b> {task.TaskName}<br>" +
                        $"<b>task Discription:</b> {task.Description}<br></p>" +
                        $"<b>Strat Date :</b> {task.StartDate}<br></p>" +
                        $"<b>End Date:</b> {task.EndDate}<br></p>");
                return RedirectToAction("Index", "Home");
            }





        }
    }

