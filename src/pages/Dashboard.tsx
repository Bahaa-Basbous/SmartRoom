import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import UpcomingMeetings from "../components/Dashboard/UpcomingMeetings";
import RoomAvailability from "../components/Dashboard/RoomAvailability";
import RecentMoMs from "../components/Dashboard/RecentMoMs";
import NavBar from "../components/NavBar";

import {
  Bell,
  CalendarCheck,
  DoorOpen,
  FileText,
  CalendarDays,
  LayoutGrid,
} from "lucide-react";

export default function Dashboard() {
  const [role, setRole] = useState<string | null>(null);
  const [name, setName] = useState<string | null>(null);
  const [hasPendingBookings, setHasPendingBookings] = useState<boolean>(false);
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

    if (storedRole === "Admin") {
      fetch("http://localhost:5042/api/Booking/pending-count")
        .then((res) => res.json())
        .then((data) => {
          if (data.count > 0) setHasPendingBookings(true);
        });
    }
  }, [navigate]);

  return (
    <>
      <NavBar />

      <div className="min-h-screen bg-gradient-to-br from-blue-50 to-purple-100 px-6 py-8">
        {/* Header */}
        <div className="flex justify-between items-center bg-white rounded-2xl shadow-xl px-6 py-5 mb-8 border border-blue-100">
          <div>
            <h1 className="text-3xl font-extrabold text-gray-800 mb-1">
              Hello, <span className="text-indigo-600">{name}</span>
            </h1>
            <p className="text-sm text-gray-500">Smart Meeting Room Dashboard</p>
          </div>

          <div className="flex items-center gap-3">
            <span className="text-sm font-semibold px-4 py-1 rounded-full bg-indigo-100 text-indigo-700 shadow">
              Role: {role}
            </span>

            {role === "Admin" && (
              <button
                onClick={() => navigate("/notifications")}
                className="relative text-gray-600 hover:text-indigo-600 transition"
              >
                <Bell className="w-6 h-6" />
                {hasPendingBookings && (
                  <>
                    <span className="absolute -top-1 -right-1 h-3 w-3 rounded-full bg-red-500 animate-ping" />
                    <span className="absolute -top-1 -right-1 h-3 w-3 rounded-full bg-red-500" />
                  </>
                )}
              </button>
            )}
          </div>
        </div>

        {/* Admin Shortcut */}
        {role === "Admin" && (
          <div className="mb-6">
            <button
              onClick={() => navigate("/rooms")}
              className="flex items-center gap-2 bg-indigo-600 text-white px-5 py-2 rounded-lg hover:bg-indigo-700 shadow-lg transition"
            >
              <DoorOpen className="w-5 h-5" />
              Manage Rooms
            </button>
          </div>
        )}

        {/* Dashboard Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-6">
          {/* Upcoming Meetings */}
          <div className="bg-white rounded-2xl shadow-lg hover:shadow-xl transition p-5 border-t-4 border-indigo-400">
            <h2 className="text-lg font-bold mb-3 flex items-center gap-2 text-indigo-600">
              <CalendarCheck className="w-5 h-5" />
              Upcoming Meetings
            </h2>
            <UpcomingMeetings />
          </div>

          {/* Room Availability */}
          <div className="bg-white rounded-2xl shadow-lg hover:shadow-xl transition p-5 border-t-4 border-teal-400">
            <h2 className="text-lg font-bold mb-3 flex items-center gap-2 text-teal-600">
              <LayoutGrid className="w-5 h-5" />
              Room Availability
            </h2>
            <RoomAvailability />
          </div>

          {/* Recent MoMs */}
          {role !== "Guest" && (
            <div className="bg-white rounded-2xl shadow-lg hover:shadow-xl transition p-5 border-t-4 border-yellow-400">
              <h2 className="text-lg font-bold mb-3 flex items-center gap-2 text-yellow-600">
                <FileText className="w-5 h-5" />
                Recent Minutes of Meeting
              </h2>
              <RecentMoMs />
            </div>
          )}

          {/* Calendar View */}
          {role !== "Guest" && (
            <div className="bg-gradient-to-r from-indigo-500 to-blue-500 text-white rounded-2xl shadow-lg p-5 flex flex-col justify-between hover:scale-[1.01] transition">
              <h2 className="text-lg font-bold mb-3 flex items-center gap-2">
                <CalendarDays className="w-5 h-5" />
                Calendar Overview
              </h2>
              <p className="text-sm opacity-90 mb-4">
                View all meetings in a calendar layout.
              </p>
              <button
                onClick={() => navigate("/calendar")}
                className="bg-white text-blue-600 font-semibold px-4 py-2 rounded shadow hover:bg-gray-100 transition"
              >
                📅 Open Calendar
              </button>
            </div>
          )}
        </div>
      </div>
    </>
  );
}
