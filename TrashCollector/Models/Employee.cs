using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
	public class Employee
	{
		[Key]
		public int EmployeeId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public string StreetAddress1 { get; set; }
		public string StreetAddress2 { get; set; }

		//[ForeignKey("CityId")]
		//public int CityId { get; set; }
		//public City City { get; set; }

		//[ForeignKey("StateId")]
		//public int StateId { get; set; }
		//public State State { get; set; }

		[ForeignKey("Zipcode")]
		public int ZipcodeId { get; set; }
		public Zipcode Zipcode { get; set; }
	}
}