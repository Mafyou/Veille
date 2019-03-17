using FrontEndWebRole.Models;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using System.Web.Mvc;

namespace FrontEndWebRole.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Submit");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Submit()
        {
            // Get a NamespaceManager which allows you to perform management and
            // diagnostic operations on your Service Bus queues.
            var namespaceManager = QueueConnector.CreateNamespaceManager();

            // Get the queue, and obtain the message count.
            var queue = namespaceManager.GetQueue(QueueConnector.QueueName);
            ViewBag.MessageCount = queue.MessageCount;

            return View();
        }

        [HttpPost]
        // Attribute to help prevent cross-site scripting attacks and
        // cross-site request forgery.  
        [ValidateAntiForgeryToken]
        public ActionResult Submit(OnlineOrder order)
        {
            if (ModelState.IsValid)
            {
                // Will put code for submitting to queue here.
                var message = new BrokeredMessage(order);
                QueueConnector.Initialize();
                QueueConnector.OrdersQueueClient.Send(message);

                return RedirectToAction("Submit");
            }
            else
            {
                return View(order);
            }
        }
    }
}