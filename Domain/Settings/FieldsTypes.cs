using System;
using System.Collections.Generic;

namespace Domain.Settings
{
    public class FieldsTypes
    {
        public Dictionary<string, Types> MobilePhones= new Dictionary<string, Types>();
        
        public FieldsTypes()
        {
            //Mobile phones
            MobilePhones.Add("ID", Types.Number);
            MobilePhones.Add("VERSIONID", Types.Number);
            MobilePhones.Add("NAME", Types.Text);
            MobilePhones.Add("DESCRIPTION", Types.Text);
            MobilePhones.Add("VIDEOURL", Types.Text);
            MobilePhones.Add("PRODUCTIMAGE", Types.Text);
            MobilePhones.Add("PRICE", Types.Number);
            MobilePhones.Add("CONFIG", Types.Text);
            MobilePhones.Add("DEVICEBRAND", Types.Text);
            MobilePhones.Add("RAM", Types.Number);
            MobilePhones.Add("CONNECTIVITY", Types.Text);
            MobilePhones.Add("STORAGE", Types.Text);
            MobilePhones.Add("SIMCARDSLOT", Types.Text);
            MobilePhones.Add("COLOR", Types.Text);
            MobilePhones.Add("VERSIONPRICE", Types.Number);
            MobilePhones.Add("VERSIONDISCOUNTPRICE", Types.Number);
            MobilePhones.Add("DISCOUNTPRICE", Types.Number);
            MobilePhones.Add("QUANTITY", Types.Number);
            MobilePhones.Add("HIGHLIGHT", Types.Number);
            MobilePhones.Add("HOTDEAL", Types.Boolean);


    }
    }
}
