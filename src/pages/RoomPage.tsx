import { useEffect, useState } from "react";
import axios from "../services/api";
import RoomCard from "../components/RoomCard";
import RoomFormModal from "../components/RoomFormModal";
import NavBar from "../components/NavBar";

export default function RoomPage() {
  const [rooms, setRooms] = useState<any[]>([]);
  const [showModal, setShowModal] = useState(false);
  const [editingRoom, setEditingRoom] = useState<any | null>(null);
  const role = localStorage.getItem("role");

  const fetchRooms = async () => {
    const res = await axios.get("/Room");
    setRooms(res.data);
  };

  useEffect(() => {
    fetchRooms();
  }, []);

  const handleDelete = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this room?")) return;
    await axios.delete(`/Room/${id}`);
    fetchRooms();
  };

  return (
    <> <NavBar /> <div className="p-6">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-3xl font-bold">Meeting Rooms</h1>
        {role === "Admin" && (
          <button
            onClick={() => {
              setEditingRoom(null);
              setShowModal(true);
            }}
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
          >
            + Add Room
          </button>
        )}
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        {rooms.map((room) => (
          <RoomCard
            key={room.roomID}
            room={room}
            role={role!}
            onEdit={() => {
              setEditingRoom(room);
              setShowModal(true);
            }}
            onDelete={() => handleDelete(room.roomID)}
          />
        ))}
      </div>

      {showModal && (
        <RoomFormModal
          room={editingRoom}
          onClose={() => setShowModal(false)}
          onSuccess={() => {
            setShowModal(false);
            fetchRooms();
          }}
        />
      )}
    </div></>
    
  );
}
