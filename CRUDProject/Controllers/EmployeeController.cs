using CRUDProject.DAL;
using CRUDProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUDProject.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDAL _dal;

        public EmployeeController(EmployeeDAL dal)
        {
            _dal = dal;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                employees = _dal.GetAll();
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
            }
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is invalid";
                }
                bool result = _dal.Insert(model);
                if (!result)
                {
                    TempData["errorMessage"] = "Unable to save the data";
                    return View();
                }
                TempData["successMessage"] = "New data has been added successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                Employee employee = _dal.GetById(id);
                if (employee.Id == 0)
                {
                    TempData["errorMessage"] = $"No data found with an Id: {id}";
                    return RedirectToAction("Index");
                }
                return View(employee);
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(Employee model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is invalid";
                    return View();
                }
                bool result = _dal.Update(model);

                if (!result)
                {
                    TempData["errorMessage"] = "Unable to update the data";
                    return View();
                }
                TempData["successMessage"] = "Data has been updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                Employee employee = _dal.GetById(id);
                if (employee.Id == 0)
                {
                    TempData["errorMessage"] = $"No data found with an Id: {id}";
                    return RedirectToAction("Index");
                }
                return View(employee);
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmation(Employee model)
        {
            try
            {
                bool result = _dal.Delete(model.Id);

                if (!result)
                {
                    TempData["errorMessage"] = "Unable to delete the data";
                    return View();
                }
                TempData["successMessage"] = "Data has been deleted successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
    }
}
