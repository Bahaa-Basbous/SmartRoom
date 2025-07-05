import { useEffect, useState } from "react";
import axios from "../../services/api";

export default function RoomAvailability() {
  const [rooms, setRooms] = useState([]);

  useEffect(() => {
    axios.get("/Room").then((res) => setRooms(res.data));
  }, []);

  return (
    <div className="bg-white rounded-2xl shadow-md p-6 hover:shadow-lg transition">
      <h2 className="text-xl font-bold text-green-600 mb-4 flex items-center gap-2">
        🏢 Room Availability
      </h2>
      {rooms.length === 0 ? (
        <p className="text-gray-500">No rooms available.</p>
      ) : (
        <ul className="space-y-2">
          {rooms.map((r : any) => (
            <li key={r.roomID} className="border border-green-100 p-3 rounded-lg bg-green-50">
              <div className="font-semibold text-green-700">{r.name}</div>
              <div className="text-sm text-gray-500">Capacity: {r.capacity}</div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}