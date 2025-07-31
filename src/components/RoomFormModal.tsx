import { useState } from "react";
import axios from "../services/api";

interface Props {
  room: any;
  onClose: () => void;
  onSuccess: () => void;
}

export default function RoomFormModal({ room, onClose, onSuccess }: Props) {
  const [name, setName] = useState(room?.name || "");
  const [capacity, setCapacity] = useState(room?.capacity || 0);
  const [location, setLocation] = useState(room?.location || "");
  const [features, setFeatures] = useState(room?.features || "");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const data = { name, capacity, location, features };
    if (room) {
      await axios.put(`/Room/${room.roomID}`, { ...room, ...data });
    } else {
      await axios.post("/Room", data);
    }
    onSuccess();
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white p-6 rounded-lg w-96">
        <h2 className="text-2xl font-semibold mb-4">
          {room ? "Edit Room" : "Add Room"}
        </h2>
        <form onSubmit={handleSubmit} className="space-y-3">
          <input
            type="text"
            placeholder="Room name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="w-full border p-2 rounded"
            required
          />
          <input
            type="number"
            placeholder="Capacity"
            value={capacity}
            onChange={(e) => setCapacity(Number(e.target.value))}
            className="w-full border p-2 rounded"
            required
          />
          <input
            type="text"
            placeholder="Location"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
            className="w-full border p-2 rounded"
          />
          <input
            type="text"
            placeholder="Features (comma-separated)"
            value={features}
            onChange={(e) => setFeatures(e.target.value)}
            className="w-full border p-2 rounded"
          />

          <div className="flex justify-end gap-3 mt-4">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
            >
              {room ? "Update" : "Create"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}