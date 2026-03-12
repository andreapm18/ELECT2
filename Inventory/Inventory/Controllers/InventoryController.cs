using Inventory.Models.Database;
using Inventory.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public IActionResult Index(string search, int page = 1)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int pageSize = 10;

            var items = _inventoryRepository.GetItemsByUser(userId.Value, search, page, pageSize);

            ViewBag.TotalItems = _inventoryRepository.GetTotalItems(userId.Value, search);
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Search = search;

            return View(items);
        }

        [HttpPost]
        public IActionResult Create(string itemName, int quantity, decimal price)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var item = new InventoryItem
            {
                ItemName = itemName,
                Quantity = quantity,
                Price = price,
                DateAdded = DateTime.Now,
                UserId = userId.Value
            };

            _inventoryRepository.AddItem(item);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var item = _inventoryRepository.GetItemById(id);

            if (item != null && item.UserId == userId.Value)
            {
                _inventoryRepository.DeleteItem(id);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(int id, string itemName, int quantity, decimal price)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var item = _inventoryRepository.GetItemById(id);

            if (item != null && item.UserId == userId.Value)
            {
                item.ItemName = itemName;
                item.Quantity = quantity;
                item.Price = price;

                _inventoryRepository.UpdateItem(item);
            }

            return RedirectToAction("Index");
        }
    }
}