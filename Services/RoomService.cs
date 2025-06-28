using SmartRoom.Entities;
using SmartRoom.Repositories;
namespace SmartRoom.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<Room>> GetAllAsync() => await _roomRepository.GetAllAsync();

        public async Task<Room?> GetByIdAsync(int id) => await _roomRepository.GetByIdAsync(id);

        public async Task AddAsync(Room room) => await _roomRepository.AddAsync(room);

        public async Task UpdateAsync(Room room) => await _roomRepository.UpdateAsync(room);

        public async Task DeleteAsync(int id) => await _roomRepository.DeleteAsync(id);
    }
}
