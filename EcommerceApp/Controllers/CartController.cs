using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Data;
using EcommerceApp.Models;
using EcommerceApp.Services;
using System.Threading.Tasks;

namespace EcommerceApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;

        public CartController(ApplicationDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        // GET: Cart
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        // POST: Cart/AddToCart/5
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }

            _cartService.AddToCart(product, quantity);
            
            return RedirectToAction(nameof(Index));
        }

        // POST: Cart/RemoveFromCart/5
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Cart/UpdateQuantity
        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            _cartService.UpdateQuantity(id, quantity);
            return RedirectToAction(nameof(Index));
        }

        // POST: Cart/ClearCart
        [HttpPost]
        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            return RedirectToAction(nameof(Index));
        }

        // GET: Cart/Checkout
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCart();
            
            if (cart.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            
            var order = new Order();
            return View(order);
        }

        // POST: Cart/PlaceOrder
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                var cart = _cartService.GetCart();
                
                if (cart.Count == 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                order.OrderDate = System.DateTime.Now;
                order.TotalAmount = _cartService.GetTotal();
                
                foreach (var item in cart)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    
                    order.OrderItems.Add(orderItem);
                }
                
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                
                _cartService.ClearCart();
                
                return RedirectToAction(nameof(OrderConfirmation), new { id = order.Id });
            }
            
            return View("Checkout", order);
        }

        // GET: Cart/OrderConfirmation/5
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
                
            if (order == null)
            {
                return NotFound();
            }
            
            return View(order);
        }
    }
}