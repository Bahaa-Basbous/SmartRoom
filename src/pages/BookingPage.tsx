import { useEffect, useState } from "react";
import axios from "../services/api";
import NavBar from "../components/NavBar";
import { toast } from "react-toastify";
import {
  CalendarPlus,
  Clock,
  DoorOpen,
  Hourglass,
  CheckCircle,
  XCircle,
} from "lucide-react";

export default function BookingPage() {
  const [rooms, setRooms] = useState<any[]>([]);
  const [bookings, setBookings] = useState<any[]>([]);
  const [roomId, setRoomId] = useState("");
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const userId = localStorage.getItem("userId");
  const role = localStorage.getItem("role");

  useEffect(() => {
    axios.get("/Room").then((res) => setRooms(res.data));
    axios.get("/Booking").then((res) => setBookings(res.data));
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (new Date(startTime) >= new Date(endTime)) {
      toast.warning("End time must be after start time.");
      return;
    }

    setIsSubmitting(true);
    try {
      await axios.post("/Booking", {
        roomId: parseInt(roomId),
        userId: parseInt(userId!),
        startTime,
        endTime,
      });

      toast.success("✅ Room booked successfully!");
      setRoomId("");
      setStartTime("");
      setEndTime("");

      const res = await axios.get("/Booking");
      setBookings(res.data);
    } catch (err: any) {
      if (err.response?.status === 409) {
        toast.error("⚠️ Booking failed: The room is already booked for this time slot.");
      } else if (err.response?.data?.message) {
        toast.error(`❌ Booking failed: ${err.response.data.message}`);
      } else {
        toast.error("❌ Booking failed. Please check your input and try again.");
      }
    } finally {
      setIsSubmitting(false);
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case "Approved":
        return "text-green-600";
      case "Pending":
        return "text-yellow-600";
      case "Rejected":
        return "text-red-600";
      default:
        return "text-gray-600";
    }
  };

  return (
    <>
      <NavBar />

      <div className="min-h-screen bg-gradient-to-br from-blue-50 to-purple-100 px-4 py-8">
        {/* Booking Form */}
        <div className="max-w-2xl mx-auto p-6 bg-white rounded-2xl shadow-xl border border-blue-100">
          <h2 className="text-2xl font-bold text-gray-800 mb-6 flex items-center gap-2">
            <CalendarPlus className="w-6 h-6 text-indigo-600" />
            Book a Room
          </h2>

          <form onSubmit={handleSubmit} className="space-y-5">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Select Room</label>
              <select
                value={roomId}
                onChange={(e) => setRoomId(e.target.value)}
                className="w-full border border-gray-300 rounded-lg px-4 py-2 focus:ring-indigo-500 focus:border-indigo-500"
                required
              >
                <option value="">-- Choose a Room --</option>
                {rooms.map((room) => (
                  <option key={room.roomID} value={room.roomID}>
                    {room.name} (Capacity: {room.capacity})
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Start Time</label>
              <input
                type="datetime-local"
                value={startTime}
                onChange={(e) => setStartTime(e.target.value)}
                className="w-full border border-gray-300 rounded-lg px-4 py-2 focus:ring-indigo-500 focus:border-indigo-500"
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">End Time</label>
              <input
                type="datetime-local"
                value={endTime}
                onChange={(e) => setEndTime(e.target.value)}
                className="w-full border border-gray-300 rounded-lg px-4 py-2 focus:ring-indigo-500 focus:border-indigo-500"
                required
              />
            </div>

            <button
              type="submit"
              disabled={isSubmitting}
              className="w-full bg-indigo-600 text-white font-semibold px-4 py-2 rounded-lg hover:bg-indigo-700 transition disabled:opacity-50"
            >
              {isSubmitting ? "Booking..." : "Book Room"}
            </button>
          </form>
        </div>

        {/* Booking List */}
        <div className="max-w-5xl mx-auto mt-12 p-6 bg-white rounded-2xl shadow-xl border border-gray-200">
          <h3 className="text-xl font-bold text-gray-800 mb-2 flex items-center gap-2">
            <Clock className="w-5 h-5 text-blue-600" />
            Bookings
          </h3>
          <p className="text-sm text-gray-500 mb-4">
            {role === "Admin" ? "Showing all bookings." : "Showing only your bookings."}
          </p>

          {bookings.length === 0 ? (
            <p className="text-gray-600">No bookings found.</p>
          ) : (
            <div className="overflow-x-auto">
              <table className="min-w-full border text-sm text-gray-800">
                <thead>
                  <tr className="bg-gray-100 text-left text-sm font-semibold text-gray-700">
                    <th className="p-3 border-b">Room</th>
                    <th className="p-3 border-b">Start Time</th>
                    <th className="p-3 border-b">End Time</th>
                    <th className="p-3 border-b">Status</th>
                  </tr>
                </thead>
                <tbody>
                  {bookings.map((booking) => (
                    <tr
                      key={booking.bookingId}
                      className="hover:bg-gray-50 transition"
                    >
                      <td className="p-3 border-b">
                        {booking.room?.name || `Room #${booking.roomId}`}
                      </td>
                      <td className="p-3 border-b">
                        {new Date(booking.startTime).toLocaleString()}
                      </td>
                      <td className="p-3 border-b">
                        {new Date(booking.endTime).toLocaleString()}
                      </td>
                      <td className={`p-3 border-b font-semibold ${getStatusColor(booking.status)}`}>
                        {booking.status}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>
    </>
  );
}
