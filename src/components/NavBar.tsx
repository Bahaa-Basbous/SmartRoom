import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "../services/api";

export default function NavBar() {
  const role = localStorage.getItem("role");
  const navigate = useNavigate();
  const [pendingCount, setPendingCount] = useState(0);

  useEffect(() => {
    if (role === "Admin") {
      axios.get("/Booking/pending/count")
        .then(res => setPendingCount(res.data))
        .catch(() => setPendingCount(0));
    }
  }, [role]);



  return (
    <nav className="bg-gray-800 text-white px-6 py-3 flex justify-between items-center">
      <div className="flex space-x-4 items-center">
        <Link to="/dashboard" className="hover:underline">Dashboard</Link>
         <Link to="/profile" className="hover:underline">My Profile</Link>
        <Link to="/calendar" className="hover:underline">Calendar</Link>
        {role === "Admin" && (
          <>
            <Link to="/rooms" className="hover:underline">Rooms</Link>
            <Link to="/bookings" className="hover:underline">Bookings</Link>
            <Link to="/meetings" className="hover:underline">Meetings</Link>
            <Link to="/profile" className="hover:underline">My Profile</Link>
            {/* Pending Bookings Link with badge */}
            <Link to="/admin/pending-bookings" className="relative hover:underline flex items-center">
              Pending Bookings
              {pendingCount > 0 && (
                <span className="ml-1 inline-flex items-center justify-center px-2 py-0.5 text-xs font-bold leading-none text-red-100 bg-red-600 rounded-full">
                  {pendingCount}
                </span>
              )}
            </Link>
          </>
        )}

        {role === "Employee" && (
          <>
            <Link to="/bookings" className="hover:underline">Bookings</Link>
            <Link to="/meetings" className="hover:underline">Meetings</Link>
            <Link to="/profile" className="hover:underline">My Profile</Link>
          </>
        )}

        {["Admin", "Employee"].includes(role || "") && (
          <Link
            to="/mom/create"
            className="bg-purple-600 text-white px-3 py-1 rounded hover:bg-purple-700"
          >
            + Create MoM
          </Link>
        )}
      </div>

    
    </nav>
  );
}
