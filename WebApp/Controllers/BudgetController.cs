using Data.Dto;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Data.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class BudgetController : Controller
    {
        private readonly IBudgetService _budgetService;
        private readonly IMapper _mapper;

        public BudgetController(IBudgetService budgetService, IMapper mapper)
        {
            _budgetService = budgetService;
            _mapper = mapper;
        }

        // GET: BudgetController
        public ActionResult Index()
        {
            //ovo ti dohvati guid od usera koji je nap request, To nam treba da ne vadimo sve buđete od svih usera
            var guid = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value;
            if (guid == null)
                return BadRequest("User not found");

            var budgets = _budgetService.GetAll(Guid.Parse(guid));
            var vms = _mapper.Map<List<BudgetVM>>(budgets);
            return View(vms);
        }

        // GET: BudgetController/Details/5
        public ActionResult Details(long id)
        {
            var budget = _budgetRepository.GetById(id);
            if (budget == null)
                return NotFound();

            var budgetDto = new BudgetDto
            {
                Idbudget = budget.Idbudget,
                Sum = budget.Sum,
                UserId = budget.UserId,
                CategoryId = budget.CategoryId,
                Category = budget.Category,
                User = budget.User
            };

            return View(budgetDto);
        }

        // GET: BudgetController/Create
        public ActionResult Create()
        {
            ViewBag.Users = _userRepository.GetAll();
            ViewBag.Categories = _categoryRepository.GetAll();
            return View();
        }

        // POST: BudgetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BudgetDto budgetDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var budget = new Budget
                    {
                        Sum = budgetDto.Sum,
                        UserId = budgetDto.UserId,
                        CategoryId = budgetDto.CategoryId
                    };

                    _budgetRepository.Add(budget);
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Users = _userRepository.GetAll();
                ViewBag.Categories = _categoryRepository.GetAll();
                return View(budgetDto);
            }
            catch
            {
                return View(budgetDto);
            }
        }

        // GET: BudgetController/Edit/5
        public ActionResult Edit(long id)
        {
            var budget = _budgetRepository.GetById(id);
            if (budget == null)
                return NotFound();

            var budgetDto = new BudgetDto
            {
                Idbudget = budget.Idbudget,
                Sum = budget.Sum,
                UserId = budget.UserId,
                CategoryId = budget.CategoryId
            };

            ViewBag.Users = _userRepository.GetAll();
            ViewBag.Categories = _categoryRepository.GetAll();
            return View(budgetDto);
        }

        // POST: BudgetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(long id, BudgetDto budgetDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var budget = _budgetRepository.GetById(id);
                    if (budget == null)
                        return NotFound();

                    budget.Sum = budgetDto.Sum;
                    budget.UserId = budgetDto.UserId;
                    budget.CategoryId = budgetDto.CategoryId;

                    _budgetRepository.Update(budget);
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Users = _userRepository.GetAll();
                ViewBag.Categories = _categoryRepository.GetAll();
                return View(budgetDto);
            }
            catch
            {
                return View(budgetDto);
            }
        }

        // GET: BudgetController/Delete/5
        public ActionResult Delete(long id)
        {
            var budget = _budgetRepository.GetById(id);
            if (budget == null)
                return NotFound();

            return View(budget);
        }

        // POST: BudgetController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id, IFormCollection collection)
        {
            try
            {
                var budget = _budgetRepository.GetById(id);
                if (budget == null)
                    return NotFound();

                _budgetRepository.Delete(budget);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}