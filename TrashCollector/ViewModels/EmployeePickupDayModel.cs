using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrashCollector.Models;

namespace TrashCollector.ViewModels
{
	public class EmployeePickupDayModel
	{
		public PickupDay CurrentDay { get; set; }
		public Zipcode CurrentZipcode { get; set; }
		public List<Pickup> Pickups { get; set; }
	}
}