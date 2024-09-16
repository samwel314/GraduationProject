using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelLinkerDataAccess.Data;

namespace TravelLinker.Helpers
{
    public static class StaticData
    {

        public static readonly string HotelType = "Hotel";
        public static readonly string CompanyType = "TransportCompany";
        public static readonly string UserTypeClaim = "UserType"; 
        public static readonly string CustomerType = "Customer";
        public static readonly string AdminType = "Admin";
        public static List<SelectListItem> LoadEnterprises()
        {
            
            return new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = "Hotel" , 
                    Text = "Hotel"
                }
                ,
                new SelectListItem
                {
                    Value ="TransportCompany" , 
                    Text = "Transport Company"
                }
                ,
                 new SelectListItem
                {
                    Value ="Admin" ,
                    Text = "System Admin"
                }
            };
        }
        public static List<SelectListItem> LoadRoomTypes()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = "single room" ,
                    Text = "Single Room"
                }
                ,
                new SelectListItem
                {
                    Value ="double room" ,
                    Text = "Double Room"
                },
                new SelectListItem
                {
                    Value ="suite" ,
                    Text = "Suite"
                },
                         
                new SelectListItem
                {
                    Value ="family room" ,
                    Text = "Family Room"
                }
            };
        }
        public static List<SelectListItem> LoadRoomFeatures()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = "has kitchenette" ,
                    Text = "Has kitchenette"
                }
                ,
                new SelectListItem
                {
                    Value ="Wi-Fi Availability and Quality." ,
                    Text = "Wi-Fi Availability and Quality."
                },
                new SelectListItem
                {
                    Value ="TV and Entertainment Options." ,
                    Text = "TV and Entertainment Options."
                },

                new SelectListItem
                {
                    Value ="Has workspace" ,
                    Text = "Has workspace"
                }
            };
        }
        public static List<SelectListItem> LoadBusTripFeatures()
        {
            return new List<SelectListItem>()
    {
        new SelectListItem
        {
            Value = "Onboard Restroom" ,
            Text = "Onboard Restroom"
        },
        new SelectListItem
        {
            Value ="Wi-Fi Availability" ,
            Text = "Wi-Fi Availability"
        },
        new SelectListItem
        {
            Value ="Power Outlets" ,
            Text = "Power Outlets"
        },
        new SelectListItem
        {
            Value ="Comfortable Seating" ,
            Text = "Comfortable Seating"
        },
        new SelectListItem
        {
            Value ="Luggage Storage" ,
            Text = "Luggage Storage"
        },
        new SelectListItem
        {
            Value ="Air Conditioning" ,
            Text = "Air Conditioning"
        },
        new SelectListItem
        {
            Value ="Wheelchair Accessibility" ,
            Text = "Wheelchair Accessibility"
        },
        new SelectListItem
        {
            Value ="Onboard Entertainment" ,
            Text = "Onboard Entertainment"
        },
        new SelectListItem
        {
            Value ="USB Charging Ports" ,
            Text = "USB Charging Ports"
        },
        new SelectListItem
        {
            Value ="Audio Commentary" ,
            Text = "Audio Commentary"
        }
        };
        }
        public static List<SelectListItem> LoadVehicleTypes()
        {
            return new List<SelectListItem>()
    {
        new SelectListItem
        {
            Value = "Bus" ,
            Text = "Bus"
        },
        new SelectListItem
        {
            Value ="Coach" ,
            Text = "Coach"
        },
        new SelectListItem
        {
            Value ="Minibus" ,
            Text = "Minibus"
        },
        new SelectListItem
        {
            Value ="Double-decker bus" ,
            Text = "Double-decker bus"
        },
        new SelectListItem
        {
            Value ="School Bus" ,
            Text = "School Bus"
        },
        new SelectListItem
        {
            Value ="Shuttle Bus" ,
            Text = "Shuttle Bus"
        },
        new SelectListItem
        {
            Value ="Tourist Bus" ,
            Text = "Tourist Bus"
        },
        new SelectListItem
        {
            Value ="Charter Bus" ,
            Text = "Charter Bus"
        },
        new SelectListItem
        {
            Value ="City Bus" ,
            Text = "City Bus"
        },
        new SelectListItem
        {
            Value ="Transit Bus" ,
            Text = "Transit Bus"
        }
    };
        }

        public static List<SelectListItem> LoadVehicleCapacities()
        {
            return new List<SelectListItem>()
    {
        new SelectListItem
        {
            Value = "15" ,
            Text = "10-15 passengers"
        },
        new SelectListItem
        {
            Value ="20" ,
            Text = "15-20 passengers"
        },
        new SelectListItem
        {
            Value ="30" ,
            Text = "20-30 passengers"
        },
        new SelectListItem
        {
            Value ="40" ,
            Text = "30-40 passengers"
        },
        new SelectListItem
        {
            Value ="50" ,
            Text = "40-50 passengers"
        },
        new SelectListItem
        {
            Value ="60" ,
            Text = "50-60 passengers"
        },
        new SelectListItem
        {
            Value ="70" ,
            Text = "60-70 passengers"
        },
        new SelectListItem
        {
            Value ="80" ,
            Text = "70-80 passengers"
        },
        new SelectListItem
        {
            Value ="90" ,
            Text = "90+ passengers"
        }
    };
        }

        public static List<SelectListItem> LoadCitiesInEgypt()
        {
            return new List<SelectListItem>()
    {
        new SelectListItem { Value = "Cairo", Text = "Cairo" },
        new SelectListItem { Value = "Alexandria", Text = "Alexandria" },
        new SelectListItem { Value = "Luxor", Text = "Luxor" },
        new SelectListItem { Value = "Aswan", Text = "Aswan" },
        new SelectListItem { Value = "Giza", Text = "Giza" },
        new SelectListItem { Value = "Sharm El Sheikh", Text = "Sharm El Sheikh" },
        new SelectListItem { Value = "Hurghada", Text = "Hurghada" },
        new SelectListItem { Value = "Port Said", Text = "Port Said" },
        new SelectListItem { Value = "Suez", Text = "Suez" },
        new SelectListItem { Value = "Mansoura", Text = "Mansoura" },
        new SelectListItem { Value = "Ismailia", Text = "Ismailia" },
        new SelectListItem { Value = "Fayoum", Text = "Fayoum" },
        new SelectListItem { Value = "Zagazig", Text = "Zagazig" },
        new SelectListItem { Value = "Beni Suef", Text = "Beni Suef" },
        new SelectListItem { Value = "Sohag", Text = "Sohag" },
        new SelectListItem { Value = "Tanta", Text = "Tanta" },
        new SelectListItem { Value = "Damanhur", Text = "Damanhur" },
        new SelectListItem { Value = "Kafr el-Sheikh", Text = "Kafr el-Sheikh" },
        new SelectListItem { Value = "Banha", Text = "Banha" },
        new SelectListItem { Value = "Mallawi", Text = "Mallawi" },
        // Add more cities as needed
    };
        }

        public static List<SelectListItem> loadCompanyVehicles (ApplicationDbContext Context , string Id )
        {
            var db = Context;
            return db.Vehicles.AsNoTracking().Where(v=>v.CompanyId == Id).Select(v =>
             new SelectListItem
             {
                 Value = v.Id.ToString() , 
                 Text = $"{v.Type} with : [{v.LicenseNumber}] and wih Capacity :  {v.Capacity}"
             }).ToList(); 
        }
    }
}
