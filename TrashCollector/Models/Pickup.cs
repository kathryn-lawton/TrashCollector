using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
	public class Pickup
	{
		[Key]
		public int PickupId { get; set; }
		public bool PickupStatus { get; set; }
		public double PickupCost { get; set; }

		[ForeignKey("PickupDay")]
		public int PickupDayId { get; set; }
		public PickupDay PickupDay { get; set; }

		[ForeignKey("Customer")]
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }
	}
}