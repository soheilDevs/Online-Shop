﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Admin.Order;
using Shop.Web.Extentions;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class OrderController : AdminBaseController
    {
        #region Constractor

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #endregion

        #region Filter-Orders

        public async Task<IActionResult> FilterOrders(FilterOrdersViewModel filterOrders)
        {
            return View(await _orderService.FilterOrders(filterOrders));
        }

        #endregion

        #region Change Order-State
        public async Task<IActionResult> ChangeStateToSent(long orderId)
        {
            var result = await _orderService.ChangeStateToSent(orderId);
            if (result)
            {
                TempData[SuccessMessage] = "وضعیت سفارش  با موفقیت تغییر کرد";

                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }


        #endregion

        #region OrderDetail

        public async Task<IActionResult> OrderDetail(long orderId)
        {
            var data = await _orderService.GetOrderDetail(orderId);
            if (data==null)
            {
                return NotFound();
            }

            return View(data);
        }

        #endregion
    }
}
