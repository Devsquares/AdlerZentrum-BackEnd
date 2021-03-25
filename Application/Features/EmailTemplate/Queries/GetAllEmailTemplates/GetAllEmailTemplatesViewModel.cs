using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features 
{
	public class GetAllEmailTemplatesViewModel
	{
		public int Id { get; set; }
		public int EmailTypeId { get; set; }
		public string TemplateName { get; set; }
		public string TemplateBody { get; set; }
	}
}
