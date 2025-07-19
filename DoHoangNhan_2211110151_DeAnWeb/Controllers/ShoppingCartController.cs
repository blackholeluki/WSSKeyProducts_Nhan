using DoHoangNhan_2211110151_DeAnWeb.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DoHoangNhan_2211110151_DeAnWeb.Controllers
{
    public class ShoppingCartController : Controller
    {
        private PayPal.Api.Payment payment;
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            // Getting the API context  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

            try
            {
                // A resource representing a Payer that funds a payment Payment Method as PayPal  
                // Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    // This section will be executed first because PayerID doesn't exist  
                    // It is returned by the create function call of the payment class  
                    // Creating a payment  
                    // Base URL is the URL on which PayPal sends back the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/PaymentWithPayPal?";

                    // Here we are generating GUID for storing the paymentID received in session  
                    // which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));

                    // CreatePayment function gives us the payment approval URL  
                    // on which payer is redirected for PayPal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    // Get links returned from PayPal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            // Saving the PayPal redirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // Saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);

                    // Redirect the user to the PayPal approval URL  
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function executes after receiving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    // If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }

            // On successful payment, show success page to user  
            return View("SuccessView");
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };

            this.payment = new Payment()
            {
                id = paymentId
            };

            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            // Create item list and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            // Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Name comes here",
                currency = "USD",
                price = "1",
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };

            // Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = "3", // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };

            var transactionList = new List<Transaction>();

            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), // Generate an Invoice No    
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

            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
    }
}
