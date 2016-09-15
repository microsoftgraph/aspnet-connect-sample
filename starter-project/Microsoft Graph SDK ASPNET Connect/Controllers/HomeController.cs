/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft_Graph_SDK_ASPNET_Connect.Helpers;
using Microsoft_Graph_SDK_ASPNET_Connect.Models;
using Resources;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class HomeController : Controller
    {
        GraphService graphService = new GraphService();

        public ActionResult Index()
        {
            return View("Graph");
        }

        // Controller actions

        public ActionResult About()
        {
            return View();
        }
    }
}