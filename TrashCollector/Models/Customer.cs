using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
	public class Customer
	{
		[Key]
		public int CustomerId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public string StreetAddress1 { get; set; }
		public string StreetAddress2 { get; set; }

		[ForeignKey("City")]
		public int CityId { get; set; }
		public City City { get; set; }

		[ForeignKey("State")]
		public int StateId { get; set; }
		public State State { get; set; }

		[ForeignKey("Zipcode")]
		public int ZipcodeId { get; set; }
		public Zipcode Zipcode { get; set; }

		[ForeignKey("PickupDay")]
		public int PickupDayId { get; set; }
		public PickupDay PickupDay { get; set; }

		[ForeignKey("ApplicationUser")]
		public string ApplicationUserID { get; set; }
		public ApplicationUser ApplicationUser { get; set; }


		public bool PickupStatus { get; set; }

		// public IEnumerable<PickupDay> PickupDays { get; set; }										   
		
		//[ForeignKey("BillingStatusId")]
		//public int BillingStatusId { get; set; }
		//public BillingStatus BillingStatus { get; set; }
		//account status (payment)
	}
}