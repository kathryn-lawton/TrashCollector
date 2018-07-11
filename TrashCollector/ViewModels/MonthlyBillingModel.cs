using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrashCollector.Models;

namespace TrashCollector.ViewModels
{
	public class MonthlyBillingModel
	{
		public List<Pickup> Pickups { get; set; }
		public double TotalMonthlyBill { get; set; }
	}
}