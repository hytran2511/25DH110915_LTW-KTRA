using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using _25DH110915_LTW.Models; // Đảm bảo khai báo đúng namespace Models của bạn

namespace _25DH110915_LTW.Controllers
{
    public class PaypalController : Controller
    {
        // Hàm khởi tạo API Context từ Web.config
        private APIContext GetAPIContext()
        {
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            return apiContext;
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            APIContext apiContext = GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    // Bước 1: Khởi tạo thanh toán và chuyển hướng sang web PayPal
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/PaymentWithPaypal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // Bước 2: Nhận kết quả trả về từ PayPal
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return Content("Thanh toán thất bại hoặc đã bị hủy!");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("Lỗi thanh toán PayPal: " + ex.Message);
            }

            // BƯỚC 3: THANH TOÁN THÀNH CÔNG -> Chuyển về trang OrderSuccess
            // (Lưu ý: Logic lưu đơn hàng vào DB đã được xử lý ở OrderController trước khi gọi sang đây)
            Session.Remove("Cart");
            return RedirectToAction("Index", "Home"); // Tạm chuyển về Home hoặc trang Lịch sử
        }

        private Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var cart = Session["Cart"] as Cart;
            var itemList = new ItemList() { items = new List<Item>() };

            // Đưa sản phẩm từ Giỏ hàng sang PayPal
            foreach (var cartItem in cart.Items)
            {
                itemList.items.Add(new Item()
                {
                    name = cartItem.ProductName,
                    currency = "USD",
                    price = Math.Round(cartItem.Price / 24000, 2).ToString(), // Đổi VNĐ sang USD
                    quantity = cartItem.Quantity.ToString(),
                    sku = cartItem.ProductID.ToString()
                });
            }

            var payer = new Payer() { payment_method = "paypal" };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var totalUSD = Math.Round(cart.TotalCartValue() / 24000, 2).ToString();
            var details = new Details() { tax = "0", shipping = "0", subtotal = totalUSD };
            var amount = new Amount() { currency = "USD", total = totalUSD, details = details };

            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Thanh toán đơn hàng MyStore",
                invoice_number = Guid.NewGuid().ToString(),
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
        }
    }
}