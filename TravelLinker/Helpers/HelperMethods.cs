using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinker.Helpers
{
    public static class HelperMethods
    {
        public static string FindRateType(double rate)
        {
            if (rate <= 1)
                return "Very Bad";
            if (rate <= 2)
                return "Bad";
            if (rate <= 3)
                return "Good";

            if (rate <= 4)
                return "Very Good";

            return "Excellent";

        }


        public static List<RoomImage> UrlImagesRoom(string roomId, string _path, IFormFileCollection files)
        {
            List<RoomImage> Urls = new List<RoomImage>();
            // Create Directory For This User 
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            foreach (var file in files)
            {
                var name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                using FileStream stream =
                         new FileStream(Path.Combine(_path, name), FileMode.Create);
                file.CopyTo(stream);

                Urls.Add(new RoomImage
                {
                    ImageUrl = name,
                    RoomId = roomId
                });
            }
            return Urls;
        }
        public static void DeleteFile(string _path)
        {
          
                if (File.Exists(_path))
                {
                    File.Delete(_path);
                }
            
        }
        //--- 
        public static Room MapToRoom(UpDateCreateRoomViewModel model)
        {
            return new Room
            {
                Id = model.RoomId == null ?
                         Guid.NewGuid().ToString() : model.RoomId,
                Type = model.Type,
                BedsNumber = model.BedsNumber,  
                HotelId = model.HotelId,
                PricePerMonth = model.PricePerMonth,
                PricePerNight = model.PricePerNight,
                PricePerWeek = model.PricePerWeek,
                Number = model.Number,
                FloorNumber = model.FloorNumber,
            };
        }

        public static List<OverViewRoomViewModel> OverViewRoomMap(IEnumerable<Room> rooms)
        {
            return rooms.Select(room => new OverViewRoomViewModel
            {
                Id = room.Id,
                HotelId = room.HotelId,
                Type = room.Type,
                MainImage = room.ImageUrl!,
                Price = room.PricePerNight,
                OfferPrice = room.RoomOffer != null ?
                          room.RoomOffer.OfferPrice : null
            }).ToList();

        }
   
        public static UpDateCreateRoomViewModel MapToUpdateViewModel (Room room)
        {
            return new UpDateCreateRoomViewModel
            {
                RoomId = room.Id,   
                BedsNumber = room.BedsNumber,
                HotelId = room.HotelId,
                PricePerMonth = room.PricePerMonth,
                PricePerNight = room.PricePerNight,
                PricePerWeek = room.PricePerWeek,
                Number = room.Number,
                FloorNumber = room.FloorNumber,
                Features = room.RoomFeatures.Select(rf => rf.FeatureName).ToList(),
                ImagesUrls = room.RoomImages.Select(rm =>rm.ImageUrl).ToList(),  
            }; 
        }

        static private List<string> _allows = new List<string>
          {
               ".jpg"  , ".png"
          };
        public static bool CheckExtension(IFormFile file)
        {
            return _allows.Contains(Path.GetExtension(file.FileName.ToLower()));
        }


      
        public static string  UrlVecicleImage( string _path, IFormFile file)
        {
         
            // Create Directory For This User 
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
         
                var name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                using FileStream stream =
                         new FileStream(Path.Combine(_path, name), FileMode.Create);
                file.CopyTo(stream);

            return name;
        }


    }
}
