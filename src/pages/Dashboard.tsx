import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import UpcomingMeetings from "../components/Dashboard/UpcomingMeetings";
import RoomAvailability from "../components/Dashboard/RoomAvailability";
import RecentMoMs from "../components/Dashboard/RecentMoMs";
import Notifications from "../components/Dashboard/Notifications";

export default function Dashboard() {
  const [role, setRole] = useState<string | null>(null);
  const [name, setName] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const storedRole = localStorage.getItem("role");
    const storedName = localStorage.getItem("name");
    if (!storedRole || !storedName) {
      navigate("/");
    } else {
      setRole(storedRole);
      setName(storedName);
    }
  }, [navigate]);

  const handleLogout = () => {
    localStorage.clear();
    navigate("/");
  };

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      {/* Header */}
      <div className="flex justify-between items-center mb-6 bg-white rounded-xl shadow px-6 py-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-800">
            Welcome, <span className="text-indigo-600">{name}</span>{" "}
            <span className="text-sm text-gray-500">({role})</span>
          </h1>
          <p className="text-sm text-gray-400">Smart Meeting Room Dashboard</p>
        </div>
        <button
          onClick={handleLogout}
          className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
        >
          Logout
        </button>
      </div>

      {/* Admin Create Room Button */}
      {role === "Admin" && (
        <div className="mb-6">
          <button
            onClick={() => navigate("/create-room")}
            className="bg-indigo-600 text-white px-5 py-2 rounded hover:bg-indigo-700 shadow"
          >
            ➕ Create New Room
          </button>
        </div>
      )}

      {/* Dashboard Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-6">
        {/* Common Components */}
        <UpcomingMeetings />
        <RoomAvailability />
        {role !== "Guest" && <RecentMoMs />}
        <Notifications />
      </div>
    </div>
  );
}
