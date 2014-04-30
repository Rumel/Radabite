using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radabite.Backend.Database;
using Radabite.Backend.Interfaces;

namespace Radabite.Tests.Mocks.Accessors
{
	public class MockEventAccessor : IEventAccessor
	{
		public SaveResult<Event> SaveOrUpdate(Event t)
		{
			t.Id = 1;
			return new SaveResult<Event>(true, t);
		}

		public Event GetById(long id)
		{
			return new Event()
			{
				Id = id,
				Title = "Test Title",
				StartTime = DateTime.Now,
				EndTime = DateTime.Now,
				IsPrivate = false,
				Description = "Test Description",
				Location = new Location()
				{
					LocationName = "Test Location",
					Latitude = 0,
					Longitude = 0,
				},
				IsActive = true,
				Guests = new List<Invitation>(),
				Posts = new List<Post>(),
				Owner = new User() { DisplayName = "test" },
				Votes = new List<Vote>() { new Vote(){ Time = DateTime.Now, UserName = "test"}}
			};
		}


		public IEnumerable<Event> GetAll()
		{
			return new List<Event>
            {
                new Event()
				{ 
					Guests = new List<Invitation>(){ new Invitation() },
					Posts = new List<Post>{ new Post(){ BlobId = "blob" } },
					StartTime = DateTime.Now.AddDays(-2),
					EndTime = DateTime.Now.AddDays(-2),
					Location = new Location(){Latitude=0,Longitude=0,LocationName="testLocation"}
				},
				new Event()
				{ 
					Guests = new List<Invitation>(){ new Invitation() },
					Posts = new List<Post>{ new Post(){ BlobId = "blob" } },
					StartTime = DateTime.Now.AddHours(1),
					EndTime = DateTime.Now.AddHours(3),
					Location = new Location(){Latitude=0,Longitude=0,LocationName="testLocation"}
				},
				new Event()
				{ 
					Guests = new List<Invitation>(),
					Posts = new List<Post>{ new Post(){ BlobId = "blob" } },
					StartTime = DateTime.Now.AddDays(2),
					EndTime = DateTime.Now.AddDays(2),
					Location = new Location(){Latitude=0,Longitude=0,LocationName="testLocation"}
				},
				new Event()
				{ 
					Guests = new List<Invitation>(),
					Posts = new List<Post>{ new Post(){ BlobId = "blob" } },
					StartTime = DateTime.Now.AddDays(2),
					EndTime = DateTime.Now.AddDays(2),
					Location = new Location(){Latitude=0,Longitude=0,LocationName="testLocation"}

				},
				new Event()
				{ 
					Guests = new List<Invitation>(),
					Posts = new List<Post>{ new Post(){ BlobId = "blob" } },
					StartTime = DateTime.Now.AddDays(2),
					EndTime = DateTime.Now.AddDays(2),
					Location = new Location(){Latitude=0,Longitude=0,LocationName="testLocation"}

				}
            };
		}

		public List<Event> GetByOwnerId(long ownerId)
		{
			return new List<Event>
            {
                new Event(){Location = new Location(){Latitude=0,Longitude=0,LocationName="testLocation"}},
                new Event(){Location = new Location(){Latitude=0,Longitude=0,LocationName="testLocation"}},
                new Event(){Location = new Location(){Latitude=0,Longitude=0,LocationName="testLocation"}},
                new Event(){Location = new Location(){Latitude=0,Longitude=0,LocationName="testLocation"}},
                new Event(){Location = new Location(){Latitude=0,Longitude=0,LocationName="testLocation"}}
            };
		}

		public List<Event> GetByGuestId(long guestId)
		{
			return new List<Event>
            {
                new Event(),
                new Event(),
                new Event(),
                new Event(),
                new Event()
            };
		}
	}
}
