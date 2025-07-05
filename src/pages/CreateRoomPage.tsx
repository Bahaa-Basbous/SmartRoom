import { useState } from "react";
import axios from "../services/api";
import { useNavigate } from "react-router-dom";

export default function CreateRoomPage() {
  const navigate = useNavigate();

  const [room, setRoom] = useState({
    roomID: 0, // default value if backend requires it
    name: "",
    capacity: 0,
    location: "",
    features: "",
  });

  const [message, setMessage] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.post("/Room", room);
      setMessage("✅ Room created successfully!");
      setTimeout(() => navigate("/dashboard"), 1000);
    } catch (err: any) {
      console.error(err);
      setMessage("❌ Failed to create room. Make sure you're an admin.");
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <form
        onSubmit={handleSubmit}
        className="bg-white p-8 rounded shadow-md w-full max-w-md"
      >
        <h2 className="text-2xl font-bold mb-6 text-center text-indigo-700">
          Create New Room
        </h2>

        <input
          type="text"
          placeholder="Room Name"
          className="w-full p-3 border mb-4 rounded"
          value={room.name}
          onChange={(e) => setRoom({ ...room, name: e.target.value })}
          required
        />

        <input
          type="number"
          placeholder="Capacity"
          className="w-full p-3 border mb-4 rounded"
          value={room.capacity}
          onChange={(e) =>
            setRoom({ ...room, capacity: parseInt(e.target.value) || 0 })
          }
          required
        />

        <input
          type="text"
          placeholder="Location"
          className="w-full p-3 border mb-4 rounded"
          value={room.location}
          onChange={(e) => setRoom({ ...room, location: e.target.value })}
          required
        />

        <input
          type="text"
          placeholder="Features (e.g. Projector, AC)"
          className="w-full p-3 border mb-4 rounded"
          value={room.features}
          onChange={(e) => setRoom({ ...room, features: e.target.value })}
          required
        />

        <button
          type="submit"
          className="bg-indigo-600 text-white w-full p-3 rounded hover:bg-indigo-700 transition"
        >
          Create Room
        </button>

        {message && (
          <p className="mt-4 text-center text-sm text-gray-600">{message}</p>
        )}
      </form>
    </div>
  );
}
